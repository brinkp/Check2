using System;
using Check.Models;
using Check.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static Check.Views.PositionView;

namespace Check.Views
{
    public partial class SettingsEditingView
    {
        #region Constructors

        internal SettingsEditingView(PositionView positionView)
        {
            Debug.Assert(positionView != null);

            InitializeComponent();

            PositionView                     = positionView;
            PositionView.SettingsEditingView = this;

            Border borderEmpty     = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border borderWhiteMan  = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border borderBlackMan  = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border borderWhiteKing = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border borderBlackKing = new Border   { Background = Brushes.LightSteelBlue, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;

            FieldToBackgroundColorConverterFill fieldToBackgroundColorConverterFill = new FieldToBackgroundColorConverterFill();

            Position          positionEditing          = new Position         ();
            PositionViewModel positionViewModelEditing = new PositionViewModel(positionEditing);
            PositionView      positionViewEditing      = new PositionView     (positionViewModelEditing) { SettingsEditingView = this, OperationStatus = OperationStatusEnum.Selecting } ;

            positionEditing._fields[1]   = (byte) Position.FieldContentEnum.Empty    ;
            positionEditing._fields[2]   = (byte) Position.FieldContentEnum.WhiteMan ;
            positionEditing._fields[3]   = (byte) Position.FieldContentEnum.BlackMan ;
            positionEditing._fields[4]   = (byte) Position.FieldContentEnum.WhiteKing;
            positionEditing._fields[5]   = (byte) Position.FieldContentEnum.BlackKing;

            FieldViewModelEmpty          = new FieldViewModel(positionViewModelEditing, 1) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModelWhiteMan       = new FieldViewModel(positionViewModelEditing, 2) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModelBlackMan       = new FieldViewModel(positionViewModelEditing, 3) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModelWhiteKing      = new FieldViewModel(positionViewModelEditing, 4) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;
            FieldViewModelBlackKing      = new FieldViewModel(positionViewModelEditing, 5) { FieldStatus = FieldViewModel.FieldStatusEnum.Editing } ;

            borderEmpty    .SetBinding(Border.BackgroundProperty, new Binding { Source = FieldViewModelEmpty    , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderWhiteMan .SetBinding(Border.BackgroundProperty, new Binding { Source = FieldViewModelWhiteMan , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderBlackMan .SetBinding(Border.BackgroundProperty, new Binding { Source = FieldViewModelBlackMan , Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderWhiteKing.SetBinding(Border.BackgroundProperty, new Binding { Source = FieldViewModelWhiteKing, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );
            borderBlackKing.SetBinding(Border.BackgroundProperty, new Binding { Source = FieldViewModelBlackKing, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = positionViewEditing } );

            FieldView fieldViewEmpty     = new FieldView(positionViewEditing, FieldViewModelEmpty    , 1) { Width = 80d, Height = 80d } ;
            FieldView fieldViewWhiteMan  = new FieldView(positionViewEditing, FieldViewModelWhiteMan , 1) { Width = 80d, Height = 80d } ;
            FieldView fieldViewBlackMan  = new FieldView(positionViewEditing, FieldViewModelBlackMan , 1) { Width = 80d, Height = 80d } ;
            FieldView fieldViewWhiteKing = new FieldView(positionViewEditing, FieldViewModelWhiteKing, 1) { Width = 80d, Height = 80d } ;
            FieldView fieldViewBlackKing = new FieldView(positionViewEditing, FieldViewModelBlackKing, 1) { Width = 80d, Height = 80d } ;

            Grid gridEditing = new Grid { Margin = new Thickness(16d, 16d, 16d, 0d ) } ;
            
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength( 30d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );
            gridEditing.   RowDefinitions.Add(new    RowDefinition { Height  = new GridLength(100d) } );

            gridEditing.ColumnDefinitions.Add(new ColumnDefinition { Width   = new GridLength(200d) } );
            gridEditing.ColumnDefinitions.Add(new ColumnDefinition { Width   = new GridLength(200d) } );

            Button buttonClearPosition = new Button { Content = "Clear position" } ; Grid.SetRow(buttonClearPosition, 0); Grid.SetColumn(buttonClearPosition, 0); Grid.SetColumnSpan(buttonClearPosition, 2); buttonClearPosition.Click += OnClearPosition;

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

            Content      = gridEditing;

            FieldContent = Position.FieldContentEnum.Empty;

            void OnClearPosition(object sender, RoutedEventArgs e)
            {
                positionView.ClearBoard     ();
                positionView.ShowEditingMode();

                positionView.PushUndoStack(new );
            }
        }

        #endregion

        #region Event handlers

        private void FieldViewEmptyOnMouseDown(object sender, MouseButtonEventArgs ea)
        {
            if (sender is FieldView fieldView)
            {
                FieldViewModelEmpty     .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                FieldViewModelWhiteMan  .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                FieldViewModelBlackMan  .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                FieldViewModelWhiteKing .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                FieldViewModelBlackKing .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;

                fieldView.FieldViewModel.FieldStatus = FieldViewModel.FieldStatusEnum.FromGiven;
            }

            ea.Handled = true;
        }

        #endregion

        #region Public properties

        private  Position.FieldContentEnum _fieldContent = Position.FieldContentEnum.Taken; // Force initial update
        internal Position.FieldContentEnum  FieldContent
        {
            get => _fieldContent;
            set
            {
                if (_fieldContent != value)
                {
                    _fieldContent  = value;

                     UpdateFieldContents();
                }
            }
        }

        #endregion

        #region Prive properties

        private  PositionView   PositionView            { get; }

        private  FieldViewModel FieldViewModelEmpty     { get; }
        private  FieldViewModel FieldViewModelWhiteMan  { get; }
        private  FieldViewModel FieldViewModelBlackMan  { get; }
        private  FieldViewModel FieldViewModelWhiteKing { get; }
        private  FieldViewModel FieldViewModelBlackKing { get; }

        #endregion

        #region Private methods

        private void UpdateFieldContents()
        {
                                                          FieldViewModelEmpty    .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                                                          FieldViewModelWhiteMan .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                                                          FieldViewModelBlackMan .FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                                                          FieldViewModelWhiteKing.FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
                                                          FieldViewModelBlackKing.FieldStatus = FieldViewModel.FieldStatusEnum.Editing;
            switch (FieldContent)
            {
                case Position.FieldContentEnum.Empty    : FieldViewModelEmpty    .FieldStatus = FieldViewModel.FieldStatusEnum.EditingSelected; break;
                case Position.FieldContentEnum.WhiteMan : FieldViewModelWhiteMan .FieldStatus = FieldViewModel.FieldStatusEnum.EditingSelected; break;
                case Position.FieldContentEnum.BlackMan : FieldViewModelBlackMan .FieldStatus = FieldViewModel.FieldStatusEnum.EditingSelected; break;
                case Position.FieldContentEnum.WhiteKing: FieldViewModelWhiteKing.FieldStatus = FieldViewModel.FieldStatusEnum.EditingSelected; break;
                case Position.FieldContentEnum.BlackKing: FieldViewModelBlackKing.FieldStatus = FieldViewModel.FieldStatusEnum.EditingSelected; break;
                case Position.FieldContentEnum.Taken    :
                default:
                    throw new ArgumentOutOfRangeException(nameof(FieldContent), "Invalid switch value");
            }
                                                          FieldViewModelEmpty    .Refresh();
                                                          FieldViewModelWhiteMan .Refresh();
                                                          FieldViewModelBlackMan .Refresh();
                                                          FieldViewModelWhiteKing.Refresh();
                                                          FieldViewModelBlackKing.Refresh();
        }

        #endregion
    }
}
