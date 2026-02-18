using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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

            Ellipse = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0.5d };

            Ellipse.SetBinding(Shape.  FillProperty, new Binding(nameof(  Fill)) { Source = this } );
            Ellipse.SetBinding(Shape.StrokeProperty, new Binding(nameof(Stroke)) { Source = this } );

            Content = Ellipse;

            Loaded += (sender, args) => { Panel = VisualTreeHelper.GetParent(this) as Panel; Debug.Assert(Panel != null); };
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty   FillProperty = DependencyProperty.Register(nameof(Fill  ), typeof(Brush), typeof(PieceView));
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PieceView));

        #endregion

        #region Dragging

        protected override void OnMouseDown(MouseButtonEventArgs ea)
        {
            base.OnMouseDown(ea);

            if ((ea.LeftButton == MouseButtonState.Pressed) && (DragInProgress == false))
            {
                DragInProgress  = true;
                DragStartPoint  = ea.GetPosition(Panel);
                DragStartZIndex = Panel.GetZIndex(Ellipse);

                Panel.SetZIndex(this, int.MaxValue);

                CaptureMouse();

                ea.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs ea)
        {
            base.OnMouseMove(ea);

            if (DragInProgress)
            {
                Point newPosition = ea.GetPosition(Panel);

                Ellipse.RenderTransform = new TranslateTransform(newPosition.X - DragStartPoint.X, newPosition.Y - DragStartPoint.Y);

                ea.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs ea)
        {
            base.OnMouseUp(ea);

            if (DragInProgress)
            {
                OnMouseUp();

                ea.Handled = true;
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs ea)
        {
            base.OnLostMouseCapture(ea);

            if (DragInProgress)
            {
                OnMouseUp();

                ea.Handled = true;
            }
        }

        private void OnMouseUp()
        {
            Ellipse.RenderTransform = null;

            Panel.SetZIndex(this, DragStartZIndex);

            // Do this...
            DragInProgress  = false;

            // ... before this
            ReleaseMouseCapture();
        }

        #endregion

        #region Event handlers

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

        #region Private properties

        private Panel   Panel           { get; set; }

        private Ellipse Ellipse         { get;      }

        private bool    DragInProgress  { get; set; }
        private Point   DragStartPoint  { get; set; }
        private int     DragStartZIndex { get; set; }

        #endregion
    }
}
