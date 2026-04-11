using Check.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Check.Models;
using static Check.ViewModels.FieldViewModel;
// ReSharper disable LocalizableElement

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

                    FieldViewModel fieldViewModel =    FieldViewModels[fieldIndex - 1] =    new FieldViewModel(      positionViewModel, fieldIndex);
                    FieldView      fieldView      = /* FieldViews     [fieldIndex - 1] = */ new FieldView     (this,    fieldViewModel, fieldIndex);

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

            ResetStatus();

            IndicatePossibleFromFields();
        }

        #endregion

        #region Event handlers

        internal void OnFieldMouseEnter(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            switch (PositionStatus)
            {
                case PositionViewModel.PositionStatusEnum.Default:
                    if (Position.PossibleMoves.Any(move => (move.FromField ==      fieldIndex)                                )) fieldViewModel.FieldStatus = FieldStatusEnum.MouseOverCanBeFrom;
                    break;
                case PositionViewModel.PositionStatusEnum.FromGiven:
                    if (Position.PossibleMoves.Any(move => (move.FromField == FromFieldIndex) && (move.ToField == fieldIndex))) fieldViewModel.FieldStatus = FieldStatusEnum.MouseOverCanBeTo  ;
                    break;
                case PositionViewModel.PositionStatusEnum.TakeInProgress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
            }
        }

        internal void OnFieldMouseLeave(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            switch (PositionStatus)
            {
                case PositionViewModel.PositionStatusEnum.Default:
                    if (fieldViewModel.FieldStatus == FieldStatusEnum.MouseOverCanBeFrom) fieldViewModel.FieldStatus = FieldStatusEnum.CanBeFrom;
                    break;
                case PositionViewModel.PositionStatusEnum.FromGiven:
                    if (fieldViewModel.FieldStatus == FieldStatusEnum.MouseOverCanBeTo  ) fieldViewModel.FieldStatus = FieldStatusEnum.CanBeTo  ;
                    break;
                case PositionViewModel.PositionStatusEnum.TakeInProgress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
            }
        }

        internal void OnFieldMouseLeftButtonDown(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            switch (PositionStatus)
            {
                case PositionViewModel.PositionStatusEnum.Default:
                    CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel);
                    break;
                case PositionViewModel.PositionStatusEnum.FromGiven:
                    if (fieldIndex == FromFieldIndex)
                    {
                        SetDefaultStatus(fieldViewModel);
                    }
                    else
                    {
                        bool moved = false;

                        foreach (Move move in Position.PossibleMoves.Where(move => (move.FromField == FromFieldIndex) && (move.ToField == fieldIndex)))
                        {
                            moved = true;

                            Position.Move(move);

                            SetDefaultStatus(fieldViewModel);
                            break;
                        }

                        if (! moved)
                        {
                            if (! CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel))
                            {
                                SetDefaultStatus(fieldViewModel);
                            }
                        }
                    }
                    break;
                case PositionViewModel.PositionStatusEnum.TakeInProgress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
            }
        }

        private bool CheckIfFieldCanBeFrom(int fieldIndex, FieldViewModel fieldViewModel)
        {
            bool result = false;

            if (Position.PossibleMoves.Any(move => move.FromField == fieldIndex))
            {
                ResetStatus();

                fieldViewModel.FieldStatus = FieldStatusEnum.FromGiven;

                IndicatePossibleToFields(fieldIndex);

                FromFieldViewModel = fieldViewModel;
                FromFieldIndex     = fieldIndex    ;

                PositionStatus = PositionViewModel.PositionStatusEnum.FromGiven;

                result = true;
            }

            return result;
        }

        #endregion

        #region Public properties

      //internal FieldViewModel  MouseOverFieldViewModel { get; set; }

      //internal bool            DragInProgress          { get; set; }
      //internal int             DragFieldIndex          { get; set; }

        #endregion

        #region Private properties

        private PositionViewModel PositionViewModel { get; }

      //private FieldView     []  FieldViews        { get; }
        private FieldViewModel[]  FieldViewModels   { get; }

        private Position                             Position       => PositionViewModel?.Position;
        private PositionViewModel.PositionStatusEnum PositionStatus
        {
            get =>  PositionViewModel?.PositionStatus ?? PositionViewModel.PositionStatusEnum.Default;
            set
            {
                if (PositionViewModel != null)
                {
                    PositionViewModel.PositionStatus = value;
                }
            }
        }

        private int            FromFieldIndex     { get; set; }
        private FieldViewModel FromFieldViewModel { get; set; }

        #endregion

        #region Private methods

        private void IndicatePossibleFromFields(                  ) { foreach (Move move in Position.PossibleMoves                    ) { FieldViewModels[move.FromField - 1].FieldStatus = FieldStatusEnum.CanBeFrom; } }
        private void IndicatePossibleToFields  (int fromFieldIndex) { foreach (Move move in Position.PossibleMovesFrom(fromFieldIndex)) { FieldViewModels[move.  ToField - 1].FieldStatus = FieldStatusEnum.CanBeTo  ; } }

        private void ResetStatus()
        {
            foreach (FieldViewModel fieldViewModel in FieldViewModels)
            {
                fieldViewModel.ResetStatus();
            }
        }

        private void SetDefaultStatus(FieldViewModel fieldViewModel)
        {
            fieldViewModel.FieldStatus = FieldStatusEnum.Default;

            FromFieldViewModel = null;
            FromFieldIndex     =    0;

            PositionStatus = PositionViewModel.PositionStatusEnum.Default;

            ResetStatus();

            IndicatePossibleFromFields();
        }

        //private void Refresh()
        //{
        //    foreach (FieldViewModel fieldViewModel in FieldViewModels)
        //    {
        //        fieldViewModel.Refresh();
        //    }
        //}

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
                    case FieldStatusEnum.CanBeFrom:
                    case FieldStatusEnum.CanBeTo  :
                        result = Brushes.LightSeaGreen;
                        break;
                    case FieldStatusEnum.MouseOverCanBeFrom:
                    case FieldStatusEnum.MouseOverCanBeTo  :
                        result = Brushes.Green;
                        break;
                    case FieldStatusEnum.FromGiven:
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
