using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using TinyKit.Entities;
using Windows.ApplicationModel.DataTransfer;

namespace TinyKit.ViewModels;

partial class TextGeneratorViewModel_TimeText : ObservableObject
{
    [ObservableProperty]
    public partial string CommittedStr { get; set; } = "";
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
    public partial bool UseCurrTime { get; set; } = true;

    [ObservableProperty]
    public partial string? Timestamp { get; set; }

    [ObservableProperty]
    public partial string? FormattedStr { get; set; }


    [ObservableProperty]
    public partial int LangIndex { get; set; } = 0;
    public ObservableCollection<string> Lang { get; } =
    [
        "zh-CN",
        "en-US",
        "ja-JP",
        "ko-KR",
        "fr-FR",
        "ru-RU"
    ];

    public bool isValidTimestamp { get; set; } = false;


    [ObservableProperty]
    public partial MyInfoBar InfoBarInstance { get; set; } = new();


    public ObservableCollection<string> Date { get; } =
    [
        "Year Month Day",
        "Month Day Year"
    ];

    public ObservableCollection<string> Separator { get; } =
    [
        "/",
        "(space)",
        ".",
        "-"
    ];

    public ObservableCollection<string> Time =
    [
        "H:m",
        "H:m:s"
    ];

    [ObservableProperty]
    public partial int UtcDataSourceIndex { get; set; } = 7;

    public ObservableCollection<string> UtcDataSource =
    [
        "UTC+14:00",
        "UTC+13:00",
        "UTC+12:00",
        "UTC+11:00",
        "UTC+10:00",
        "UTC+09:30",
        "UTC+09:00",
        "UTC+08:00",
        "UTC+07:00",
        "UTC+06:30",
        "UTC+06:00",
        "UTC+05:45",
        "UTC+05:30",
        "UTC+05:00",
        "UTC+04:30",
        "UTC+04:00",
        "UTC+03:30",
        "UTC+03:00",
        "UTC+02:00",
        "UTC+01:00",
        "UTC±00:00 (UTC/Zulu)",
        "UTC-01:00",
        "UTC-02:00",
        "UTC-03:00",
        "UTC-03:30",
        "UTC-04:00",
        "UTC-05:00",
        "UTC-06:00",
        "UTC-07:00",
        "UTC-08:00",
        "UTC-09:00",
        "UTC-09:30",
        "UTC-10:00",
        "UTC-11:00",
        "UTC-12:00"
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
        string dateFormat = (DateIndex == 0 ? "yyyy M d" : "M d yyyy");
        string separator = SeparatorIndex == 1 ? " " : Separator[SeparatorIndex];
        separator = $"'{separator}'";
        dateFormat = dateFormat.Replace(" ", separator);
        string timeFormat = Time[TimeIndex];

        if (!ShortDate)
        {
            dateFormat = dateFormat.Replace("M", "MM");
            dateFormat = dateFormat.Replace("d", "dd");

            timeFormat = timeFormat.Replace("H", "HH");
            timeFormat = timeFormat.Replace("h", "hh");
        }
        if (Ddd)
        {
            if (!ShortDate) dateFormat += " dddd";
            else dateFormat += " ddd";
        }
        if (Tt) timeFormat += " tt";
        timeFormat = timeFormat.Replace("m", "mm");
        timeFormat = timeFormat.Replace("s", "ss");

        Debug.WriteLine($"dateFormat: {dateFormat}, timeFormat: {timeFormat}");

        string timestampToUse;
        if (UseCurrTime || string.IsNullOrEmpty(Timestamp))
        {
            long generatedMs = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            timestampToUse = generatedMs.ToString();
            isValidTimestamp = true;
        }
        else
        {
            timestampToUse = Timestamp!;
        }

        // 校验并解析时间戳
        if (!isValidTimestamp || string.IsNullOrWhiteSpace(timestampToUse) || !long.TryParse(timestampToUse, out long tsValue))
        {
            DisplayInfoBarInstance(false, "Invalid timestamp.");
            return;
        }

        DateTimeOffset utcDateTime;
        if (timestampToUse.Length == 9 || timestampToUse.Length == 10)
        {
            utcDateTime = DateTimeOffset.FromUnixTimeSeconds(tsValue);
        }
        else
        {
            DisplayInfoBarInstance(false, "Invalid timestamp.");
            return;
        }


        string utcString = UtcDataSource[UtcDataSourceIndex];
        TimeSpan offset = TimeSpan.Zero;
        var m = Regex.Match(utcString, @"UTC([+\-±])(\d{1,2}):(\d{2})");
        if (m.Success)
        {
            char signChar = m.Groups[1].Value[0];
            int hours = int.Parse(m.Groups[2].Value);
            int minutes = int.Parse(m.Groups[3].Value);
            int sign = signChar == '-' ? -1 : 1;
            offset = new TimeSpan(sign * hours, sign * minutes, 0);
        }

        DateTimeOffset target = utcDateTime.ToOffset(offset);

        string formattedDate = target.ToString(dateFormat, CultureInfo.CreateSpecificCulture(Lang[LangIndex]));
        string formattedTime = target.ToString(timeFormat, CultureInfo.CreateSpecificCulture(Lang[LangIndex]));


        if (!string.IsNullOrWhiteSpace(FormattedStr) && (FormattedStr.Contains("%s") || FormattedStr.Contains("%S")))
        {
            CommittedStr = FormattedStr.Replace("%S", "%s");
            CommittedStr = CommittedStr.Replace("%s", $"{formattedDate} {formattedTime}");
        }
        else
        {
            CommittedStr = $"{formattedDate} {formattedTime}";
        }


        DisplayInfoBarInstance(true, $"Generated: {CommittedStr}");
    }

