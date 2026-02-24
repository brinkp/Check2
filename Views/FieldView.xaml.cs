using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Check.ViewModels;

namespace Check.Views
{
    public partial class FieldView
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

        internal FieldView(PositionView positionView, FieldViewModel fieldViewModel, int fieldIndex)
        {
            InitializeComponent();

            Debug.Assert(positionView   != null);
            Debug.Assert(fieldViewModel != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

            PositionView = positionView  ;
            FieldIndex   = fieldIndex    ;

            Ellipse      = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0.5d };

            Ellipse.SetBinding(Shape.  FillProperty, new Binding(nameof(FieldViewModel.Fill  )));
            Ellipse.SetBinding(Shape.StrokeProperty, new Binding(nameof(FieldViewModel.Stroke)));

            Content = Ellipse;

            DataContext  = fieldViewModel;
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

        #endregion

        #region Private properties

        private PositionView PositionView       { get;      }

        private Ellipse      Ellipse            { get;      }

        private int          FieldIndex         { get;      }

        private FieldView    MouseOverFieldView { get => PositionView.MouseOverFieldView; set => PositionView.MouseOverFieldView = value; }
        private bool         DragInProgress     { get => PositionView.DragInProgress    ; set => PositionView.DragInProgress     = value; }
        private int          DragFieldIndex     { get => PositionView.DragFieldIndex    ; set => PositionView.DragFieldIndex     = value; }
        private Point        DragStartPoint     { get; set; }
        private int          DragStartZIndex    { get; set; }

        #endregion
    }
}
