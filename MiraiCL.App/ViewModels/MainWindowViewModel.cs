using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MiraiCL.App.Views.Controls.Card;
using MiraiCL.App.Views.Pages.MainPage;

namespace MiraiCL.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    [ObservableProperty]
    public string userText = "怎么还是这张图捏，能不能换一张呀？";
    [ObservableProperty]
    public string? data;
    [ObservableProperty]
    private UserControl? _models = new LocalCard();
}
