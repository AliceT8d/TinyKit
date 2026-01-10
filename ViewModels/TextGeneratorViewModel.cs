using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TinyKit.Entities;
using Windows.ApplicationModel.DataTransfer;

namespace TinyKit.ViewModels;

partial class TextGeneratorViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string? CommittedStr { get; set; } = "";
    [ObservableProperty]
    public partial int DateIndex { get; set; }
    [ObservableProperty]
    public partial int SeparatorIndex { get; set; }
    [ObservableProperty]
    public partial int TimeIndex { get; set; }

    private int _timeIndex = 0;

    [ObservableProperty]
    public partial bool Ddd { get; set; } = true;
    [ObservableProperty]
    public partial bool Tt { get; set; } = false;
    [ObservableProperty]
    public partial bool ShortDate { get; set; } = true;


    [ObservableProperty]
    public partial InfoBar_ InfoBarInstance { get; set; } = new();


    public ObservableCollection<string> Date { get; } =
    [
        "yyyy M d",
        "M d yyyy"
    ];

    public ObservableCollection<string> Separator { get; } =
    [
        "(space)",
        "/",
        ".",
        "-"
    ];

    public ObservableCollection<string> Time =
    [
        "H:m",
        "H:m:s"
    ];

    [RelayCommand]
    public void HasTt()
    {
        Time.Add("h:m");
        Time.Add("h:m:s");
    }

    [RelayCommand]
    public void HasNoTt()
    {
        Time.Remove("h:m");
        Time.Remove("h:m:s");
        if (_timeIndex > 1)
        {
            TimeIndex = _timeIndex - 2;
        }
    }

    [RelayCommand]
    public void SelectTimeIndex()
    {
        if (TimeIndex < 0) return;
        _timeIndex = TimeIndex;
    }


    [RelayCommand]
    public void GenerateCommittedStr()
    {
        string dateFormat = Date[DateIndex];
        string separator = SeparatorIndex == 0 ? " " : Separator[SeparatorIndex];
        dateFormat = dateFormat.Replace(" ", separator);
        if (Ddd) dateFormat += " ddd";

        string timeFormat = Time[TimeIndex];
        if (Tt) timeFormat += " tt";

        if (!ShortDate)
        {
            dateFormat = dateFormat.Replace("M", "MM");
            dateFormat = dateFormat.Replace("d", "dd");

            timeFormat = timeFormat.Replace("H", "HH");
            timeFormat = timeFormat.Replace("h", "hh");
            timeFormat = timeFormat.Replace("m", "mm");
            timeFormat = timeFormat.Replace("s", "ss");
        }

        DateTime now = DateTime.Now;
        string formattedDate = now.ToString(dateFormat, CultureInfo.CreateSpecificCulture("zh-CN"));
        string formattedTime = now.ToString(timeFormat, CultureInfo.CreateSpecificCulture("zh-CN"));

        CommittedStr = "已于 " + $"{formattedDate} {formattedTime}" + " 提交";
        Debug.WriteLine(CommittedStr);

        InfoBarInstance.Title = "成功";
        InfoBarInstance.Message = $"已生成：{CommittedStr}";
        InfoBarInstance.Severity = InfoBarSeverity.Success;
        InfoBarInstance.IsOpen = true;
        InfoBarInstance.InfoBarVisibility = Visibility.Visible;
    }

    [RelayCommand]
    public void CopyCommittedStr()
    {
        var package = new DataPackage();
        package.SetText(CommittedStr);
        Clipboard.SetContent(package);

        InfoBarInstance.Title = "成功";
        InfoBarInstance.Message = $"已复制：{CommittedStr}";
        InfoBarInstance.Severity = InfoBarSeverity.Success;
        InfoBarInstance.IsOpen = true;
        InfoBarInstance.InfoBarVisibility = Visibility.Visible;
    }

    [RelayCommand]
    public void CloseInfoBarInstance()
    {
        InfoBarInstance.IsOpen = false;
        InfoBarInstance.InfoBarVisibility = Visibility.Collapsed;
    }
}
