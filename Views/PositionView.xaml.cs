using Check.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Check.Models;
using static Check.ViewModels.FieldViewModel;

namespace Check.Views
{
    public partial class PositionView
    {
        #region Constructors

        internal PositionView(PositionViewModel positionViewModel)
        {
            Debug.Assert(positionViewModel != null);

            InitializeComponent();

            PositionViewModel = positionViewModel;

            Grid grid = new Grid();

            for (int index = 0; index < 10; index += 1)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(50d) } );
                grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength(50d) } );
            }

            FieldToBackgroundColorConverterFill fieldToBackgroundColorConverterFill = new FieldToBackgroundColorConverterFill();

          //FieldViews      = new FieldView     [50];
            FieldViewModels = new FieldViewModel[50];

            int       delta = 0;
            int  fieldIndex = 1;

            for (int rowIndex = 0; rowIndex < 10; rowIndex += 1)
            {
                bool  lastRow = rowIndex == 9;

                for (int columnIndex = 0; columnIndex < 10; columnIndex += 2)
                {
                    int      columnBorder    = columnIndex + delta    ;
                    int      columnFieldView = columnIndex - delta + 1;

                    bool lastColumnBorder    = columnBorder    == 9   ;
                    bool lastColumnFieldView = columnFieldView == 9   ;

                    Border       border1 = new Border { Background = Brushes.White     , BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnBorder    ? 1d : 0d, lastRow ? 1d : 0d) } ;
                    Border       border2 = new Border { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnFieldView ? 1d : 0d, lastRow ? 1d : 0d) } ;

                    FieldViewModel fieldViewModel =    FieldViewModels[fieldIndex - 1] =    new FieldViewModel(      positionViewModel,                 fieldIndex);
                    FieldView      fieldView      = /* FieldViews     [fieldIndex - 1] = */ new FieldView     (this, positionViewModel, fieldViewModel, fieldIndex);

                    border2.SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill } );

                    fieldIndex += 1;

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

                if (MouseOverFieldViewModel != null) MouseOverFieldViewModel.FieldStatus = FieldStatusEnum.Default  ;

                    MouseOverFieldViewModel  = FieldViewModels[fieldViewIndex];

                    if (MouseOverFieldViewModel != null)
                    {
                        int fieldViewIndex1 = fieldViewIndex + 1;

                        foreach (Move move in Position.PossibleMoves)
                        {
                            if (move.FromField == fieldViewIndex1)
                            {
                                MouseOverFieldViewModel.FieldStatus = FieldStatusEnum.MouseOver;
                                break;
                            }
                        }
                    }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (MouseOverFieldViewModel != null) MouseOverFieldViewModel.FieldStatus = FieldStatusEnum.Default  ;
        }

        #region Public properties

        internal FieldViewModel  MouseOverFieldViewModel { get; set; }

        internal bool       DragInProgress     { get; set; }
        internal int        DragFieldIndex     { get; set; }

        #endregion

        #region Private properties

        private PositionViewModel PositionViewModel { get; }

      //private FieldView     []  FieldViews        { get; }
        private FieldViewModel[]  FieldViewModels   { get; }

        private Position Position => PositionViewModel?.Position;

        #endregion

        #region FieldToBackgroundColorConverterFill

        internal class FieldToBackgroundColorConverterFill : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType == typeof(Brush));

                FieldStatusEnum fieldStatusEnum = FieldStatusEnum.Default;

                if (value is FieldViewModel fieldViewModel)
                {
                    fieldStatusEnum = fieldViewModel.FieldStatus;
                }
                else if (value is FieldStatusEnum @enum)
                {
                    fieldStatusEnum = @enum;
                }

                switch (fieldStatusEnum)
                {
                    case FieldStatusEnum.Default:
                        result = Brushes.SandyBrown;
                        break;
                    case FieldStatusEnum.MouseOver:
                        result = Brushes.Red;
                        break;
                    case FieldStatusEnum.CanStart:
                        result = Brushes.Red;
                        break;
                    case FieldStatusEnum.Started:
                        result = Brushes.Red;
                        break;
                    case FieldStatusEnum.CanBeTaken:
                        result = Brushes.Red;
                        break;
                    case FieldStatusEnum.Taken:
                        result = Brushes.Red;
                        break;
                    default:
                        result = Brushes.Transparent;
                        break;
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
