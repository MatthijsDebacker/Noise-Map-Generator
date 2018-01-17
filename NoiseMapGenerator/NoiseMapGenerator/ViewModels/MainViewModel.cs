using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using NoiseMapGenerator.Annotations;
using NoiseMapGenerator.Helpers;
using NoiseMapGenerator.Models;

namespace NoiseMapGenerator.ViewModels
{
    class MainViewModel:INotifyPropertyChanged
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

        public RelayCommand SavePngCommand { get; set; }
        public RelayCommand SaveBmpCommand { get; set; }
        public RelayCommand SaveJpegCommand { get; set; }
        public RelayCommand SaveXmlCommand { get; set; }

        public MainViewModel()
        {
            SavePngCommand = new RelayCommand(SavePng);
            SaveBmpCommand = new RelayCommand(SaveBmp);
            SaveJpegCommand = new RelayCommand(SaveJpeg);
            SaveXmlCommand = new RelayCommand(SaveXml);
            NoiseData = new NoiseData();
        }

        public void SaveBmp()
        {
            BitmapEncoder encoder = new BmpBitmapEncoder();
            Save(_noiseData.NoiseMap, encoder);
        }

        public void SavePng()
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            Save(_noiseData.NoiseMap, encoder);
        }

        public void SaveJpeg()
        {
            BitmapEncoder encoder = new JpegBitmapEncoder();
            Save(_noiseData.NoiseMap, encoder);
        }

        public void SaveXml()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Xml files (*.xml;)|*.xml;|All files (*.*)|*.*"
            };
            var folder = Environment.SpecialFolder.MyDocuments.ToString();
            saveFileDialog.InitialDirectory = folder;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    XmlNoiseData data = new XmlNoiseData
                    {
                        NoiseData = NoiseData.NoiseValueData,
                        Dimensions = NoiseData.Dimensions,
                        Size = NoiseData.NoiseValueData.Count
                    };
                    SaveXML.Save(data, saveFileDialog.FileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }

        public void Save(BitmapSource source, BitmapEncoder encoder)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.bmp;)|*.png;*.jpeg;*.bmp;|All files (*.*)|*.*"
            };
            var folder = Environment.SpecialFolder.MyDocuments.ToString();
            saveFileDialog.InitialDirectory = folder;

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Frames.Add(BitmapFrame.Create(source));
                    encoder.Save(fileStream);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
