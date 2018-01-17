using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NoiseMapGenerator.Models;
using NoiseMapGenerator.ViewModels;

namespace NoiseMapGenerator.Views
{
    /// <summary>
    /// Interaction logic for NoiseDataView.xaml
    /// </summary>
    public partial class NoiseDataView : UserControl
    {
        private NoiseViewModel _vm;

        public static readonly DependencyProperty NoiseSourceProperty = DependencyProperty.Register(
            "NoiseSource", typeof(NoiseData), typeof(NoiseDataView), new FrameworkPropertyMetadata(OnSourceChanged));

        public NoiseData NoiseSource
        {
            get { return (NoiseData) GetValue(NoiseSourceProperty); }
            set { SetValue(NoiseSourceProperty, value); }
        }

        public static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NoiseDataView)._vm.NoiseData = e.NewValue as NoiseData;
        }

        public NoiseDataView()
        {
            InitializeComponent();
            _vm = Root.DataContext as NoiseViewModel;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _vm?.GenerateMapCommand.Execute(null);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _vm?.GenerateMapCommand.Execute(null);
        }
    }
}
