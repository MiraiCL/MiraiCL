using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MiraiCL.App.ViewModels.Controls.Card;

public partial class CardViewModel : ObservableObject
{
    // Default values chosen sensibly for a card control. Adjust as needed.

    [ObservableProperty]
    private CornerRadius _cornerRadius = new(8);

    [ObservableProperty]
    private Thickness _cardMargin = new(8);

    [ObservableProperty]
    private Thickness _cardPadding = new(12);

    [ObservableProperty]
    private IBrush _cardBackground = Brushes.White;

    // Use object to allow both simple content (string) and visual content (IControl).
    [ObservableProperty]
    private UserControl[]? _children;
}