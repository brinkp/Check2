using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Check.Models;
using Check.ViewModels;

namespace Check.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView(PositionView positionView)
        {
            InitializeComponent();

            Debug.Assert(positionView != null);

            PositionView = positionView;

            FontSize     = 18d;

            StackPanel stackPanelMain      = new StackPanel { Orientation = Orientation.Vertical        } ;
            StackPanel stackPanelSelection = new StackPanel { Orientation = Orientation.Horizontal      } ;
            StackPanel stackPanelEditing   = new StackPanel { Orientation = Orientation.Vertical        } ;

            Grid       gridPlaying         = new Grid       { Margin = new Thickness(0d, 24d, 24d, 0d ) } ;

            stackPanelMain.Children.Add(stackPanelSelection);
            stackPanelMain.Children.Add(gridPlaying        );
            stackPanelMain.Children.Add(stackPanelEditing  );

            Content = stackPanelMain;

            RadioButton radioButtonPlaying = new RadioButton { Content = new TextBlock { Text = "Playing", FontSize = 24 } , VerticalContentAlignment = VerticalAlignment.Center , Margin = new Thickness( 4d, 0d, 52d, 0d) } ; radioButtonPlaying.Checked += delegate { gridPlaying.Visibility = Visibility.Visible; stackPanelEditing.Visibility = Visibility.Hidden ; } ;
            RadioButton radioButtonEditing = new RadioButton { Content = new TextBlock { Text = "Editing", FontSize = 24 } , VerticalContentAlignment = VerticalAlignment.Center , Margin = new Thickness(52d, 0d,  0d, 0d) } ; radioButtonEditing.Checked += delegate { gridPlaying.Visibility = Visibility.Hidden ; stackPanelEditing.Visibility = Visibility.Visible; } ;

            radioButtonPlaying.IsChecked   = true;

            stackPanelSelection.Children.Add(radioButtonPlaying);
            stackPanelSelection.Children.Add(radioButtonEditing);

            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            gridPlaying.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );

            gridPlaying.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );
            gridPlaying.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );

            Button    buttonUndoMove         = new Button    { Content = new TextBlock { Text = "Undo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Undo last move <Ctrl+Z>"                     } ; buttonUndoMove         .Click += OnUndoMove             ; Grid.SetRow(buttonUndoMove         , 0); Grid.SetColumn(buttonUndoMove         , 0); gridPlaying.Children.Add(buttonUndoMove        );
            Button    buttonRedoMove         = new Button    { Content = new TextBlock { Text = "Redo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Redo last move <Ctrl+Y>"                     } ; buttonRedoMove         .Click += OnRedoMove             ; Grid.SetRow(buttonRedoMove         , 0); Grid.SetColumn(buttonRedoMove         , 1); gridPlaying.Children.Add(buttonRedoMove        );
            CheckBox  checkBoxAutomaticMoves = new CheckBox  { Content = new TextBlock { Text = "Automatic moves"       , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Automatically perform forced moves <Ctrl+A>" } ; checkBoxAutomaticMoves .Click += OnAutomaticMoves       ; Grid.SetRow(checkBoxAutomaticMoves , 1); Grid.SetColumn(checkBoxAutomaticMoves , 0); gridPlaying.Children.Add(checkBoxAutomaticMoves);
            CheckBox  checkBoxFeedback       = new CheckBox  { Content = new TextBlock { Text = "Feedback for moves"    , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Feedback for possible moves <Ctrl+F>"        } ; checkBoxFeedback       .Click += OnFeedbackForMoves     ; Grid.SetRow(checkBoxFeedback       , 1); Grid.SetColumn(checkBoxFeedback       , 1); gridPlaying.Children.Add(checkBoxFeedback      );
            CheckBox  checkBoxIntermediate   = new CheckBox  { Content = new TextBlock { Text = "Intermediate positions", VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Show intermediate positions <Ctrl+I>"        } ; checkBoxIntermediate   .Click += OnIntermediatePositions; Grid.SetRow(checkBoxIntermediate   , 2); Grid.SetColumn(checkBoxIntermediate   , 0); gridPlaying.Children.Add(checkBoxIntermediate  );
            Button    buttonLoadPosition     = new Button    { Content = new TextBlock { Text = "Load stored position"  , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Load stored position <Ctrl+L>"               } ; buttonLoadPosition     .Click += OnLoadPosition         ; Grid.SetRow(buttonLoadPosition     , 3); Grid.SetColumn(buttonLoadPosition     , 0); gridPlaying.Children.Add(buttonLoadPosition    );
            Button    buttonSavePosition     = new Button    { Content = new TextBlock { Text = "Save current position" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Save current position <Ctrl+S>"              } ; buttonSavePosition     .Click += OnSavePosition         ; Grid.SetRow(buttonSavePosition     , 3); Grid.SetColumn(buttonSavePosition     , 1); gridPlaying.Children.Add(buttonSavePosition    );
            Button    buttonMove             = new Button    { Content = new TextBlock { Text = "Do a move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Do a move <Ctrl+M>"                          } ; buttonMove             .Click += OnMove                 ; Grid.SetRow(buttonMove             , 4); Grid.SetColumn(buttonMove             , 0); gridPlaying.Children.Add(buttonMove            );
            Button    buttonPlay             = new Button    { Content = new TextBlock { Text = "Play until end"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Play until end <Ctrl+P>"                     } ; buttonPlay             .Click += OnPlay                 ; Grid.SetRow(buttonPlay             , 4); Grid.SetColumn(buttonPlay             , 1); gridPlaying.Children.Add(buttonPlay            );
            Button    buttonFlipBoard        = new Button    { Content = new TextBlock { Text = "Flip the board"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip the board"                              } ; buttonFlipBoard        .Click += OnFlipBoard            ; Grid.SetRow(buttonFlipBoard        , 5); Grid.SetColumn(buttonFlipBoard        , 0); gridPlaying.Children.Add(buttonFlipBoard       );
            Button    buttonFlipTurn         = new Button    { Content = new TextBlock { Text = "Flip whose turn it is" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip whose turn it is"                       } ; buttonFlipTurn         .Click += OnFlipTurn             ; Grid.SetRow(buttonFlipTurn         , 5); Grid.SetColumn(buttonFlipTurn         , 1); gridPlaying.Children.Add(buttonFlipTurn        );
            Button    buttonSolve            = new Button    { Content = new TextBlock { Text = "Solve combination"     , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Solve combination"                           } ; buttonSolve            .Click += OnSolveCombination     ; Grid.SetRow(buttonSolve            , 6); Grid.SetColumn(buttonSolve            , 0); gridPlaying.Children.Add(buttonSolve           );

            Position                   positionDummy = new Position         (                         );
            PositionViewModel positionViewModelDummy = new PositionViewModel(positionDummy            );
            PositionView           positionViewDummy = new PositionView(positionViewModelDummy        );
            FieldViewModel       fieldViewModelDummy = new    FieldViewModel(positionViewModelDummy, 1);

            FieldView             fieldViewEmpty     = new FieldView(positionViewDummy, fieldViewModelDummy, 1); stackPanelEditing.Children.Add(fieldViewEmpty    );
            FieldView             fieldViewWhiteMan  = new FieldView(positionViewDummy, fieldViewModelDummy, 1); stackPanelEditing.Children.Add(fieldViewWhiteMan );
            FieldView             fieldViewBlackMan  = new FieldView(positionViewDummy, fieldViewModelDummy, 1); stackPanelEditing.Children.Add(fieldViewBlackMan );
            FieldView             fieldViewWhiteKing = new FieldView(positionViewDummy, fieldViewModelDummy, 1); stackPanelEditing.Children.Add(fieldViewWhiteKing);
            FieldView             fieldViewBlackKing = new FieldView(positionViewDummy, fieldViewModelDummy, 1); stackPanelEditing.Children.Add(fieldViewBlackKing);

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
