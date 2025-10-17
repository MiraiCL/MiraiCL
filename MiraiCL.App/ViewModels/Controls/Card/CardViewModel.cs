using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MiraiCL.App.ViewModels.Controls.Card;

public partial class CardViewModel:ObservableObject{
    ///<summary>
    /// The width of this card. 
    ///</summary>
    [ObservableProperty]
    private double _width;
    ///<summary>
    /// The height of this card. 
    ///</summary>
    [ObservableProperty] 
    private double _height;
    ///<summary>
    /// The child of this card. 
    ///</summary>
    [ObservableProperty]
    private StackPanel _child;

    
}