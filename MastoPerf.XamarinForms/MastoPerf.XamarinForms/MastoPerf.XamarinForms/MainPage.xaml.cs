using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MastoPerf.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this.TestViewModel;
        }

        public TestViewModel TestViewModel { get; set; } = new TestViewModel();

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.TestViewModel.StartStreaming();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            this.TestViewModel.EndStreaming();
        }
    }
}
