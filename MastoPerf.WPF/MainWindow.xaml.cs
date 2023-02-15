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

namespace MastoPerf.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.TestViewModel = new TestViewModel();
            this.MainGrid.DataContext = this.TestViewModel;
        }

        public TestViewModel TestViewModel { get; set; } = new TestViewModel();

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            this.TestViewModel.StartStreaming();
        }

        private void Button_Clicked_1(object sender, RoutedEventArgs e)
        {
            this.TestViewModel.EndStreaming();
        }
    }
}
