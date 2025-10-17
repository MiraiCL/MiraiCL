using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace MiraiCL.App.Views.Controls.Card;

public partial class LocalCard : UserControl
{
/*
    private Point _dragStart;
    private bool _isResizing;
    private ResizeDirection _resizeDirection;
    
    public static readonly StyledProperty<double> CardWidthProperty =
        AvaloniaProperty.Register<LocalCard, double>(nameof(CardWidth), 400);

    public static readonly StyledProperty<double> CardHeightProperty =
        AvaloniaProperty.Register<LocalCard, double>(nameof(CardHeight), 300);

    public double CardWidth
    {
        get => GetValue(CardWidthProperty);
        set => SetValue(CardWidthProperty, value);
    }

    public double CardHeight
    {
        get => GetValue(CardHeightProperty);
        set => SetValue(CardHeightProperty, value);
    }

    public LocalCard()
    {
        InitializeComponent();
        
        Width = CardWidth;
        Height = CardHeight;
        
        RegisterResizeHandlers();
        
        this.GetPropertyChangedObservable(CardWidthProperty).Subscribe(_ => UpdateSize());
        this.GetPropertyChangedObservable(CardHeightProperty).Subscribe(_ => UpdateSize());
    }

    public void AddChild(Control child)
    {
        ContentArea.Content = child;
    }

    public void ClearChildren()
    {
        ContentArea.Content = null;
    }

    public Control GetChild()
    {
        return ContentArea.Content as Control;
    }

    private void UpdateSize()
    {
        Width = CardWidth;
        Height = CardHeight;
    }

    private void RegisterResizeHandlers()
    {
        // 为所有调整大小手柄注册事件
        var resizeHandles = new[] { ResizeHandleRight, ResizeHandleBottom, ResizeHandleBottomRight };
        
        foreach (var handle in resizeHandles)
        {
            handle.PointerPressed += OnResizeHandlePressed;
            handle.PointerMoved += OnResizeHandleMoved;
            handle.PointerReleased += OnResizeHandleReleased;
        }
    }

    private void OnResizeHandlePressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is Rectangle handle)
        {
            _isResizing = true;
            _dragStart = e.GetPosition(this.GetVisualRoot());
            _resizeDirection = GetResizeDirection(handle.Name);
            ((Control)sender).Classes.Add("dragging");
        }
    }

    private void OnResizeHandleMoved(object sender, PointerEventArgs e)
    {
        if (!_isResizing) return;

        var currentPosition = e.GetPosition(this.GetVisualRoot());
        var delta = currentPosition - _dragStart;

        switch (_resizeDirection)
        {
            case ResizeDirection.Right:
                CardWidth = Math.Max(MinWidth, Width + delta.X);
                break;
            case ResizeDirection.Bottom:
                CardHeight = Math.Max(MinHeight, Height + delta.Y);
                break;
            case ResizeDirection.BottomRight:
                CardWidth = Math.Max(MinWidth, Width + delta.X);
                CardHeight = Math.Max(MinHeight, Height + delta.Y);
                break;
        }

        _dragStart = currentPosition;
    }

    private void OnResizeHandleReleased(object sender, PointerReleasedEventArgs e)
    {
        _isResizing = false;
        if (sender is Rectangle handle)
        {
            ((Control)sender).Classes.Remove("dragging");
        }
    }

    private ResizeDirection GetResizeDirection(string handleName)
    {
        return handleName switch
        {
            "ResizeHandleRight" => ResizeDirection.Right,
            "ResizeHandleBottom" => ResizeDirection.Bottom,
            "ResizeHandleBottomRight" => ResizeDirection.BottomRight,
            _ => ResizeDirection.None
        };
    }

    private enum ResizeDirection
    {
        None,
        Right,
        Bottom,
        BottomRight
    }
    */
}