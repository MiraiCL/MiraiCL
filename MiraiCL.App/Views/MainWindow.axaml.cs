using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MiraiCL.App.ViewModels;
using MiraiCL.App.Models.Media;
namespace MiraiCL.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine("111");
    
        Task.Run(async() =>{
        
        using var audio = new AudioPlayer();
        audio.LoadAudioFile("zhenzhu.m4a");
        audio.Play();
        while (audio.IsPlaying){
            await Task.Delay(1000);
        }
        });
    }

    public void ClickButton(object sender,RoutedEventArgs e) => ((MainWindowViewModel)DataContext).Data = $"泥嚎！{((MainWindowViewModel)DataContext).UserText}";
}