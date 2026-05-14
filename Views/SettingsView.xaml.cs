using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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

            Grid grid = new Grid { Margin = new Thickness(0d, 0d, 24d, 0d ) } ;

            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );
            grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength( 40D) } );

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(200d) } );

            TextBlock textBlock = new TextBlock {                           Text = "Settings", FontSize = 24d, FontWeight = FontWeights.Bold,                              Margin = new Thickness(4d)                                                        } ;                                            Grid.SetRow(textBlock, 0); Grid.SetColumn(textBlock, 0); grid.Children.Add(textBlock);
            Button    button    = new Button    { Content = new TextBlock { Text = "Undo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Undo last move <Ctrl+Z>"                     } ; button  .Click += OnUndoMove             ; Grid.SetRow(button   , 1); Grid.SetColumn(button   , 0); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Redo move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Redo last move <Ctrl+Y>"                     } ; button  .Click += OnRedoMove             ; Grid.SetRow(button   , 1); Grid.SetColumn(button   , 1); grid.Children.Add(button   );
            CheckBox  checkBox  = new CheckBox  { Content = new TextBlock { Text = "Automatic moves"       , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Automatically perform forced moves <Ctrl+A>" } ; checkBox.Click += OnAutomaticMoves       ; Grid.SetRow(checkBox , 2); Grid.SetColumn(checkBox , 0); grid.Children.Add(checkBox );
                      checkBox  = new CheckBox  { Content = new TextBlock { Text = "Feedback for moves"    , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Feedback for possible moves <Ctrl+F>"        } ; checkBox.Click += OnFeedbackForMoves     ; Grid.SetRow(checkBox , 2); Grid.SetColumn(checkBox , 1); grid.Children.Add(checkBox );
                      checkBox  = new CheckBox  { Content = new TextBlock { Text = "Intermediate positions", VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Show intermediate positions <Ctrl+I>"        } ; checkBox.Click += OnIntermediatePositions; Grid.SetRow(checkBox , 3); Grid.SetColumn(checkBox , 0); grid.Children.Add(checkBox );
                      button    = new Button    { Content = new TextBlock { Text = "Load stored position"  , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Load stored position <Ctrl+L>"               } ; button  .Click += OnLoadPosition         ; Grid.SetRow(button   , 4); Grid.SetColumn(button   , 0); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Save current position" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Save current position <Ctrl+S>"              } ; button  .Click += OnSavePosition         ; Grid.SetRow(button   , 4); Grid.SetColumn(button   , 1); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Do a move"             , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Do a move <Ctrl+M>"                          } ; button  .Click += OnMove                 ; Grid.SetRow(button   , 5); Grid.SetColumn(button   , 0); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Play until end"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Play until end <Ctrl+P>"                     } ; button  .Click += OnPlay                 ; Grid.SetRow(button   , 5); Grid.SetColumn(button   , 1); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Flip the board"        , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip the board"                              } ; button  .Click += OnFlipBoard            ; Grid.SetRow(button   , 6); Grid.SetColumn(button   , 0); grid.Children.Add(button   );
                      button    = new Button    { Content = new TextBlock { Text = "Flip whose turn it is" , VerticalAlignment = VerticalAlignment.Center }, Height = 28d, Margin = new Thickness(4d), ToolTip="Flip whose turn it is"                       } ; button  .Click += OnFlipTurn             ; Grid.SetRow(button   , 6); Grid.SetColumn(button   , 1); grid.Children.Add(button   );

            Content = grid;

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
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private PositionView PositionView { get; }
    }
}
