using Check.Models;
using Check.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static Check.ViewModels.FieldViewModel;
// ReSharper disable LocalizableElement

namespace Check.Views
{
    public partial class PositionView
    {
        #region Constants

        private const int NumberOfRows          = 10;
        private const int NumberOfColumns       = 10;

        private const int NumberOfRowsOrColumns = 10;

        private const int NumberOfRows1         = NumberOfRows    - 1;
        private const int NumberOfColumns1      = NumberOfColumns - 1;

        private const int NumberOfFields        = 50;
        private const int NumberOfFields2       = NumberOfFields / 2;

        private const int DisplayOfIntermediatePositionsDelay = 1000;

        #endregion

        #region Constructors

        internal PositionView(PositionViewModel positionViewModel)
        {
            Debug.Assert(positionViewModel != null);

            InitializeComponent();

            PositionViewModel = positionViewModel;

            for (int index = 0; index < NumberOfRowsOrColumns; index += 1)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition { Width  = new GridLength(50d) } );
                Grid.   RowDefinitions.Add(new    RowDefinition { Height = new GridLength(50d) } );
            }

            FieldToBackgroundColorConverterFill fieldToBackgroundColorConverterFill = new FieldToBackgroundColorConverterFill();

            FieldViewModels = new FieldViewModel[NumberOfFields];
            FieldViews      = new FieldView     [NumberOfFields];
            Borders         = new Border        [NumberOfFields];

            int       delta = 0;
            int  fieldIndex = 1;

