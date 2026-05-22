using Check.Models;
using Check.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static Check.Views.PositionView;

namespace Check.Views
{
    public partial class SettingsEditingView
    {
        internal SettingsEditingView(PositionView positionView)
        {
            Debug.Assert(positionView != null);

            InitializeComponent();

            PositionView = positionView;

            Border    borderEmpty            = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderWhiteMan         = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderBlackMan         = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderWhiteKing        = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderBlackKing        = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;

            FieldToBackgroundColorConverterFill fieldToBackgroundColorConverterFill = new FieldToBackgroundColorConverterFill();

            Position          positionEditing          = new Position();
            PositionViewModel positionViewModelEditing = new PositionViewModel(positionEditing);
            PositionView      positionViewEditing      = new PositionView(positionViewModelEditing);

            positionEditing._fields[1] = (byte) Position.FieldContentEnum.Empty    ;
            positionEditing._fields[2] = (byte) Position.FieldContentEnum.WhiteMan ;
            positionEditing._fields[3] = (byte) Position.FieldContentEnum.WhiteKing;
            positionEditing._fields[4] = (byte) Position.FieldContentEnum.BlackMan ;
            positionEditing._fields[5] = (byte) Position.FieldContentEnum.BlackKing;

            FieldViewModel       fieldViewModelEmpty     = new    FieldViewModel(positionViewModelEditing, 1) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModel       fieldViewModelWhiteMan  = new    FieldViewModel(positionViewModelEditing, 2) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModel       fieldViewModelBlackMan  = new    FieldViewModel(positionViewModelEditing, 3) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModel       fieldViewModelWhiteKing = new    FieldViewModel(positionViewModelEditing, 4) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModel       fieldViewModelBlackKing = new    FieldViewModel(positionViewModelEditing, 5) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;

            borderEmpty    .SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModelEmpty    , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderWhiteMan .SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModelWhiteMan , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderBlackMan .SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModelBlackMan , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderWhiteKing.SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModelWhiteKing, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderBlackKing.SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModelBlackKing, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );

            FieldView                 fieldViewEmpty     = new FieldView(positionViewEditing, fieldViewModelEmpty    , 1) { Width = 80d, Height = 80d };
            FieldView                 fieldViewWhiteMan  = new FieldView(positionViewEditing, fieldViewModelWhiteMan , 1) { Width = 80d, Height = 80d };
            FieldView                 fieldViewBlackMan  = new FieldView(positionViewEditing, fieldViewModelBlackMan , 1) { Width = 80d, Height = 80d };
            FieldView                 fieldViewWhiteKing = new FieldView(positionViewEditing, fieldViewModelWhiteKing, 1) { Width = 80d, Height = 80d };
            FieldView                 fieldViewBlackKing = new FieldView(positionViewEditing, fieldViewModelBlackKing, 1) { Width = 80d, Height = 80d };

            Grid gridEditing = new Grid { Margin = new Thickness(16d, 16d, 16d, 0d ) } ;
            
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength( 30d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );

            gridEditing.ColumnDefinitions.Add(new ColumnDefinition { Width   = new GridLength(200d) } );
            gridEditing.ColumnDefinitions.Add(new ColumnDefinition { Width   = new GridLength(200d) } );

            Button buttonClearPosition = new Button { Content = "Clear position" } ; Grid.SetRow(buttonClearPosition, 0); Grid.SetColumn(buttonClearPosition, 0); Grid.SetColumnSpan(buttonClearPosition, 2);

            Grid.SetRow(borderEmpty    , 1); Grid.SetColumn(borderEmpty    , 0);     Grid.SetRow(fieldViewEmpty     , 1); Grid.SetColumn(fieldViewEmpty     , 0); Grid.SetColumnSpan(borderEmpty        , 2); Grid.SetColumnSpan(fieldViewEmpty, 2);
            Grid.SetRow(borderWhiteMan , 2); Grid.SetColumn(borderWhiteMan , 0);     Grid.SetRow(fieldViewWhiteMan  , 2); Grid.SetColumn(fieldViewWhiteMan  , 0);
            Grid.SetRow(borderBlackMan , 2); Grid.SetColumn(borderBlackMan , 1);     Grid.SetRow(fieldViewBlackMan  , 2); Grid.SetColumn(fieldViewBlackMan  , 1);
            Grid.SetRow(borderWhiteKing, 3); Grid.SetColumn(borderWhiteKing, 0);     Grid.SetRow(fieldViewWhiteKing , 3); Grid.SetColumn(fieldViewWhiteKing , 0);
            Grid.SetRow(borderBlackKing, 3); Grid.SetColumn(borderBlackKing, 1);     Grid.SetRow(fieldViewBlackKing , 3); Grid.SetColumn(fieldViewBlackKing , 1);

            gridEditing.Children.Add(buttonClearPosition);
            gridEditing.Children.Add(borderEmpty        ); gridEditing.Children.Add(fieldViewEmpty    );
            gridEditing.Children.Add(borderWhiteMan     ); gridEditing.Children.Add(fieldViewWhiteMan );
            gridEditing.Children.Add(borderBlackMan     ); gridEditing.Children.Add(fieldViewBlackMan );
            gridEditing.Children.Add(borderWhiteKing    ); gridEditing.Children.Add(fieldViewWhiteKing);
            gridEditing.Children.Add(borderBlackKing    ); gridEditing.Children.Add(fieldViewBlackKing);

            Content = gridEditing;
        }

        private PositionView PositionView { get; }
    }
}
