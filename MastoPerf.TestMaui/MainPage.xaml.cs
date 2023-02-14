namespace MastoPerf.TestMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = this.TestViewModel;
	}

	public TestViewModel TestViewModel { get; set; } = new TestViewModel();
}

