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
using TinyKit.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace TinyKit.Pages;

public sealed partial class TextGenerator : Page
{
    TextGeneratorViewModel viewModel;
    public TextGenerator()
    {
        InitializeComponent();
        viewModel = new();
    }

}
