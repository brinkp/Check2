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

namespace Check.Views
{
    public partial class PositionView
    {
        #region Constructors

        internal PositionView(PositionViewModel positionViewModel)
        {
            Debug.Assert(positionViewModel != null);

            InitializeComponent();

            Grid grid = new Grid();

            for (int index = 0; index < 10; index += 1)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(50d) } );
                grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength(50d) } );
            }

            FieldToColorConverterFill   fieldToColorConverterFill   = new FieldToColorConverterFill  ();
            FieldToColorConverterStroke fieldToColorConverterStroke = new FieldToColorConverterStroke();

            FieldViews = new FieldView[50];

            int      delta = 0;
            int fieldIndex = 1;

            for (int rowIndex = 0; rowIndex < 10; rowIndex += 1)
            {
                bool  lastRow = rowIndex == 9;

                for (int columnIndex = 0; columnIndex < 10; columnIndex += 2)
                {
                    int      columnBorder    = columnIndex + delta    ;
                    int      columnFieldView = columnIndex - delta + 1;

                    bool lastColumnBorder    = columnBorder    == 9   ;
                    bool lastColumnFieldView = columnFieldView == 9   ;

                    Border      border1 = new Border { Background = Brushes.White     , BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnBorder    ? 1d : 0d, lastRow ? 1d : 0d) };
                    Border      border2 = new Border { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnFieldView ? 1d : 0d, lastRow ? 1d : 0d) } ;

                    FieldView fieldView = new FieldView(this, new FieldViewModel(), fieldIndex);

                    FieldViews[fieldIndex - 1] = fieldView;

                    string  bindingPath = "F" + fieldIndex++.ToString("00");

                    fieldView.SetBinding(FieldViewModel.  FillProperty, new Binding(bindingPath) { Converter = fieldToColorConverterFill   } ) ;
                    fieldView.SetBinding(FieldViewModel.StrokeProperty, new Binding(bindingPath) { Converter = fieldToColorConverterStroke } ) ;

                    Grid.SetRow   (border1  , rowIndex        );
                    Grid.SetColumn(border1  , columnBorder    );

                    Grid.SetRow   (border2  , rowIndex        );
                    Grid.SetColumn(border2  , columnFieldView);

                    Grid.SetRow   (fieldView, rowIndex       );
                    Grid.SetColumn(fieldView, columnFieldView);

                    grid.Children.Add(border1  );
                    grid.Children.Add(border2  );

                    grid.Children.Add(fieldView);
                }

                delta = 1 - delta;
            }

            Content = grid;

            DataContext = positionViewModel;
        }

        #endregion

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            Point position = e.GetPosition(this);

            int row        = (int) position.Y / 50;
            int column     = (int) position.X / 50;

            bool    rowIsEven =     row % 2 == 0;
            bool columnIsEven =  column % 2 == 0;

            if (rowIsEven == ! columnIsEven)
            {
                int fieldViewIndex = row * 5 + column / 2;

                if (MouseOverFieldView != null) MouseOverFieldView.Background = null;

                    MouseOverFieldView  = FieldViews[fieldViewIndex];

                if (MouseOverFieldView != null) MouseOverFieldView.Background = Brushes.Purple;
            }
        }

        #region Public properties

        internal FieldView  MouseOverFieldView { get; set; }
        internal bool       DragInProgress     { get; set; }
        internal int        DragFieldIndex     { get; set; }

        #endregion

        #region Private properties

        private FieldView[] FieldViews { get; }

        #endregion

        #region Private methods

        public FieldView GetFieldViewUnder(Point position)
        {
            FieldView result = null;

            foreach (FieldView fieldView in FieldViews)
            {
                if (fieldView.IsMouseOver)
                {
                    result = fieldView;
                    break;
                }
            }

            return result;
        }

        #endregion
    }

    #region FieldToColorConverterFill

    public class FieldToColorConverterFill : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush result;

            if (value is Position.FieldContentEnum field)
            {
                switch (field)
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

    #region FieldToColorConverterStroke

    public class FieldToColorConverterStroke : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush result;

            if (value is Position.FieldContentEnum field)
            {
                switch (field)
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