    [RelayCommand]
    public void CopyCommittedStr()
    {
        var package = new DataPackage();
        package.SetText(CommittedStr);
        Clipboard.SetContent(package);

        DisplayInfoBarInstance(true, $"Copied: {CommittedStr}");
    }

    [RelayCommand]
    public void CloseInfoBarInstance()
    {
        InfoBarInstance.IsOpen = false;
        InfoBarInstance.InfoBarVisibility = Visibility.Collapsed;
    }

    partial void OnTimestampChanged(string? value)
    {
        isValidTimestamp = false;
        CloseInfoBarInstance();
        // 本身是空直接返回
        if (string.IsNullOrEmpty(Timestamp) || string.IsNullOrWhiteSpace(Timestamp))
            return;

        string onlyDigits = Regex.Replace(Timestamp, @"[^0-9]", string.Empty);
        string result = Regex.Replace(onlyDigits, @"^0+", string.Empty);
        Timestamp = result;
        // 处理后还是空，直接返回
        if (string.IsNullOrEmpty(Timestamp) || string.IsNullOrWhiteSpace(Timestamp))
            return;

        bool isValid = true;

        if (Timestamp.Length == 13 || Timestamp.Length == 12)
        {
            Timestamp = Timestamp[..^3];
        }
        isValid = isValid && (Timestamp.Length == 9 || Timestamp.Length == 10);

        if (!isValid)
            DisplayInfoBarInstance(false, "Invalid timestamp.");
        else
            isValidTimestamp = isValid;
    }


    private void DisplayInfoBarInstance(bool isSuccess, string msg)
    {
        // InfoBarInstance.Title = "成功";
        InfoBarInstance.Message = msg;
        InfoBarInstance.Severity = isSuccess ? InfoBarSeverity.Success : InfoBarSeverity.Error;
        InfoBarInstance.IsOpen = true;
        InfoBarInstance.InfoBarVisibility = Visibility.Visible;
    }


    [RelayCommand]
    public void SetCurrentLocalTime()
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        TimeSpan utcOffset = localTimeZone.GetUtcOffset(DateTime.Now);

        string offsetSign = utcOffset >= TimeSpan.Zero ? "+" : "-";
        int absoluteHours = Math.Abs(utcOffset.Hours);
        int minutes = utcOffset.Minutes;

        int temp_index = 0;
        foreach (var x in UtcDataSource)
        {
            if (x.Contains($"{offsetSign}{absoluteHours:D2}:{minutes:D2}"))
            {
                UtcDataSourceIndex = temp_index;
                break;
            }
            temp_index++;
        }
        Timestamp = null;
        return;
    }


    [RelayCommand]
    public void Test()
    {
        Debug.WriteLine($"Timestamp {Timestamp}");
        Debug.WriteLine($"FormattedStr {FormattedStr}");
    }

}
