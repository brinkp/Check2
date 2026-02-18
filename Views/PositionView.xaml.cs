using Check.Models;
using Check.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Check.Views
{
    public partial class PositionView
    {
        #region Constructors

        public PositionView(PositionViewModel positionViewModel)
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

            int      delta = 0;
            int fieldIndex = 1;

            for (int rowIndex = 0; rowIndex < 10; rowIndex += 1)
            {
                bool  lastRow = rowIndex == 9;

                for (int columnIndex = 0; columnIndex < 10; columnIndex += 2)
                {
                    int      columnBorder    = columnIndex + delta    ;
                    int      columnPieceView = columnIndex - delta + 1;

                    bool lastColumnBorder    = columnBorder    == 9   ;
                    bool lastColumnPieceView = columnPieceView == 9   ;

                    Border      border1 = new Border { Background = Brushes.White     , BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnBorder    ? 1d : 0d, lastRow ? 1d : 0d) };
                    Border      border2 = new Border { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnPieceView ? 1d : 0d, lastRow ? 1d : 0d) } ;

                    PieceView pieceView = new PieceView(this, fieldIndex);

                    string  bindingPath = "F" + fieldIndex++.ToString("00");

                    pieceView.SetBinding(PieceView.  FillProperty, new Binding(bindingPath) { Converter = fieldToColorConverterFill   } ) ;
                    pieceView.SetBinding(PieceView.StrokeProperty, new Binding(bindingPath) { Converter = fieldToColorConverterStroke } ) ;

                    Grid.SetRow   (border1  , rowIndex        );
                    Grid.SetColumn(border1  , columnBorder    );

                    Grid.SetRow   (border2  , rowIndex        );
                    Grid.SetColumn(border2  , columnPieceView);

                    Grid.SetRow   (pieceView, rowIndex       );
                    Grid.SetColumn(pieceView, columnPieceView);

                    grid.Children.Add(border1  );
                    grid.Children.Add(border2  );

                    grid.Children.Add(pieceView);
                }

                delta = 1 - delta;
            }

            Content = grid;

            DataContext = positionViewModel;
        }

        #endregion

        #region Public properties

        internal bool DragInProgress { get; set; }
        internal int  DragFieldIndex { get; set; }

        #endregion
    }

    #region FieldToColorConverterFill

    public class FieldToColorConverterFill : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush result;

            if (value is Position.Field field)
            {
                switch (field)
                {
                    case Position.Field.Empty:
                        result = Brushes.Transparent;
                        break;
                    case Position.Field.WhitePiece:
                        result = Brushes.White;
                        break;
                    case Position.Field.BlackPiece:
                        result = Brushes.Black;
                        break;
                    case Position.Field.WhiteRook:
                        result = Brushes.White;
                        break;
                    case Position.Field.BlackRook:
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

            if (value is Position.Field field)
            {
                switch (field)
                {
                    case Position.Field.Empty:
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
