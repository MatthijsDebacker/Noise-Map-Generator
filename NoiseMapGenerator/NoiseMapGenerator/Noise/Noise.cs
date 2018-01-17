using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using NoiseMapGenerator.Models;
using Xceed.Wpf.Toolkit;

public delegate float NoiseMethod(Vector3D point);

public enum NoiseMethodType
{
    Value,
    Perlin
}

namespace NoiseMapGenerator.Noise
{
    class Noise
    {
        private static readonly int[] Hash =
        {
            151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
            140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
            247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
            57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
            74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
            60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
            65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
            200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
            52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
            207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
            119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
            129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
            218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
            81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
            184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
            222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

            151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
            140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
            247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
            57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
            74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
            60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
            65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
            200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
            52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
            207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
            119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
            129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
            218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
            81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
            184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
            222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
        };

        private const int HashMask = 255;

        private static readonly float[] Gradients1D =
        {
            1.0f, -1.0f
        };

        private const int GradientsMask1D = 1;

        private static readonly Vector[] Gradients2D =
        {
            new Vector(1.0f, 0.0f),
            new Vector(-1.0f, 0.0f),
            new Vector(0.0f, 1.0f),
            new Vector(0.0f, -1.0f),
            new Vector(1.0f, 1.0f),
            new Vector(-1.0f, 1.0f),
            new Vector(1.0f, -1.0f),
            new Vector(-1.0f, -1.0f)
        };

        private const int GradientsMask2D = 7;

        private static readonly Vector3D[] Gradients3D =
        {
            new Vector3D( 1f, 1f, 0f),
            new Vector3D(-1f, 1f, 0f),
            new Vector3D( 1f,-1f, 0f),
            new Vector3D(-1f,-1f, 0f),
            new Vector3D( 1f, 0f, 1f),
            new Vector3D(-1f, 0f, 1f),
            new Vector3D( 1f, 0f,-1f),
            new Vector3D(-1f, 0f,-1f),
            new Vector3D( 0f, 1f, 1f),
            new Vector3D( 0f,-1f, 1f),
            new Vector3D( 0f, 1f,-1f),
            new Vector3D( 0f,-1f,-1f),
                       
            new Vector3D( 1f, 1f, 0f),
            new Vector3D(-1f, 1f, 0f),
            new Vector3D( 0f,-1f, 1f),
            new Vector3D( 0f,-1f,-1f)
        };

        private const int GradientsMask3D = 15;

        public static float Sqr2 = (float)Math.Sqrt(2.0);

        public static NoiseMethod[] ValueMethods =
        {
            Value1D,
            Value2D,
            Value3D
        };

        public static NoiseMethod[] PerlinMethods =
        {
            Perlin1D,
            Perlin2D,
            Perlin3D
        };

        public static NoiseMethod[][] Methods =
        {
            ValueMethods,
            PerlinMethods
        };

        public static float Dot(Vector g, float x, float y)
        {
            return (float)(g.X * x + g.Y * y);
        }

        private static float Dot(Vector3D g, float x, float y, float z)
        {
            return (float)(g.X * x + g.Y * y + g.Z * z);
        }

        private static float Smooth(float t)
        {
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
        }

        public static float Lerp(float a, float b, float by)
        {
            return ((b - a) * by) + a;
        }

        public static Vector3D Lerp(Vector3D a, Vector3D b, float by)
        {
            return a * by + b * (1.0f - by);
        }

        public static void NormalizeGradients2D()
        {
            Gradients2D[4].Normalize();
            Gradients2D[5].Normalize();
            Gradients2D[6].Normalize();
            Gradients2D[7].Normalize();
        }

        public static float Sum(NoiseMethod method, Vector3D point, float frequency, int octaves, float lacunarity, float persistence)
        {
            var sum = method(point * frequency);
            var amplitude = 1.0f;
            var range = 1.0f;
            for (var o = 0; o < octaves; o++)
            {
                frequency *= lacunarity;
                amplitude *= persistence;
                range += amplitude;
                sum += method(point * frequency) * amplitude;
            }
            return sum / range;
        }

