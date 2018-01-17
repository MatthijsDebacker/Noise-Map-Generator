using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using GalaSoft.MvvmLight.Command;
using NoiseMapGenerator.Annotations;
using NoiseMapGenerator.Models;
using NoiseG = NoiseMapGenerator.Noise.Noise;
using static NoiseMapGenerator.Helpers.ExtensionMethods;

namespace NoiseMapGenerator.ViewModels
{
    class NoiseViewModel:INotifyPropertyChanged
    {
        private NoiseData _noiseData;

        public NoiseData NoiseData
        {
            get { return _noiseData; }
            set
            {
                _noiseData = value;
                OnPropertyChanged("NoiseData");
            }
        }

        private int _resolutionScale;

        public int ResolutionScale
        {
            get { return _resolutionScale; }
            set
            {
                _resolutionScale = value;
                var nd = NoiseData;
                var res = 256;
                for (int i = 0; i < _resolutionScale; i++)
                {
                    res *= 2;
                }
                nd.Resolution = res;
                nd.Width = nd.Height = res;
                NoiseData = nd;
                OnPropertyChanged("ResolutionScale");
            }
        }

        public RelayCommand GenerateMapCommand { get; set; }
        public RelayCommand ResetMapCommand { get; set; }

        public NoiseViewModel()
        {
            GenerateMapCommand = new RelayCommand(GenerateMap);
            ResetMapCommand = new RelayCommand(Reset);
            NoiseData = new NoiseData();
            NoiseG.NormalizeGradients2D();
            ResolutionScale = 0;
        }

        private void Reset()
        {
            _noiseData.Reset();
            ResolutionScale = 0;
            GenerateMap();
        }
        private void GenerateMap()
        {
            var nd = NoiseData;
            var dpi = _noiseData.DPI;
            var resolution = _noiseData.Resolution;

            byte[] pixelData = new byte[resolution * resolution];
            _noiseData.NoiseValueData = new List<float>();

            Vector3D p00 = new Vector3D(-0.5, -0.5, 0.0);
            Vector3D p10 = new Vector3D(0.5, -0.5, 0.0);
            Vector3D p01 = new Vector3D(-0.5, 0.5, 0.0);
            Vector3D p11 = new Vector3D(0.5, 0.5, 0.0);

            var stepSize = 1.0f / resolution;

            var method = NoiseG.Methods[(int) nd.Type][nd.Dimensions - 1];

            for (int y = 0; y < resolution; y++)
            {
                Vector3D p0 = NoiseG.Lerp(p00, p01, (y + 0.5f) * stepSize);
                Vector3D p1 = NoiseG.Lerp(p10, p11, (y + 0.5f) * stepSize);

                for (int x = 0; x < resolution; x++)
                {
                    Vector3D p = NoiseG.Lerp(p0, p1, (x + 0.5f) * stepSize);
                    p.X += _noiseData.XSeed;
                    p.Y += _noiseData.YSeed;
                    p.Z += _noiseData.ZSeed;

                    float sample;

                    if (_noiseData.UseWeights)
                        sample = NoiseG.Sum(method, p, _noiseData.Frequency, _noiseData.Octaves,
                            _noiseData.OctaveWeights.ToList());
                    else
                        sample = NoiseG.Sum(method, p, _noiseData.Frequency, _noiseData.Octaves,
                            _noiseData.Lacunarity, _noiseData.Persistence);

                    if (_noiseData.Type != NoiseMethodType.Value)
                        sample = sample * 0.5f + 0.5f;

                    if (_noiseData.Turbulence)
                        sample = Math.Abs(2.0f * (sample - 0.5f));

                    sample += _noiseData.Brightness;
                    sample = sample.Clamp(0.0f, 1.0f);

                    sample *= _noiseData.Grain;

                    _noiseData.NoiseValueData.Add(sample);
                    pixelData[x + y * resolution] = (byte)(sample * 255);
                }
            }
            nd.NoiseMap = BitmapSource.Create(resolution, resolution, dpi, dpi,
                PixelFormats.Indexed8, BitmapPalettes.Gray256, pixelData, resolution);
            NoiseData = nd;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
