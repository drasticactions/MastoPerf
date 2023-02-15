using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MastoPerf.AvaloniaApp;

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