        public static float Sum(NoiseMethod method, Vector3D point, float frequency, int octaves,
            List<OctaveWeight> octaveWeights)
        {
            var sum = method(point * frequency);
            float range = 1.0f;
            for (int o = 0; o < octaves; o++)
            {
                if(octaveWeights[o].Weight <= 0.0f) continue;
                frequency *= 2.0f;
                range += octaveWeights[o].Weight;
                sum += method(point * frequency) * octaveWeights[o].Weight;
            }
            return sum / range;
        }

        public static float Value1D(Vector3D point)
        {
            var i0 = (int)Math.Floor(point.X);
            var t = (float)point.X - i0;
            i0 &= HashMask;
            var i1 = i0 + 1;
            
            var h0 = Hash[i0];
            var h1 = Hash[i1];

            t = Smooth(t);
            return Lerp(h0, h1, t) * (1.0f / HashMask);
        }

        public static float Value2D(Vector3D point)
        {
            var ix0 = (int)Math.Floor(point.X);
            var iy0 = (int)Math.Floor(point.Y);
            var tx = (float)point.X - ix0;
            var ty = (float)point.Y - iy0;

            ix0 &= HashMask;
            iy0 &= HashMask;

            var ix1 = ix0 + 1;
            var iy1 = iy0 + 1;

            var h0 = Hash[ix0];
            var h1 = Hash[ix1];
            var h00 = Hash[h0 + iy0];
            var h10 = Hash[h1 + iy0];
            var h01 = Hash[h0 + iy1];
            var h11 = Hash[h1 + iy1];

            tx = Smooth(tx);
            ty = Smooth(ty);
            return Lerp(
                       Lerp(h00, h10, tx),
                       Lerp(h01, h11, tx),
                       ty) * Sqr2;   
        }

        public static float Value3D(Vector3D point)
        {
            var ix0 = (int)Math.Floor(point.X);
            var iy0 = (int)Math.Floor(point.Y);
            var iz0 = (int)Math.Floor(point.Z);
            var tx = (float)point.X - ix0;
            var ty = (float)point.Y - iy0;
            var tz = (float)point.Z - iz0;
            ix0 &= HashMask;
            iy0 &= HashMask;
            iz0 &= HashMask;
            var ix1 = ix0 + 1;
            var iy1 = iy0 + 1;
            var iz1 = iz0 + 1;

            var h0 = Hash[ix0];
            var h1 = Hash[ix1];
            var h00 = Hash[h0 + iy0];
            var h10 = Hash[h1 + iy0];
            var h01 = Hash[h0 + iy1];
            var h11 = Hash[h1 + iy1];
            var h000 = Hash[h00 + iz0];
            var h100 = Hash[h10 + iz0];
            var h010 = Hash[h01 + iz0];
            var h110 = Hash[h11 + iz0];
            var h001 = Hash[h00 + iz1];
            var h101 = Hash[h10 + iz1];
            var h011 = Hash[h01 + iz1];
            var h111 = Hash[h11 + iz1];

            tx = Smooth(tx);
            ty = Smooth(ty);
            tz = Smooth(tz);
            return Lerp(
                       Lerp(Lerp(h000, h100, tx), Lerp(h010, h110, tx), ty),
                       Lerp(Lerp(h001, h101, tx), Lerp(h011, h111, tx), ty),
                       tz) * (1f / HashMask);
        }

        public static float Perlin1D(Vector3D point)
        {
            var i0 = (int)Math.Floor(point.X);
            var t0 = (float)point.X - i0;
            var t1 = t0 - 1.0f;
            i0 &= HashMask;
            var i1 = i0 + 1;

            var g0 = Gradients1D[Hash[i0] & GradientsMask1D];
            var g1 = Gradients1D[Hash[i1] & GradientsMask1D];

            var v0 = g0 * t0;
            var v1 = g1 * t1;

            var t = Smooth(t0);
            return Lerp(v0, v1, t) * 2.0f;
        }

