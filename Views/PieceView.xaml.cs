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

        public PieceView(PositionView positionView, int fieldIndex)
        {
            InitializeComponent();

            Debug.Assert(positionView != null);

            PositionView = positionView;
            FieldIndex   = fieldIndex  ;

            Ellipse      = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0.5d };

            Ellipse.SetBinding(Shape.  FillProperty, new Binding(nameof(  Fill)) { Source = this } );
            Ellipse.SetBinding(Shape.StrokeProperty, new Binding(nameof(Stroke)) { Source = this } );

            Content = Ellipse;
        }

        #endregion

        #region Dragging

        protected override void OnMouseDown(MouseButtonEventArgs ea)
        {
            base.OnMouseDown(ea);

          //if ((ea.LeftButton == MouseButtonState.Pressed) && (DragInProgress == false))
            if (DragInProgress == false)
            {
                DragInProgress  = true;
                DragFieldIndex  = FieldIndex;
                DragStartPoint  = ea.GetPosition(PositionView);
                DragStartZIndex = Panel.GetZIndex(Ellipse);

                Panel.SetZIndex(this, int.MaxValue);

                CaptureMouse();

                ea.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs ea)
        {
            base.OnMouseMove(ea);

            //PieceView pieceView = PositionView.GetPieceViewUnder(newPosition);

            //if (pieceView != null)
            //{
            //    pieceView.Background = Brushes.Purple;
            //}

            if (DragInProgress)
            {
                Point newPosition = ea.GetPosition(PositionView);

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
            DragInProgress = false;
            DragFieldIndex = 0    ;

            // ... before this
            ReleaseMouseCapture();
        }

        //protected override void OnMouseEnter(MouseEventArgs ea)
        //{
        //    base.OnMouseEnter(ea);

        //    MouseOverPieceView = this;

        //    //if (DragInProgress && (DragFieldIndex != FieldIndex))
        //    {
        //        Background = Brushes.Purple;

        //        ea.Handled = true;
        //    }
        //}

        //protected override void OnMouseLeave(MouseEventArgs ea)
        //{
        //    base.OnMouseLeave(ea);

        //    MouseOverPieceView = null;

        //    //if (DragInProgress && (DragFieldIndex != FieldIndex))
        //    {
        //        Background = null;

        //        ea.Handled = true;
        //    }
        //}

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty   FillProperty = DependencyProperty.Register(nameof(Fill  ), typeof(Brush), typeof(PieceView));
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PieceView));

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

        private PositionView PositionView       { get;      }

        private Ellipse      Ellipse            { get;      }

        private int          FieldIndex         { get;      }

        private PieceView    MouseOverPieceView { get => PositionView.MouseOverPieceView; set => PositionView.MouseOverPieceView = value; }
        private bool         DragInProgress     { get => PositionView.DragInProgress    ; set => PositionView.DragInProgress     = value; }
        private int          DragFieldIndex     { get => PositionView.DragFieldIndex    ; set => PositionView.DragFieldIndex     = value; }
        private Point        DragStartPoint     { get; set; }
        private int          DragStartZIndex    { get; set; }

        #endregion
    }
}
