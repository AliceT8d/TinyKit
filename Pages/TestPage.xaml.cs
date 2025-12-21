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


namespace TinyKit.Pages;

public sealed partial class TestPage : Page
{
    public TestPage()
    {
        InitializeComponent();
    }

    private void Button_ClickToChangeText(object sender, RoutedEventArgs e)
    {
        Button? button = sender as Button;
        if(button is not null)
            button.Content = "clicked!";
    }
}
