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
    public partial class SettingsView
    {
        public SettingsView(PositionView positionView)
        {
            InitializeComponent();

            Debug.Assert(positionView != null);

            PositionView = positionView;

            FontSize     = 18d;
            
            Grid gridSettings = new Grid { Margin = new Thickness(16d, 16d, 16d, 0d ) } ;
            Grid gridEditing  = new Grid { Margin = new Thickness(16d, 16d, 16d, 0d ) } ;

            // Settings

            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridSettings.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );

            gridSettings.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );
            gridSettings.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );

            Button    buttonUndoMove         = new Button   { Content = new TextBlock { Text = "Undo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Undo last move <Ctrl+Z>"                     } ; buttonUndoMove         .Click += OnUndoMove             ; Grid.SetRow(buttonUndoMove         , 0); Grid.SetColumn(buttonUndoMove         , 0); gridSettings.Children.Add(buttonUndoMove        );
            Button    buttonRedoMove         = new Button   { Content = new TextBlock { Text = "Redo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Redo last move <Ctrl+Y>"                     } ; buttonRedoMove         .Click += OnRedoMove             ; Grid.SetRow(buttonRedoMove         , 0); Grid.SetColumn(buttonRedoMove         , 1); gridSettings.Children.Add(buttonRedoMove        );
            CheckBox  checkBoxAutomaticMoves = new CheckBox { Content = new TextBlock { Text = "Automatic moves"       , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Automatically perform forced moves <Ctrl+A>" } ; checkBoxAutomaticMoves .Click += OnAutomaticMoves       ; Grid.SetRow(checkBoxAutomaticMoves , 1); Grid.SetColumn(checkBoxAutomaticMoves , 0); gridSettings.Children.Add(checkBoxAutomaticMoves);
            CheckBox  checkBoxFeedback       = new CheckBox { Content = new TextBlock { Text = "Feedback for moves"    , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Feedback for possible moves <Ctrl+F>"        } ; checkBoxFeedback       .Click += OnFeedbackForMoves     ; Grid.SetRow(checkBoxFeedback       , 1); Grid.SetColumn(checkBoxFeedback       , 1); gridSettings.Children.Add(checkBoxFeedback      );
            CheckBox  checkBoxIntermediate   = new CheckBox { Content = new TextBlock { Text = "Intermediate positions", VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Show intermediate positions <Ctrl+I>"        } ; checkBoxIntermediate   .Click += OnIntermediatePositions; Grid.SetRow(checkBoxIntermediate   , 2); Grid.SetColumn(checkBoxIntermediate   , 0); gridSettings.Children.Add(checkBoxIntermediate  );
            Button    buttonLoadPosition     = new Button   { Content = new TextBlock { Text = "Load stored position"  , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Load stored position <Ctrl+L>"               } ; buttonLoadPosition     .Click += OnLoadPosition         ; Grid.SetRow(buttonLoadPosition     , 3); Grid.SetColumn(buttonLoadPosition     , 0); gridSettings.Children.Add(buttonLoadPosition    );
            Button    buttonSavePosition     = new Button   { Content = new TextBlock { Text = "Save current position" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Save current position <Ctrl+S>"              } ; buttonSavePosition     .Click += OnSavePosition         ; Grid.SetRow(buttonSavePosition     , 3); Grid.SetColumn(buttonSavePosition     , 1); gridSettings.Children.Add(buttonSavePosition    );
            Button    buttonMove             = new Button   { Content = new TextBlock { Text = "Do a move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Do a move <Ctrl+M>"                          } ; buttonMove             .Click += OnMove                 ; Grid.SetRow(buttonMove             , 4); Grid.SetColumn(buttonMove             , 0); gridSettings.Children.Add(buttonMove            );
            Button    buttonPlay             = new Button   { Content = new TextBlock { Text = "Play until end"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Play until end <Ctrl+P>"                     } ; buttonPlay             .Click += OnPlay                 ; Grid.SetRow(buttonPlay             , 4); Grid.SetColumn(buttonPlay             , 1); gridSettings.Children.Add(buttonPlay            );
            Button    buttonFlipBoard        = new Button   { Content = new TextBlock { Text = "Flip the board"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip the board"                              } ; buttonFlipBoard        .Click += OnFlipBoard            ; Grid.SetRow(buttonFlipBoard        , 5); Grid.SetColumn(buttonFlipBoard        , 0); gridSettings.Children.Add(buttonFlipBoard       );
            Button    buttonFlipTurn         = new Button   { Content = new TextBlock { Text = "Flip whose turn it is" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip whose turn it is"                       } ; buttonFlipTurn         .Click += OnFlipTurn             ; Grid.SetRow(buttonFlipTurn         , 5); Grid.SetColumn(buttonFlipTurn         , 1); gridSettings.Children.Add(buttonFlipTurn        );
            Button    buttonSolve            = new Button   { Content = new TextBlock { Text = "Solve combination"     , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Solve combination"                           } ; buttonSolve            .Click += OnSolveCombination     ; Grid.SetRow(buttonSolve            , 6); Grid.SetColumn(buttonSolve            , 0); gridSettings.Children.Add(buttonSolve           );

            // Editing

            Border    borderEmpty            = new Border   { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderWhiteMan         = new Border   { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderBlackMan         = new Border   { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderWhiteKing        = new Border   { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;
            Border    borderBlackKing        = new Border   { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d), Width = 80d, Height = 80d } ;

            FieldToBackgroundColorConverterFill fieldToBackgroundColorConverterFill = new FieldToBackgroundColorConverterFill();

            Position          positionEditing          = new Position();
            PositionViewModel positionViewModelEditing = new PositionViewModel(positionEditing);
            PositionView      positionViewEditing      = new PositionView(positionViewModelEditing);

            positionEditing._fields[1] = (byte) Position.FieldContentEnum.Empty    ;
            positionEditing._fields[2] = (byte) Position.FieldContentEnum.WhiteMan ;
            positionEditing._fields[3] = (byte) Position.FieldContentEnum.WhiteKing;
            positionEditing._fields[4] = (byte) Position.FieldContentEnum.BlackMan ;
            positionEditing._fields[5] = (byte) Position.FieldContentEnum.BlackKing;

            FieldViewModel       fieldViewModelEmpty     = new    FieldViewModel(positionViewModelEditing, 1);
            FieldViewModel       fieldViewModelWhiteMan  = new    FieldViewModel(positionViewModelEditing, 2);
            FieldViewModel       fieldViewModelBlackMan  = new    FieldViewModel(positionViewModelEditing, 3);
            FieldViewModel       fieldViewModelWhiteKing = new    FieldViewModel(positionViewModelEditing, 4);
            FieldViewModel       fieldViewModelBlackKing = new    FieldViewModel(positionViewModelEditing, 5);

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

            TabControl tabControl      = new TabControl();

            TabItem    tabItemSettings = new TabItem { Header = "Settings", Content = gridSettings } ;
            TabItem    tabItemEditing  = new TabItem { Header = "Editing" , Content = gridEditing  } ;

            tabControl.Items.Add(tabItemSettings);
            tabControl.Items.Add(tabItemEditing );

            Content = tabControl;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            void OnUndoMove             (object sender, RoutedEventArgs e) { PositionView.UndoLastMove                      (); }
            void OnRedoMove             (object sender, RoutedEventArgs e) { PositionView.RedoLastMove                      (); }
            void OnAutomaticMoves       (object sender, RoutedEventArgs e) { PositionView.ToggleAutomaticMoves              (); }
            void OnFeedbackForMoves     (object sender, RoutedEventArgs e) { PositionView.ToggleGiveVisualFeedback          (); }
            void OnIntermediatePositions(object sender, RoutedEventArgs e) { PositionView.ToggleDisplayIntermediatePositions(); }
            void OnLoadPosition         (object sender, RoutedEventArgs e) { PositionView.LoadPosition                      (); }
            void OnSavePosition         (object sender, RoutedEventArgs e) { PositionView.SavePosition                      (); }
            void OnMove                 (object sender, RoutedEventArgs e) { PositionView.MoveRandom                        (); }
            void OnPlay                 (object sender, RoutedEventArgs e) { PositionView.PlayRandom                        (); }
            void OnFlipBoard            (object sender, RoutedEventArgs e) { PositionView.FlipBoard                         (); }
            void OnFlipTurn             (object sender, RoutedEventArgs e) { PositionView.FlipTurn                          (); }
            void OnSolveCombination     (object sender, RoutedEventArgs e) { PositionView.SolveCombination                  (); }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private PositionView PositionView { get; }
    }
}
