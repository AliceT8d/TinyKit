using Microsoft.UI.Xaml.Controls;
using TinyKit.ViewModels;


namespace TinyKit.Pages;

public sealed partial class TextGenerator : Page
{
    TextGeneratorViewModel_TimeText timeTextViewModel;
    public TextGenerator()
    {
        InitializeComponent();
        timeTextViewModel = new();
    }

}
