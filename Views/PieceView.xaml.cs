using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Check.Views
{
    public partial class PieceView
    {
        #region Delegates and events

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // Do nothing
            }
        }

        #endregion

        #region Constructors

        public PieceView()
        {
            InitializeComponent();

            Ellipse ellipse = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0.5d };

            ellipse.SetBinding(Shape.  FillProperty, new Binding(nameof(  Fill)) { Source = this } );
            ellipse.SetBinding(Shape.StrokeProperty, new Binding(nameof(Stroke)) { Source = this } );

            Content = ellipse;

            MouseDown += PieceView_MouseDown;

            DragOver += PieceView_DragOver;
            Drop     += PieceView_Drop    ;
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty   FillProperty = DependencyProperty.Register(nameof(Fill  ), typeof(Brush), typeof(PieceView));
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PieceView));

        #endregion

        #region Event handlers

        private void PieceView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is PieceView pieceView)
            {
                DragDrop.DoDragDrop(pieceView, pieceView, DragDropEffects.Move);
            }
        }

        private void PieceView_DragOver(object sender, DragEventArgs e)
        {
            if (sender.GetType() == typeof(PieceView))
            {
                Cursor = Cursors.Pen;
            }
        }

        private void PieceView_Drop(object sender, DragEventArgs e)
        {

        }

        #endregion

        #region Public properties

        public Brush Fill
        {
            get => (Brush) GetValue(  FillProperty);
            set
            {
                if (Fill != value)
                {
                    SetValue(FillProperty, value);

                    OnPropertyChanged();
                }
            }
        }

        public Brush Stroke
        {
            get => (Brush) GetValue(StrokeProperty);
            set
            {
                if (Stroke != value)
                {
                    SetValue(StrokeProperty, value);

                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