            for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex += 1)
            {
                bool  lastRow = rowIndex == (NumberOfRows1);

                for (int columnIndex = 0; columnIndex < NumberOfColumns; columnIndex += 2)
                {
                    int      columnBorder    = columnIndex + delta    ;
                    int      columnFieldView = columnIndex - delta + 1;

                    bool lastColumnBorder    = columnBorder    == NumberOfColumns1;
                    bool lastColumnFieldView = columnFieldView == NumberOfColumns1;

                    Border       border1 =                           new Border { Background = Brushes.White     , BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnBorder    ? 1d : 0d, lastRow ? 1d : 0d) } ;
                    Border       border2 = Borders[fieldIndex - 1] = new Border { Background = Brushes.SandyBrown, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1d, 1d, lastColumnFieldView ? 1d : 0d, lastRow ? 1d : 0d) } ;

                    FieldViewModel fieldViewModel = FieldViewModels[fieldIndex - 1] = new FieldViewModel(      positionViewModel, fieldIndex);
                    FieldView      fieldView      = FieldViews     [fieldIndex - 1] = new FieldView     (this,    fieldViewModel, fieldIndex);

                    border2.SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = this } );

                    fieldIndex += 1;

                    Grid.SetRow   (border1  , rowIndex        );
                    Grid.SetColumn(border1  , columnBorder    );

                    Grid.SetRow   (border2  , rowIndex        );
                    Grid.SetColumn(border2  , columnFieldView);

                    Grid.SetRow   (fieldView, rowIndex       );
                    Grid.SetColumn(fieldView, columnFieldView);

                    Grid.Children.Add(border1  );
                    Grid.Children.Add(border2  );

                    Grid.Children.Add(fieldView);
                }

                delta = 1 - delta;
            }

            Content     = Grid;

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
                    if (Position.PossibleMoves.Any(move => (move.FromField == fieldIndex)))
                    {
                        fieldViewModel.FieldStatus = FieldStatusEnum.MouseOverCanBeFrom;

                        IndicatePossibleTakeFields(fieldIndex, FieldStatusEnum.CanBeTaken);
                    }
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

                    IndicatePossibleTakeFields(fieldIndex, FieldStatusEnum.Default);
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

        internal async Task OnFieldMouseLeftButtonDown(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            switch (PositionStatus)
            {
                case PositionViewModel.PositionStatusEnum.Default:
                    await CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel);
                    break;
                case PositionViewModel.PositionStatusEnum.FromGiven:
                    if (fieldIndex == FromFieldIndex)
                    {
                        SetDefaultStatus(fieldViewModel);
                    }
                    else
                    {
                        List<Move> possibleMoves = Position.PossibleMoves.Where(move => (move.FromField == FromFieldIndex) && (move.ToField == fieldIndex)).ToList();

                        if (possibleMoves.Count > 0)
                        {
                            await HandleMoveExt(possibleMoves.First(), fieldViewModel);
                        }
                        else
                        {
                            if (! await CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel))
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

        internal async Task CheckForControlKeys(KeyEventArgs ea)
        {
            base.OnKeyDown(ea);

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (ea.Key)
                {
                    case Key.A:
                        AutomaticMoves = ! AutomaticMoves;
                        break;
                    case Key.F:
                        GiveVisualFeedback = ! GiveVisualFeedback;

                        RefreshFields();

                        ea.Handled = true;
                        break;
                    case Key.I:
                        DisplayIntermediatePositions = ! DisplayIntermediatePositions;

                        ea.Handled = true;
                        break;
                    case Key.L:
                        LoadPosition();

                        ea.Handled = true;
                        break;
                    case Key.M:
                        MoveRandom();

                        ea.Handled = true;
                        break;
                    case Key.N:
                        Position.Initialize((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift);

                        ResetStatus();

                        IndicatePossibleFromFields();

                        UndoMoveStack.Clear();
                        RedoMoveStack.Clear();

                        ea.Handled = true;
                        break;
                    case Key.P:
                        await PlayRandom();

                        ea.Handled = true;
                        break;
                    case Key.S:
                        SavePosition();

                        ea.Handled = true;
                        break;
                    case Key.W:
                        await SolveCombination();

                        ea.Handled = true;
                        break;
                    case Key.Y:
                        if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                        {
                            while (RedoMoveStack.Count > 0)
                            {
                                RedoLastMove();

                                await PauseIfRequired();
                            }
                        }
                        else
                        {
                            RedoLastMove();
                        }

                        ea.Handled = false;
                        break;
                    case Key.Z:
                        if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                        {
                            while (UndoMoveStack.Count > 0)
                            {
                                UndoLastMove();

                                await PauseIfRequired();
                            }
                        }
                        else
                        {
                            UndoLastMove();
                        }

                        ea.Handled = false;
                        break;
                }
            }
            else
            {
                switch (ea.Key)
                {
                    case Key.Add    :
                    case Key.OemPlus:
                        if (DelayOfDisplayOfIntermediatePositions ==   1)
                        {
                            DelayOfDisplayOfIntermediatePositions  = 100;
                        }
                        else
                        {
                            DelayOfDisplayOfIntermediatePositions += 100;
                        }

                        ea.Handled = true;
                        break;
                    case Key.Subtract:
                    case Key.OemMinus:
                        if (DelayOfDisplayOfIntermediatePositions == 100)
                        {
                            DelayOfDisplayOfIntermediatePositions  =   1;
                        }
                        else
                        {
                            DelayOfDisplayOfIntermediatePositions -= 100;
                        }

                        ea.Handled = true;
                        break;
                }
            }
        }

        #endregion

        #region Private properties

        private Grid Grid { get; } =  new Grid();

        private PositionViewModel PositionViewModel { get; }
        private FieldViewModel[]  FieldViewModels   { get; }
        private FieldView     []  FieldViews        { get; }
        private Border        []  Borders           { get; }

        private Position                             Position       => PositionViewModel?.Position;
        private PositionViewModel.PositionStatusEnum PositionStatus
        {
            get =>  PositionViewModel?.PositionStatus ?? PositionViewModel.PositionStatusEnum.Default;
            set
            {
                if (PositionViewModel != null) { PositionViewModel.PositionStatus = value; }
            }
        }

        private int   FromFieldIndex { get; set; }

        private bool _automaticMoves = Properties.Settings.Default.AutomaticMoves;
        private bool  AutomaticMoves
        {
            get => _automaticMoves;
            set
            {
                if (_automaticMoves != value)
                {
                    _automaticMoves  = value;

                    Properties.Settings.Default.AutomaticMoves = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _giveVisualFeedback = Properties.Settings.Default.GiveVisualFeedback;
        private bool  GiveVisualFeedback
        {
            get =>  _giveVisualFeedback;
            set
            {
                if (_giveVisualFeedback != value)
                {
                    _giveVisualFeedback  = value;

                    Properties.Settings.Default.GiveVisualFeedback = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private bool _displayIntermediatePositions = Properties.Settings.Default.DisplayIntermediatePositions;
        private bool  DisplayIntermediatePositions
        {
            get => _displayIntermediatePositions;
            set
            {
                if (_displayIntermediatePositions != value)
                {
                    _displayIntermediatePositions = value;

                    Properties.Settings.Default.DisplayIntermediatePositions = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private int _delayOfDisplayOfIntermediatePositions = Properties.Settings.Default.DelayOfDisplayOfIntermediatePositions;
        private int  DelayOfDisplayOfIntermediatePositions
        {
            get => _delayOfDisplayOfIntermediatePositions;
            set
            {
                if (_delayOfDisplayOfIntermediatePositions != value)
                {
                    if ((value >= 0) && (value <= 1000))
                    {
                        if (value == 0) value = 1;

                       _delayOfDisplayOfIntermediatePositions = value;

                        Properties.Settings.Default.DelayOfDisplayOfIntermediatePositions = value;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        private Stack<Move> UndoMoveStack { get; } = new Stack<Move>();
        private Stack<Move> RedoMoveStack { get; } = new Stack<Move>();

        #endregion


        #region Commands

        public void ToggleAutomaticMoves              () { AutomaticMoves               = ! AutomaticMoves              ; }
        public void ToggleGiveVisualFeedback          () { GiveVisualFeedback           = ! GiveVisualFeedback          ; }
        public void ToggleDisplayIntermediatePositions() { DisplayIntermediatePositions = ! DisplayIntermediatePositions; }

        public void LoadPosition()
        {
            Position.Load();

            ResetStatus();

            IndicatePossibleFromFields();

            UndoMoveStack.Clear();
            RedoMoveStack.Clear();
        }

        public void SavePosition()
        {
            Position.Save();
        }

        public void MoveRandom()
        {
            ResetStatus();

            Move move = PositionViewModel.MoveRandom();

            if  (move.IsValid)
            {
                SetDefaultStatus(null);

                UndoMoveStack.Push(move);
            }
        }

        public async Task PlayRandom()
        {
            ResetStatus();

            await PositionViewModel.PlayRandom(PauseIfRequiredExt);

            RefreshFields();
        }

        public async Task SolveCombination()
        {
            ResetStatus();

            Move bestMove = await PositionViewModel.SolveCombination(PauseIfRequired);

            if  (bestMove.IsValid)
            {
                Position.MoveInSitu(ref bestMove);

                RefreshFields();

                UndoMoveStack.Push(bestMove);
            }

            Position.GetMovesAndTakes();

            SetDefaultStatus(null);
        }

        public void FlipBoard()
        {
            // This could also be done with a RenderTransform, maybe combined with an animation

            for (int fieldIndex1 = 0; fieldIndex1 < NumberOfFields2; fieldIndex1 += 1)
            {
                int  fieldIndex2 = NumberOfFields - fieldIndex1 - 1;

                FieldView fieldView1  = FieldViews[fieldIndex1];
                FieldView fieldView2  = FieldViews[fieldIndex2];

                Border    border21    = Borders[fieldIndex1];
                Border    border22    = Borders[fieldIndex2];

                int       tempRow1    = Grid.GetRow   (fieldView1) ;
                int       tempRow2    = Grid.GetRow   (fieldView2) ;
                int       tempColumn1 = Grid.GetColumn(fieldView1) ;
                int       tempColumn2 = Grid.GetColumn(fieldView2) ;

                Grid.SetRow   (fieldView1, tempRow2   );
                Grid.SetColumn(fieldView1, tempColumn2);

                Grid.SetRow   (border21  , tempRow2   );
                Grid.SetColumn(border21  , tempColumn2);

                Grid.SetRow   (fieldView2, tempRow1   );
                Grid.SetColumn(fieldView2, tempColumn1);

                Grid.SetRow   (border22  , tempRow1   );
                Grid.SetColumn(border22  , tempColumn1);

                Grid.Children.Remove(fieldView1);
                Grid.Children.Remove(fieldView2);
                Grid.Children.Add   (fieldView1);
                Grid.Children.Add   (fieldView2);
            }

            IndicatePossibleFromFields();
        }

        public void FlipTurn()
        {
            Position.FlipTurn();

            Position.GetMovesAndTakes();

            SetDefaultStatus(null);
        }

        #endregion

        #region Undo and Redo

        public void UndoLastMove()
        {
            if (UndoMoveStack.Count > 0)
            {
                Move move = UndoMoveStack.Pop();

                RedoMoveStack.Push(move);

                Position.UndoMoveInSitu(ref move);

                Position.GetMovesAndTakes();

                SetDefaultStatus(null);
            }
        }

        public void RedoLastMove()
        {
            if (RedoMoveStack.Count > 0)
            {
                Move move = RedoMoveStack.Pop();

                UndoMoveStack.Push(move);

                Position.MoveInSitu(ref move);

                Position.GetMovesAndTakes();

                SetDefaultStatus(null);
            }
        }

        #endregion

        #region Support methods

        private async Task<bool> CheckAutomaticMoves(int fieldIndex = 0)
        {
            bool result = false;

            if (AutomaticMoves)
            {
                bool tempDisplay =          DisplayIntermediatePositions;
                int  tempDelay   = DelayOfDisplayOfIntermediatePositions;

                try
                {
                             DisplayIntermediatePositions = true;
                    DelayOfDisplayOfIntermediatePositions = DisplayOfIntermediatePositionsDelay;

                    int  numberOfMoves = (fieldIndex == 0) ? Position.NumberOfMoves                  : Position.PossibleMovesFromCount(fieldIndex)                 ;
                    Move move          = (fieldIndex == 0) ? Position.PossibleMoves.FirstOrDefault() : Position.PossibleMovesFrom     (fieldIndex).FirstOrDefault();

                    while (numberOfMoves == 1)
                    {
                        result = true;

                        HandleMove(move);

                        await PauseIfRequired();

                        numberOfMoves = Position.NumberOfMoves                 ;
                        move          = Position.PossibleMoves.FirstOrDefault();
                    }
                }
                finally
                {
                             DisplayIntermediatePositions = tempDisplay;
                    DelayOfDisplayOfIntermediatePositions = tempDelay;
                }
            }

            return result;
        }

        private async Task<bool> CheckIfFieldCanBeFrom(int fieldIndex, FieldViewModel fieldViewModel)
        {
            bool result      ;
            bool movePossible;

            switch (Position.PossibleMovesFromCount(fieldIndex))
            {
                case 0:
                    result       =   false;
                    movePossible =   false;
                    break;
                case 1:
                    result       =   true ;
                    movePossible = ! await CheckAutomaticMoves(fieldIndex);
                    break;
                default:
                    result       =   true ;
                    movePossible =   true ;
                    break;
            }

            if (movePossible)
            {
                ResetStatus();

                fieldViewModel.FieldStatus = FieldStatusEnum.FromGiven;

                IndicatePossibleToFields  (fieldIndex);
                IndicatePossibleTakeFields(fieldIndex, FieldStatusEnum.CanBeTaken);

                FromFieldIndex = fieldIndex;

                PositionStatus = PositionViewModel.PositionStatusEnum.FromGiven;
            }

            return result;
        }

        private async Task PauseIfRequired()
        {
            if (DisplayIntermediatePositions && (DelayOfDisplayOfIntermediatePositions > 1))
            {
                RefreshFields();

                await Task.Delay(DelayOfDisplayOfIntermediatePositions);
            }
        }

        private async Task PauseIfRequiredExt(Move move)
        {
            await PauseIfRequired();

            UndoMoveStack.Push(move);
        }

        #endregion

        #region User interface update methods

        private void UpdateUserInterface()
        {
            // ResetStatus               (): clear statuses of all fields and trigger bindings for all fields
            // Position.GetMovesAndTakes (): get all moves and takes in a new Position
            // IndicatePossibleFromFields(): for all moves set From field  status
            // IndicatePossibleToFields  (): for one move  set To   field  status
            // IndicatePossibleTakeFields(): for one move  set Take fields statuses
            // SetDefaultStatus          (): 
            // RefreshFields             (): trigger bindings for all fields
        }

        private void HandleMove(Move move, FieldViewModel fieldViewModel = null)
        {
            Position.MoveInSitu(ref move);

            Position.GetMovesAndTakes();

            UndoMoveStack.Push(move);

            SetDefaultStatus(fieldViewModel);
        }

        private async Task HandleMoveExt(Move move, FieldViewModel fieldViewModel)
        {
            if ((RedoMoveStack.Count > 0) && (RedoMoveStack.Peek().Equals(ref move)))
            {
                RedoLastMove();
            }
            else
            {
                HandleMove(move, fieldViewModel);
            }

            await CheckAutomaticMoves();
        }

        private void IndicatePossibleFromFields(                  ) { foreach (Move move in Position.PossibleMoves                    ) { FieldViewModels[move.FromField - 1].FieldStatus = FieldStatusEnum.CanBeFrom; } }
        private void IndicatePossibleToFields  (int fromFieldIndex) { foreach (Move move in Position.PossibleMovesFrom(fromFieldIndex)) { FieldViewModels[move.  ToField - 1].FieldStatus = FieldStatusEnum.CanBeTo  ; } }
        private void IndicatePossibleTakeFields(int fromFieldIndex, FieldStatusEnum fieldStatus)
        {
            foreach (Move move in Position.PossibleMovesFrom(fromFieldIndex))
            {
                if (move.TakeFields?.Count > 0)
                {
                    foreach (int fieldIndexTake in move.TakeFields)
                    {
                        FieldViewModels[fieldIndexTake - 1].FieldStatus = fieldStatus;
                    }
                }
            }
        }
        private void ResetStatus()
        {
            foreach (FieldViewModel fieldViewModel in FieldViewModels)
            {
                fieldViewModel.ResetStatus();
            }
        }

        private void SetDefaultStatus(FieldViewModel fieldViewModel)
        {
            if (fieldViewModel != null) fieldViewModel.FieldStatus = FieldStatusEnum.Default;

            FromFieldIndex = 0;

            PositionStatus = PositionViewModel.PositionStatusEnum.Default;

            ResetStatus();

            IndicatePossibleFromFields();
        }

        private void RefreshFields()
        {
            foreach (FieldViewModel fieldViewModel in FieldViewModels)
            {
                fieldViewModel.Refresh();
            }
        }

        #endregion

        #region FieldToBackgroundColorConverterFill

        internal class FieldToBackgroundColorConverterFill : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Brush result;

                Debug.Assert(targetType   == typeof(Brush));

                PositionView positionView  = parameter as PositionView;

                Debug.Assert(positionView != null);

                FieldStatusEnum fieldStatusEnum = FieldStatusEnum.Default;

                if (value is FieldViewModel fieldViewModel)
                {
                    Debugger.Break();

                    fieldStatusEnum = fieldViewModel.FieldStatus;
                }
                else if (value is FieldStatusEnum @enum)
                {
                    fieldStatusEnum = @enum;
                }

                if (positionView.GiveVisualFeedback)
                {
                    switch (fieldStatusEnum)
                    {
                        case FieldStatusEnum.Default:
                            result = Brushes.SandyBrown;
                            break;
                        case FieldStatusEnum.CanBeFrom:
                        case FieldStatusEnum.CanBeTo:
                            result = Brushes.LightSeaGreen;
                            break;
                        case FieldStatusEnum.MouseOverCanBeFrom:
                        case FieldStatusEnum.MouseOverCanBeTo:
                        case FieldStatusEnum.FromGiven:
                            result = Brushes.Green;
                            break;
                        case FieldStatusEnum.CanBeTaken:
                            result = Brushes.Red;
                            break;
                        default:
                            result = Brushes.Transparent;
                            break;
                    }
                }
                else
                {
                    switch (fieldStatusEnum)
                    {
                        case FieldStatusEnum.Default:
                        case FieldStatusEnum.CanBeFrom:
                        case FieldStatusEnum.CanBeTo:
                        case FieldStatusEnum.CanBeTaken:
                            result = Brushes.SandyBrown;
                            break;
                        case FieldStatusEnum.MouseOverCanBeFrom:
                        case FieldStatusEnum.MouseOverCanBeTo:
                        case FieldStatusEnum.FromGiven:
                            result = Brushes.Green;
                            break;
                        default:
                            result = Brushes.Transparent;
                            break;
                    }
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

    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            if (x is null || y is null) return false;

            int lengthX  = x.Length;
            int lengthY  = y.Length;

            if (lengthX != lengthY)     return false;

            if (ReferenceEquals(x, y))  return true ;

            for (int index = 0; index < x.Length; index++) { if (x[index] != y[index]) return false; }

            return true;
        }

        public int GetHashCode(byte[] obj)
        {
            if (obj is null) return 0;

            unchecked
            {
                int hash = 17;
                foreach (var b in obj)
                    hash = hash * 31 + b;
                return hash;
            }
        }
    }}
