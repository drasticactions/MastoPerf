using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MastoPerf.AvaloniaCrossApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, RoutedEventArgs e)
    {
        ((TestViewModel)this.DataContext).StartStreaming();
    }

    private void Button_Clicked_1(object sender, RoutedEventArgs e)
    {
        ((TestViewModel)this.DataContext).EndStreaming();
    }
}