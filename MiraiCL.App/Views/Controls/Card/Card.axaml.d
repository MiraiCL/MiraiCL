<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d" d:DesignWidth="400" d:DesignHeight="300"
             x:Name="Card">
    
    <UserControl.Styles>
        <Style Selector="LocalCard">
            <Setter Property="Width" Value="{Binding Width, RelativeSource={RelativeSource Self}}"/>
            <Setter Property="Height" Value="{Binding Height, RelativeSource={RelativeSource Self}}"/>
        </Style>
    </UserControl.Styles>

    <!-- 卡片容器 -->
    <Border x:Name="CardBorder"
            CornerRadius="8"
            Background="#CC2C3E50"  
            BorderBrush="White"
            BorderThickness="1"
            Padding="10">
        <Grid x:Name="MainGrid">
            <Grid.RowDefinitions>
                <RowDefinition Height="*"/>
                <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            
            <!-- 内容区域，用于动态添加子控件 -->
            <ContentControl x:Name="ContentArea"
                          Grid.Row="0"
                          HorizontalContentAlignment="Stretch"
                          VerticalContentAlignment="Stretch"/>
            
            <!-- 调整大小控制区域 -->
            <Canvas Grid.Row="1"
                    Height="10"
                    Margin="0,5,0,0">
                <Rectangle x:Name="ResizeHandleRight"
                          Width="5"
                          Height="10"
                          Canvas.Right="0"
                          Fill="Transparent"
                          Cursor="SizeWE"/>
                
                <Rectangle x:Name="ResizeHandleBottom"
                          Height="5"
                          Width="10"
                          Canvas.Bottom="0"
                          Fill="Transparent"
                          Cursor="SizeNS"/>
                
                <Rectangle x:Name="ResizeHandleBottomRight"
                          Width="5"
                          Height="5"
                          Canvas.Right="0"
                          Canvas.Bottom="0"
                          Fill="Transparent"
                          Cursor="SizeNWSE"/>
            </Canvas>
        </Grid>
    </Border>
</UserControl>