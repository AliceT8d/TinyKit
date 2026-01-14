using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using TinyKit.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;

namespace TinyKit;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        WinUIEx.WindowManager.Get(this).Width = 1450;
        WinUIEx.WindowManager.Get(this).Height = 950;
        WinUIEx.WindowManager.Get(this).IsResizable = false;
        WinUIEx.WindowManager.Get(this).IsMaximizable = false;
    }

    private void NavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions navOptions = new FrameNavigationOptions();
        navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

        var invokedContainer = args.InvokedItemContainer as NavigationViewItem;
        var tag = invokedContainer?.Tag?.ToString();

        switch (tag)
        {
            case "NavigationViewItemTag_TestPage":
                NavigationViewFrame_ContentFrame.NavigateToType(typeof(TestPage), null, navOptions);
                MainWindowNavigationView.Header = "TestPage";
                Debug.WriteLine("NavigationViewItem_TestPage");
                break;

            case "NavigationViewItemTag_TextGeneratorPage":
                NavigationViewFrame_ContentFrame.NavigateToType(typeof(TextGenerator), null, navOptions);
                MainWindowNavigationView.Header = "TextGenerator";
                Debug.WriteLine("NavigationViewItem_TextGeneratorPage");
                break;

            case "NavigationViewItemTag_SettingsPage":
                NavigationViewFrame_ContentFrame.NavigateToType(typeof(SettingsPage), null, navOptions);
                MainWindowNavigationView.Header = "SettingsPage";
                Debug.WriteLine("NavigationViewItem_SettingsPage");
                break;

            default:
                Debug.WriteLine("Unknown navigation tag: " + tag);
                break;
        }
    }

    private void MainWindowNavigationViewLoaded(object sender, RoutedEventArgs e)
    {
        MainWindowNavigationView.SelectedItem = NavigationViewItem_TextGeneratorPage;
        MainWindowNavigationView.Header = "TextGenerator";
        NavigationViewFrame_ContentFrame.NavigateToType(typeof(TextGenerator), null, new FrameNavigationOptions());
    }
}
