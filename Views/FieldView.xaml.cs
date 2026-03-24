using Check.Models;
using Check.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Check.Views
{
    public partial class FieldView
    {
        #region Constructors

        internal FieldView(PositionView positionView, FieldViewModel fieldViewModel, int fieldIndex)
        {
            InitializeComponent();

            Debug.Assert(positionView      != null);
            Debug.Assert(fieldViewModel    != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

            PositionView      = positionView     ;
          //PositionViewModel = positionViewModel;
            FieldViewModel    = fieldViewModel   ;
            FieldIndex        = fieldIndex       ;

            Grid    grid      = new Grid    { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch } ;
            Ellipse ellipse   = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, StrokeThickness = 0.5d };

            ellipse.SetBinding(Shape.  FillProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath("FieldContent"), Converter = new FieldToColorConverterFill      () } ) ;
            ellipse.SetBinding(Shape.StrokeProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath("FieldContent"), Converter = new FieldToColorConverterStroke    () } ) ;

            grid.Children.Add(ellipse);

            ellipse           = new Ellipse { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, StrokeThickness = 1.0d, Margin = new Thickness(5d) };

            ellipse.SetBinding(Shape.StrokeProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath("FieldContent"), Converter = new FieldToColorConverterStrokeKing() } ) ;

            grid.Children.Add(ellipse);

            Content           = grid;

            DataContext       = fieldViewModel;
        }

        #endregion

        #region Event handlers

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

        #endregion

        #region Private properties

        private PositionView   PositionView   { get; }
        private FieldViewModel FieldViewModel { get; }
        private int            FieldIndex     { get; }

        #endregion

        #region FieldToColorConverterFill

        internal class FieldToColorConverterFill : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType == typeof(Brush));

                if (value is Position.FieldContentEnum fieldContent)
                {
                    switch (fieldContent)
                    {
                        case Position.FieldContentEnum.Empty:
                            result = Brushes.Transparent;
                            break;
                        case Position.FieldContentEnum.WhiteMan:
                            result = Brushes.White;
                            break;
                        case Position.FieldContentEnum.BlackMan:
                            result = Brushes.Black;
                            break;
                        case Position.FieldContentEnum.WhiteKing:
                            result = Brushes.White;
                            break;
                        case Position.FieldContentEnum.BlackKing:
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

                if (value is Position.FieldContentEnum fieldContent)
                {
                    switch (fieldContent)
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

        #region FieldToColorConverterStrokeKing

        internal class FieldToColorConverterStrokeKing : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType == typeof(Brush));

                if (value is Position.FieldContentEnum fieldContent)
                {
                    switch (fieldContent)
                    {
                        case Position.FieldContentEnum.Empty:
                        case Position.FieldContentEnum.WhiteMan:
                        case Position.FieldContentEnum.BlackMan:
                            result = Brushes.Transparent;
                            break;
                        case Position.FieldContentEnum.WhiteKing:
                            result = Brushes.Black;
                            break;
                        case Position.FieldContentEnum.BlackKing:
                            result = Brushes.White;
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
    }
}