        public static float Perlin2D(Vector3D point)
        {
            var ix0 = (int)Math.Floor(point.X);
            var iy0 = (int)Math.Floor(point.Y);
            var tx0 = (float)point.X - ix0;
            var ty0 = (float)point.Y - iy0;
            var tx1 = tx0 - 1f;
            var ty1 = ty0 - 1f;

            ix0 &= HashMask;
            iy0 &= HashMask;

            var ix1 = ix0 + 1;
            var iy1 = iy0 + 1;

            var h0 = Hash[ix0];
            var h1 = Hash[ix1];

            var g00 = Gradients2D[Hash[h0 + iy0] & GradientsMask2D];
            var g10 = Gradients2D[Hash[h1 + iy0] & GradientsMask2D];
            var g01 = Gradients2D[Hash[h0 + iy1] & GradientsMask2D];
            var g11 = Gradients2D[Hash[h1 + iy1] & GradientsMask2D];

            var v00 = Dot(g00, tx0, ty0);
            var v10 = Dot(g10, tx1, ty0);
            var v01 = Dot(g01, tx0, ty1);
            var v11 = Dot(g11, tx1, ty1);

            var tx = Smooth(tx0);
            var ty = Smooth(ty0);
            return Lerp(
                       Lerp(v00, v10, tx),
                       Lerp(v01, v11, tx),
                       ty) * Sqr2;
        }

        public static float Perlin3D(Vector3D point)
        {
            var ix0 = (int)Math.Floor(point.X);
            var iy0 = (int)Math.Floor(point.Y);
            var iz0 = (int)Math.Floor(point.Z);
            var tx0 = (float)point.X - ix0;
            var ty0 = (float)point.Y - iy0;
            var tz0 = (float)point.Z - iz0;
            var tx1 = tx0 - 1f;
            var ty1 = ty0 - 1f;
            var tz1 = tz0 - 1f;
            ix0 &= HashMask;
            iy0 &= HashMask;
            iz0 &= HashMask;
            var ix1 = ix0 + 1;
            var iy1 = iy0 + 1;
            var iz1 = iz0 + 1;

            var h0 = Hash[ix0];
            var h1 = Hash[ix1];
            var h00 = Hash[h0 + iy0];
            var h10 = Hash[h1 + iy0];
            var h01 = Hash[h0 + iy1];
            var h11 = Hash[h1 + iy1];
            Vector3D g000 = Gradients3D[Hash[h00 + iz0] & GradientsMask3D];
            Vector3D g100 = Gradients3D[Hash[h10 + iz0] & GradientsMask3D];
            Vector3D g010 = Gradients3D[Hash[h01 + iz0] & GradientsMask3D];
            Vector3D g110 = Gradients3D[Hash[h11 + iz0] & GradientsMask3D];
            Vector3D g001 = Gradients3D[Hash[h00 + iz1] & GradientsMask3D];
            Vector3D g101 = Gradients3D[Hash[h10 + iz1] & GradientsMask3D];
            Vector3D g011 = Gradients3D[Hash[h01 + iz1] & GradientsMask3D];
            Vector3D g111 = Gradients3D[Hash[h11 + iz1] & GradientsMask3D];

            float v000 = Dot(g000, tx0, ty0, tz0);
            float v100 = Dot(g100, tx1, ty0, tz0);
            float v010 = Dot(g010, tx0, ty1, tz0);
            float v110 = Dot(g110, tx1, ty1, tz0);
            float v001 = Dot(g001, tx0, ty0, tz1);
            float v101 = Dot(g101, tx1, ty0, tz1);
            float v011 = Dot(g011, tx0, ty1, tz1);
            float v111 = Dot(g111, tx1, ty1, tz1);

            float tx = Smooth(tx0);
            float ty = Smooth(ty0);
            float tz = Smooth(tz0);
            return Lerp(
                Lerp(Lerp(v000, v100, tx), Lerp(v010, v110, tx), ty),
                Lerp(Lerp(v001, v101, tx), Lerp(v011, v111, tx), ty),
                tz);
        }
    }
}
