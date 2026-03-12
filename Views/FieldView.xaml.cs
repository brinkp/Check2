using Check.Models;
using Check.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Check.Views
{
    public partial class FieldView
    {
        #region Constructors

        internal FieldView(PositionView positionView, PositionViewModel positionViewModel, FieldViewModel fieldViewModel, int fieldIndex)
        {
            InitializeComponent();

            Debug.Assert(positionView      != null);
            Debug.Assert(positionViewModel != null);
            Debug.Assert(fieldViewModel    != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

            PositionView      = positionView     ;
            PositionViewModel = positionViewModel;
            FieldViewModel    = fieldViewModel   ;
            FieldIndex        = fieldIndex       ;

            Ellipse      = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0.5d };

            Ellipse.SetBinding(Shape.  FillProperty, new Binding { Source = fieldViewModel, Converter = new FieldToColorConverterFill  () } ) ;
            Ellipse.SetBinding(Shape.StrokeProperty, new Binding { Source = fieldViewModel, Converter = new FieldToColorConverterStroke() } ) ;

            Content = Ellipse;

            DataContext  = fieldViewModel;
        }

        #endregion

        #region Dragging

        protected override void OnMouseEnter(MouseEventArgs ea)
        {
            base.OnMouseEnter(ea);

            PositionView.OnFieldMouseEnter(FieldIndex, FieldViewModel);

            ea.Handled = true;
        }

        protected override void OnMouseLeave(MouseEventArgs ea)
        {
            base.OnMouseLeave(ea);

            PositionView.OnFieldMouseLeave(FieldIndex, FieldViewModel);

            ea.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs ea)
        {
            base.OnMouseLeftButtonDown(ea);

            PositionView.OnFieldMouseLeftButtonDown(FieldIndex, FieldViewModel);

            ea.Handled = true;
        }

        //protected override void OnMouseDown(MouseButtonEventArgs ea)
        //{
        //    base.OnMouseDown(ea);

        //  //if ((ea.LeftButton == MouseButtonState.Pressed) && (DragInProgress == false))
        //    if (DragInProgress == false)
        //    {
        //        DragInProgress  = true;
        //        DragFieldIndex  = FieldIndex;
        //        DragStartPoint  = ea.GetPosition(PositionView);
        //        DragStartZIndex = Panel.GetZIndex(Ellipse);

        //        Panel.SetZIndex(this, int.MaxValue);

        //        CaptureMouse();

        //        ea.Handled = true;
        //    }
        //}

        //protected override void OnMouseMove(MouseEventArgs ea)
        //{
        //    base.OnMouseMove(ea);

        //    if (DragInProgress)
        //    {
        //        Point newPosition = ea.GetPosition(PositionView);

        //        Ellipse.RenderTransform = new TranslateTransform(newPosition.X - DragStartPoint.X, newPosition.Y - DragStartPoint.Y);

        //        ea.Handled = true;
        //    }
        //}

        //protected override void OnMouseUp(MouseButtonEventArgs ea)
        //{
        //    base.OnMouseUp(ea);

        //    if (DragInProgress)
        //    {
        //        OnMouseUp();

        //        ea.Handled = true;
        //    }
        //}

        //protected override void OnLostMouseCapture(MouseEventArgs ea)
        //{
        //    base.OnLostMouseCapture(ea);

        //    if (DragInProgress)
        //    {
        //        OnMouseUp();

        //        ea.Handled = true;
        //    }
        //}

        //private void OnMouseUp()
        //{
        //    Ellipse.RenderTransform = null;

        //    Panel.SetZIndex(this, DragStartZIndex);

        //    // Do this...
        //    DragInProgress = false;
        //    DragFieldIndex = 0    ;

        //    // ... before this
        //    ReleaseMouseCapture();
        //}

        #endregion

        #region Private properties

        private PositionView      PositionView       { get;      }
        private PositionViewModel PositionViewModel  { get;      }

        private FieldViewModel    FieldViewModel     { get;      }
        private int               FieldIndex         { get;      }

        private Ellipse           Ellipse            { get;      }

      //private FieldViewModel    MouseOverFieldView { get => PositionView.MouseOverFieldViewModel; set => PositionView.MouseOverFieldViewModel = value; }
      //private bool              DragInProgress     { get => PositionView.DragInProgress         ; set => PositionView.DragInProgress          = value; }
      //private int               DragFieldIndex     { get => PositionView.DragFieldIndex         ; set => PositionView.DragFieldIndex          = value; }
        private Point             DragStartPoint     { get; set; }
        private int               DragStartZIndex    { get; set; }

        #endregion

        #region FieldToColorConverterFill

        internal class FieldToColorConverterFill : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType == typeof(Brush));

                if (value is FieldViewModel fieldViewModel)
                {
                    switch (fieldViewModel.FieldContent)
                    {
                        case Position.FieldContentEnum.Empty:
                            result = Brushes.Transparent;
                            break;
                        case Position.FieldContentEnum.WhitePiece:
                            result = Brushes.White;
                            break;
                        case Position.FieldContentEnum.BlackPiece:
                            result = Brushes.Black;
                            break;
                        case Position.FieldContentEnum.WhiteRook:
                            result = Brushes.White;
                            break;
                        case Position.FieldContentEnum.BlackRook:
                            result = Brushes.Black;
                            break;
                        default:
                            throw new Exception("Invalid Field value");
                    }
                }
                else
                {
                    result = Brushes.Transparent;
                }

                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region FieldToColorConverterStroke

        internal class FieldToColorConverterStroke : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType == typeof(Brush));

                if (value is FieldViewModel fieldViewModel)
                {
                    switch (fieldViewModel.FieldContent)
                    {
                        case Position.FieldContentEnum.Empty:
                            result = Brushes.Transparent;
                            break;
                        default:
                            result = Brushes.Black;
                            break;
                    }
                }
                else
                {
                    result = Brushes.White;
                }

                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
