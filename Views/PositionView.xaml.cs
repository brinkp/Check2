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

                    FieldViewModel fieldViewModel =    FieldViewModels[fieldIndex - 1] = new FieldViewModel(      positionViewModel, fieldIndex);
                    FieldView      fieldView      =                                      new FieldView     (this,    fieldViewModel, fieldIndex);

                    border2.SetBinding(Border.BackgroundProperty, new Binding { Source = fieldViewModel, Path = new PropertyPath(nameof(FieldViewModel.FieldStatus)), Converter = fieldToBackgroundColorConverterFill, ConverterParameter = this } );

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

        #region Delegates

        private delegate void Callback(Stack<string> ownMoves, Stack<string> opponentsMoves);

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
                        List<Move> possibleMoves      = Position.PossibleMoves.Where(move => (move.FromField == FromFieldIndex) && (move.ToField == fieldIndex)).ToList();
                        int        possibleMovesCount = possibleMoves.Count;

                        bool moved = false;

                        for (int possibleMoveIndex = 0; possibleMoveIndex < possibleMovesCount; possibleMoveIndex += 1)
                        {
                            Move move = possibleMoves[possibleMoveIndex];

                            moved = true;

                            Position.MoveInSitu(ref move);

                            Position.GetMovesAndTakes();

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

                IndicatePossibleToFields  (fieldIndex);
                IndicatePossibleTakeFields(fieldIndex, FieldStatusEnum.CanBeTaken);

              //FromFieldViewModel = fieldViewModel;
                FromFieldIndex     = fieldIndex    ;

                PositionStatus = PositionViewModel.PositionStatusEnum.FromGiven;

                result = true;
            }

            return result;
        }

        internal async Task CheckForControlKeys(KeyEventArgs ea)
        {
            base.OnKeyDown(ea);

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (ea.Key)
                {
                    case Key.N:
                        Position.Initialize((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift);

                        ResetStatus();

                        IndicatePossibleFromFields();

                        ea.Handled = true;
                        break;
                    case Key.S:
                        Position.Save();

                        ea.Handled = true;
                        break;
                    case Key.L:
                        Position.Load();

                        ResetStatus();

                        IndicatePossibleFromFields();

                        ea.Handled = true;
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
                    case Key.M:
                        Move();

                        ea.Handled = true;
                        break;
                    case Key.P:
                        await Play();

                        ea.Handled = true;
                        break;
                    case Key.W:
                        await WhiteBeginsAndWins();

                        ea.Handled = true;
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

        private PositionViewModel PositionViewModel { get; }
        private FieldViewModel[]  FieldViewModels   { get; }

        private Position                             Position       => PositionViewModel?.Position;
        private PositionViewModel.PositionStatusEnum PositionStatus
        {
            get =>  PositionViewModel?.PositionStatus ?? PositionViewModel.PositionStatusEnum.Default;
            set
            {
                if (PositionViewModel != null) { PositionViewModel.PositionStatus = value; }
            }
        }

        private int            FromFieldIndex     { get; set; }
      //private FieldViewModel FromFieldViewModel { get; set; }

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

        #endregion

        #region Private methods

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
            fieldViewModel.FieldStatus = FieldStatusEnum.Default;

          //FromFieldViewModel = null;
            FromFieldIndex     =    0;

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

        private void Move()
        {
            ResetStatus();

            Random random = new Random();

            int count = Position.PossibleMoves.Count();

            if (count > 0)
            {
                int randomIndex = random.Next(count);

                Move move = Position.PossibleMoves.ElementAt(randomIndex);

                Position.MoveInSitu(ref move);

                RefreshFields();
            }
        }

        private async Task Play()
        {
            ResetStatus();

            Random random = new Random();

            int count = Position.PossibleMoves.Count();

            while (count > 0)
            {
                int randomIndex = random.Next(count);

                Move move = Position.PossibleMoves.ElementAt(randomIndex);

                Position.MoveInSitu(ref move);

                await PauseIfRequired();

                count = Position.PossibleMoves.Count();
            }

            RefreshFields();
        }

        private async Task WhiteBeginsAndWins()
        {
            ResetStatus();

            byte[] originalFields = Position.CopyFields();
            double originalValue  = Position.Evaluate  ();

            Stack<string> ownMoves       = new Stack<string>();
            Stack<string> opponentsMoves = new Stack<string>();

            Dictionary<byte[], RecursionResult> alreadyHandledPositions = new Dictionary<byte[], RecursionResult>(new ByteArrayComparer());

            Callback callback = (ownMovesSolution, opponentsMovesSolution) =>
            {
                MessageBox.Show(ownMoves.Peek());
            } ;

            var a = await BeginAndWin(0, ownMoves, opponentsMoves, alreadyHandledPositions, callback);

            Position.GetMovesAndTakes();
        }

        private enum RecursionResult
        {
            DoesNotLeadToForcedWin,
            DoesLeadToForcedWin
        }

        private async Task<RecursionResult> BeginAndWin(int depth, Stack<string> ownMoves, Stack<string> opponentsMoves, Dictionary<byte[], RecursionResult> alreadyHandledPositions, Callback callback)
        {
            depth += 1;

            RecursionResult           result = RecursionResult.DoesNotLeadToForcedWin;

            List<Move> possibleOwnMoves      = CopyListOfMoves(Position.PossibleMoves);
            int        possibleOwnMovesCount = possibleOwnMoves.Count;

            bool continueOwnMoves = true;

            for (int ownMoveIndex = 0; ownMoveIndex < possibleOwnMovesCount; ownMoveIndex += 1)
            {
                if (continueOwnMoves)
                {
                    Move ownMove = possibleOwnMoves[ownMoveIndex];

                    ownMoves.Push(ownMove.ToString());
#if CHECK_MOVES
                    byte[] fieldsBeforeMove = Position.CopyFields();

                    Position.    MoveInSitu(ref ownMove);
                    Position.UndoMoveInSitu(ref ownMove);

                    if (! Position.PositionEquals(fieldsBeforeMove)) throw new Exception();
#endif
                    Position.    MoveInSitu(ref ownMove);

                  //if (alreadyHandledPositions.ContainsKey(Position._fields))
                  //{
                  //    result = alreadyHandledPositions[Position._fields];
                  //}
                  //else
                  //{
                        await PauseIfRequired();

                        Position.GetTakes();

                        if (Position.NumberOfMoves > 0)
                        {
                            List<Move> possibleOpponentsMoves      = CopyListOfMoves(Position.PossibleMoves);
                            int        possibleOpponentsMovesCount = possibleOpponentsMoves.Count;

                            bool allMovesLeadToForcedWin = true;

                            for (int opponentIndex = 0; opponentIndex < possibleOpponentsMovesCount; opponentIndex += 1)
                            {
                                if (allMovesLeadToForcedWin)
                                {
                                    Move      opponentsMove = possibleOpponentsMoves[opponentIndex];
#if CHECK_MOVES
                                    byte[] fieldsBeforeTake = Position.CopyFields();

                                    Position.    MoveInSitu(ref opponentsMove);
                                    Position.UndoMoveInSitu(ref opponentsMove);

                                    if (! Position.PositionEquals(fieldsBeforeTake)) throw new Exception();
#endif
                                    Position.    MoveInSitu(ref opponentsMove);

                                    RecursionResult recursionResult;

                                  //if (alreadyHandledPositions.TryGetValue(Position._fields, out var recursionResult2))
                                  //{
                                  //    recursionResult = recursionResult2;
                                  //}
                                  //else
                                  //{
                                        await PauseIfRequired();

                                        opponentsMoves.Push(opponentsMove.ToString());

                                        Position.GetMovesAndTakes();

                                        recursionResult = await BeginAndWin(depth, ownMoves, opponentsMoves, alreadyHandledPositions, callback);

                                        opponentsMoves.Pop();

                                        if (recursionResult == RecursionResult.DoesNotLeadToForcedWin)
                                        {
                                            allMovesLeadToForcedWin = false;
                                        }

                                        await PauseIfRequired();

                                  //    alreadyHandledPositions.Add(Position._fields, recursionResult);
                                  //}

                                    Position.UndoMoveInSitu(ref opponentsMove);
#if CHECK_MOVES
                                    if (! Position.PositionEquals(fieldsBeforeTake)) throw new Exception();
#endif
                                }
                            }

                            if (allMovesLeadToForcedWin)
                            {
                                continueOwnMoves = false;

                                result = RecursionResult.DoesLeadToForcedWin;

                                if (depth == 1) callback(ownMoves, opponentsMoves);
                            }
                        }
                        else
                        {
                            Position.GetMovesAndTakes();

                            if (! Position.HasMoves)
                            {
                                continueOwnMoves = false;

                                result = RecursionResult.DoesLeadToForcedWin;

                                if (depth == 1) callback(ownMoves, opponentsMoves);
                            }
                        }

                  //    alreadyHandledPositions.Add(Position._fields, result);
                  //}

                    Position.UndoMoveInSitu(ref ownMove);
#if CHECK_MOVES
                    if (! Position.PositionEquals(fieldsBeforeMove)) throw new Exception();
#endif
                    await PauseIfRequired();

                    ownMoves.Pop();
                }
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

        private List<Move> CopyListOfMoves(IEnumerable<Move> moves)
        {
            List<Move> result = new List<Move>();

            if (moves != null)
            {
                result.AddRange(moves.Select(move => move.Copy()));
            }

            return result;
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
