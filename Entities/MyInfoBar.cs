using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace TinyKit.Entities;

public partial class MyInfoBar : ObservableObject
{
    [ObservableProperty]
    public partial Visibility InfoBarVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial string Title { get; set; } = "";

    [ObservableProperty]
    public partial string Message { get; set; } = "Essential app message or take action on.";

    [ObservableProperty]
    public partial bool IsOpen { get; set; } = true;

    [ObservableProperty]
    public partial InfoBarSeverity Severity { get; set; } = InfoBarSeverity.Informational;


}