using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace NoiseMapGenerator.Models
{
    public class OctaveWeight
    {
        private float _weight;

        public float Weight
        {
            get { return _weight; }
            set
            {
                if (value < 0.0f)
                    _weight = 0.0f;
                else if (value > 1.0f)
                    _weight = 1.0f;
                else _weight = value;
            }
        }
        public int Index { get; set; }
        public bool Enabled { get; set; }

        public OctaveWeight(float weight, int index, bool enabled)
        {
            Weight = _weight = weight;
            Index = index;
            Enabled = enabled;
        }
    }

    public class NoiseData
    {
        private int _dimensions;
        public int Dimensions { get { return _dimensions; }
            set
            {
                if (value < 1)
                    _dimensions = 1;
                else if (value > 3)
                    _dimensions = 3;
                else _dimensions = value;
            }
            
        }

        public int Resolution { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public double DPI { get; set; }

        private float _xSeed;

        public float XSeed
        {
            get { return _xSeed; }
            set
            {
                if (value < -1.0f)
                    _xSeed = -1.0f;
                else if (value > 1.0f)
                    _xSeed = 1.0f;
                else _xSeed = value;
            }
        }

        private float _ySeed;

        public float YSeed
        {
            get { return _ySeed; }
            set
            {
                if (value < -1.0f)
                    _ySeed = -1.0f;
                else if (value > 1.0f)
                    _ySeed = 1.0f;
                else _ySeed = value;
            }
        }

        private float _zSeed;

        public float ZSeed
        {
            get { return _zSeed; }
            set
            {
                if (value < -1.0f)
                    _zSeed = -1.0f;
                else if (value > 1.0f)
                    _zSeed = 1.0f;
                else _zSeed = value;
            }
        }

        private float _brightness;

        public float Brightness
        {
            get { return _brightness; }
            set
            {
                if (value < -1.0f)
                    _brightness = -1.0f;
                else if (value > 1.0f)
                    _brightness = 1.0f;
                else _brightness = value;
            }
        }

        private float _frequency;
        public float Frequency { get { return _frequency; }
            set
            {
                if (value < 0.0f)
                    _frequency = 0.0f;
                else if (value > 50.0f)
                    _frequency = 50.0f;
                else _frequency = value;
            } 
        }

        private int _octaves;
        public int Octaves { get { return _octaves; }
            set
            {
                if (value < 0)
                    _octaves = 0;
                else if (value > 6)
                    _octaves = 6;
                else _octaves = value;
                if (!UseWeights)
                    return;
                for (int i = 0; i < 6; i++)
                {
                    if (OctaveWeights == null)
                        return;
                    if (i <= value)
                    {
                        OctaveWeights[i].Enabled = true;
                    }
                    if (i > value)
                    {
                        OctaveWeights[i].Enabled = false;
                    }
                }
            } 
        }

        private bool _useWeights;
        public bool UseWeights { get { return _useWeights; }
            set
            {
                _useWeights = value;
                if (OctaveWeights == null) return;
                for (int i = 0; i < 6; i++)
                {
                    if (i <= Octaves)
                    {
                        OctaveWeights[i].Enabled = true;
                    }
                    if (i >= _octaves)
                        OctaveWeights[i].Enabled = false;
                }
            } 
        }

        public ObservableCollection<OctaveWeight> OctaveWeights { get; set; }

        private float _lacunarity;

        public float Lacunarity { get { return _lacunarity; }
            set
            {
                if (value < 1.0f)
                    _lacunarity = 1.0f;
                else if (_lacunarity > 4.0f)
                    _lacunarity = 4.0f;
                else _lacunarity = value;
            }
        }

        private float _persistence;

        public float Persistence { get { return _persistence; }
            set
            {
                if (value < 0.0f)
                    _persistence = 0.0f;
                else if (value > 1.0f)
                    _persistence = 1.0f;
                else _persistence = value;
            }
        }

        public bool Turbulence { get; set; }
        private int _grain;
        public int Grain { get { return _grain; }
            set
            {
                if (value < 1)
                    _grain = 1;
                else if (value > 10)
                    _grain = 10;
                else _grain = value;
            } 
        }
        public NoiseMethodType Type { get; set; }
        public BitmapSource NoiseMap { get; set; }
        public List<float> NoiseValueData { get; set; }

        public NoiseData()
        {
            Reset();
        }

        public void Reset()
        {
            Dimensions = 2;
            Resolution = 256;
            Width = Height = Resolution;
            DPI = 300.0;
            XSeed = 0.0f;
            YSeed = 0.0f;
            ZSeed = 0.0f;
            Brightness = 0.0f;
            Frequency = 5.0f;
            Octaves = _octaves = 0;
            UseWeights = false;
            OctaveWeights = new ObservableCollection<OctaveWeight>();
            for (int i = 0; i < 6; i++)
            {
                OctaveWeights.Add((i <= Octaves) ? new OctaveWeight(1.0f, i, UseWeights) : new OctaveWeight(0.0f, i, UseWeights));
            }
            Lacunarity = 2.0f;
            Persistence = 0.5f;
            Turbulence = false;
            Grain = 1;
            Type = NoiseMethodType.Perlin;
        }
    }
}
