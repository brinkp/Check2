using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Check.Models;
using Check.ViewModels;

namespace Check.Views
{
    public partial class PositionView
    {
        #region Constructors

        public PositionView(PositionViewModel positionViewModel)
        {
            Debug.Assert(positionViewModel != null);

            InitializeComponent();

            DataContext = positionViewModel;
        }

        #endregion
    }

    #region FieldToColorConverter

    public class FieldToColorConverter : IValueConverter
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
}
