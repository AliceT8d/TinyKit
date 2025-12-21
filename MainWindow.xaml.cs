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
        WinUIEx.WindowManager.Get(this).Width = 1230;
        WinUIEx.WindowManager.Get(this).Height = 950;

        MainWindow_Loaded();
    }

    //C# code behind
    private void NavigationViewInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions navOptions = new FrameNavigationOptions();

        navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
        if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
        {
            navOptions.IsNavigationStackEnabled = false;
        }

        Type pageType = typeof(TestPage);
        if (args.InvokedItem is NavigationViewItem item && item == NavigationViewItem_TestPage)
        {
            pageType = typeof(TestPage);
        }

        if (args.InvokedItem is NavigationViewItem item4 && item4 == NavigationViewItem_TestPage)
        {
            pageType = typeof(TestPage);
        }

        NavigationViewFrame_ContentFrame.NavigateToType(pageType, null, navOptions);
    }

    private void MainWindow_Loaded()
    {
        if (NavigationViewFrame_ContentFrame.Content == null)
        {
            MainWindowNavigationView.SelectedItem = NavigationViewItem_TestPage;
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
            // 初始化页面
            NavigationViewFrame_ContentFrame.NavigateToType(typeof(TestPage), null, navOptions);
        }
    }
}
