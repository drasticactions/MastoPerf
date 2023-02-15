// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MastoPerf.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
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
