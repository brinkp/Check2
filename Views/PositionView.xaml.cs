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
        #region Enumerations

        public enum OperationStatusEnum
        {
            Playing,
            Editing,
            Selecting
        }

        #endregion

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

        #region IUndoableCommand

        internal enum CommandIndexEnum
        {
            ClearBoard
        }

        internal enum SimpleCommandIndexEnum
        {
            FlipBoard ,
            FlipPosition
        }

        internal interface IUndoableCommand
        {
            void Undo();
            void Redo();
        }

        private abstract class UndoableBase : IUndoableCommand
        {
            protected PositionView PositionView { get; set; }

            public abstract void Undo();
            public abstract void Redo();
        }

        private class UndoableMove : UndoableBase
        {
            public UndoableMove(PositionView positionView, Move move)
            {
                Debug.Assert(positionView != null);

                PositionView = positionView;
                Move         = move;
            }

            private Move Move { get; }

            public override void Undo()
            {
                PositionView.UndoLastMove(Move);
            }

            public override void Redo()
            {
                PositionView.RedoLastMove(Move);
            }

            public Move GetMove() => Move;
        }

        private class UndoablePositionCommand : UndoableBase
        {
            public UndoablePositionCommand(PositionView positionView, CommandIndexEnum commandIndex, Position position = null)
            {
                Debug.Assert(positionView != null);

                PositionView = positionView;
                CommandIndex = commandIndex;
                Position     = position    ;
            }

            private CommandIndexEnum CommandIndex { get; }
            private Position         Position     { get; }

            public override void Undo()
            {
                switch (CommandIndex)
                {
                    case CommandIndexEnum.ClearBoard:
                        Debug.Assert(Position != null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(CommandIndex), "Invalid switch value");
                }
            }

            public override void Redo()
            {
                switch (CommandIndex)
                {
                    case CommandIndexEnum.ClearBoard:
                        Debug.Assert(Position != null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(CommandIndex), "Invalid switch value");
                }
            }
        }

        private class UndoableSimpleCommand : UndoableBase
        {
            public UndoableSimpleCommand(PositionView positionView, SimpleCommandIndexEnum simpleCommandIndex)
            {
                Debug.Assert(positionView != null);

                PositionView       = positionView;
                SimpleCommandIndex = simpleCommandIndex;
            }

            private SimpleCommandIndexEnum SimpleCommandIndex { get; }

            public override void Undo()
            {
                switch (SimpleCommandIndex)
                {
                    case SimpleCommandIndexEnum.FlipBoard:
                        PositionView.FlipBoard();
                        break;
                    case SimpleCommandIndexEnum.FlipPosition:
                        PositionView.FlipTurn();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(SimpleCommandIndex), "Invalid switch value");
                }
            }

            public override void Redo()
            {
                switch (SimpleCommandIndex)
                {
                    case SimpleCommandIndexEnum.FlipBoard:
                        PositionView.FlipBoard();
                        break;
                    case SimpleCommandIndexEnum.FlipPosition:
                        PositionView.FlipTurn();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(SimpleCommandIndex), "Invalid switch value");
                }
            }
        }

        #endregion

        #region Constructors

        internal PositionView(PositionViewModel positionViewModel)
        {
            Debug.Assert(positionViewModel != null);

            InitializeComponent();

            PositionViewModel = positionViewModel;

            Grid = new Grid();

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

            if (! Busy)
            {
                switch (OperationStatus)
                {
                    case OperationStatusEnum.Playing:
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
                                // Do nothing
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
                        }
                        break;
                    case OperationStatusEnum.Editing  :
                    case OperationStatusEnum.Selecting:
                        fieldViewModel.FieldStatus = FieldStatusEnum.EditingMouseOver;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(OperationStatus), "Invalid switch value");
                }
            }
        }

        internal void OnFieldMouseLeave(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            if (! Busy)
            {
                switch (OperationStatus)
                {
                    case OperationStatusEnum.Playing:
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
                                // Do nothing
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
                        }
                        break;
                    case OperationStatusEnum.Editing:
                        fieldViewModel.FieldStatus = FieldStatusEnum.Editing;
                        break;
                    case OperationStatusEnum.Selecting:
                        Debug.Assert(SettingsEditingView != null);

                        fieldViewModel.FieldStatus = FieldStatusEnum.Editing;

                        switch (fieldViewModel.FieldContent)
                        {
                            case Position.FieldContentEnum.Empty    : if (SettingsEditingView.FieldContent == Position.FieldContentEnum.Empty    ) fieldViewModel.FieldStatus = FieldStatusEnum.EditingSelected; break;
                            case Position.FieldContentEnum.WhiteMan : if (SettingsEditingView.FieldContent == Position.FieldContentEnum.WhiteMan ) fieldViewModel.FieldStatus = FieldStatusEnum.EditingSelected; break;
                            case Position.FieldContentEnum.BlackMan : if (SettingsEditingView.FieldContent == Position.FieldContentEnum.BlackMan ) fieldViewModel.FieldStatus = FieldStatusEnum.EditingSelected; break;
                            case Position.FieldContentEnum.WhiteKing: if (SettingsEditingView.FieldContent == Position.FieldContentEnum.WhiteKing) fieldViewModel.FieldStatus = FieldStatusEnum.EditingSelected; break;
                            case Position.FieldContentEnum.BlackKing: if (SettingsEditingView.FieldContent == Position.FieldContentEnum.BlackKing) fieldViewModel.FieldStatus = FieldStatusEnum.EditingSelected; break;
                            case Position.FieldContentEnum.Taken    :
                            default:
                                throw new ArgumentOutOfRangeException(nameof(fieldViewModel.FieldContent), "Invalid switch value");
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(OperationStatus), "Invalid switch value");
                }
            }
        }

        internal async Task OnFieldMouseLeftButtonDown(int fieldIndex, FieldViewModel fieldViewModel)
        {
            Debug.Assert(fieldViewModel != null);

            if (! Busy)
            {
                switch (OperationStatus)
                {
                    case OperationStatusEnum.Playing:
                        switch (PositionStatus)
                        {
                            case PositionViewModel.PositionStatusEnum.Default:
                                await CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel);
                                break;
                            case PositionViewModel.PositionStatusEnum.FromGiven:
                                if (fieldIndex == FromFieldIndex)
                                {
                                    SetDefaultStatus();
                                }
                                else
                                {
                                    List<Move> possibleMoves = Position.PossibleMoves.Where(move => (move.FromField == FromFieldIndex) && (move.ToField == fieldIndex)).ToList();

                                    if (possibleMoves.Count > 0)
                                    {
                                        await HandleMoveExt(possibleMoves.First());
                                    }
                                    else
                                    {
                                        if (! await CheckIfFieldCanBeFrom(fieldIndex, fieldViewModel))
                                        {
                                            SetDefaultStatus();
                                        }
                                    }
                                }
                                break;
                            case PositionViewModel.PositionStatusEnum.TakeInProgress:
                                // Do nothing
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(PositionStatus), "Invalid switch value");
                        }
                        break;
                    case OperationStatusEnum.Editing:
                        Debug.Assert(SettingsEditingView != null);

                        fieldViewModel.FieldContent = SettingsEditingView.FieldContent;

                        fieldViewModel.Refresh();
                        break;
                    case OperationStatusEnum.Selecting:
                        SettingsEditingView.FieldContent = fieldViewModel.FieldContent;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(OperationStatus), "Invalid switch value");
                }
            }
        }

        internal async Task CheckForControlKeys(KeyEventArgs ea)
        {
            base.OnKeyDown(ea);

            if (CanPlay)
            {
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

                            UndoCommandStack.Clear();
                            RedoCommandStack.Clear();

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
                                while (RedoCommandStack.Count > 0)
                                {
                                    RedoLastCommand();

                                    await PauseIfRequired();
                                }
                            }
                            else
                            {
                                RedoLastCommand();
                            }

                            ea.Handled = false;
                            break;
                        case Key.Z:
                            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                            {
                                while (UndoCommandStack.Count > 0)
                                {
                                    UndoLastCommand();

                                    await PauseIfRequired();
                                }
                            }
                            else
                            {
                                UndoLastCommand();
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
        }

        #endregion

        #region Public properties

        internal SettingsEditingView  SettingsEditingView { get; set; }

        #endregion

        #region Private properties

        private  Grid                 Grid                { get;      }
        private  PositionViewModel    PositionViewModel   { get;      }
        private  FieldViewModel[]     FieldViewModels     { get;      }
        private  FieldView     []     FieldViews          { get;      }
        private  Border        []     Borders             { get;      }

        private  bool                 Busy                { get; set; }

        private  OperationStatusEnum _operationStatus = OperationStatusEnum.Playing;
        public   OperationStatusEnum  OperationStatus
        {
            get => _operationStatus;
            set
            {
                if (_operationStatus != value)
                {
                    _operationStatus  = value;

                    switch (value)
                    {
                        case OperationStatusEnum.Playing:
                            Position.GetMovesAndTakes();

                            SetDefaultStatus();
                            break;
                        case OperationStatusEnum.Editing  :
                        case OperationStatusEnum.Selecting:
                            ShowEditingMode();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(value), "Invalid switch value");
                    }
                }
            }
        }

        private bool CanPlay => (OperationStatus == OperationStatusEnum.Playing) && (! Busy);
        private bool CanEdit => (OperationStatus == OperationStatusEnum.Editing) && (! Busy);

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

        private Stack<IUndoableCommand> UndoCommandStack { get; } = new Stack<IUndoableCommand>();
        private Stack<IUndoableCommand> RedoCommandStack { get; } = new Stack<IUndoableCommand>();

        #endregion


        #region Commands

        public void ToggleAutomaticMoves              () { AutomaticMoves               = ! AutomaticMoves              ; }
        public void ToggleGiveVisualFeedback          () { GiveVisualFeedback           = ! GiveVisualFeedback          ; }
        public void ToggleDisplayIntermediatePositions() { DisplayIntermediatePositions = ! DisplayIntermediatePositions; }

        public void LoadPosition()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    Position.Load();

                    ResetStatus();

                    IndicatePossibleFromFields();

                    UndoCommandStack.Clear();
                    RedoCommandStack.Clear();
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public void SavePosition()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    Position.Save();
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public void MoveRandom()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    ResetStatus();

                    Move move = PositionViewModel.MoveRandom();

                    if  (move.IsValid)
                    {
                        SetDefaultStatus();

                        UndoCommandStack.Push(new UndoableMove(this, move));
                    }
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public async Task PlayRandom()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    ResetStatus();

                    await PositionViewModel.PlayRandom(PauseIfRequiredExt);

                    RefreshFields();
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public async Task SolveCombination()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    ResetStatus();

                    Move bestMove = await PositionViewModel.SolveCombination(PauseIfRequired);

                    if  (bestMove.IsValid)
                    {
                        Position.MoveInSitu(ref bestMove);

                        RefreshFields();

                        UndoCommandStack.Push(new UndoableMove(this, bestMove));
                    }

                    Position.GetMovesAndTakes();

                    SetDefaultStatus();
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public void ClearBoard()
        {
            Position.Clear();

            Position.GetMovesAndTakes();

            SetDefaultStatus();
        }

        public void FlipBoard()
        {
            // This could also be done with a RenderTransform, maybe combined with an animation

            if (CanPlay)
            {
                Busy = true;

                try
                {
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

                    UndoCommandStack.Push(new UndoableSimpleCommand(this, SimpleCommandIndexEnum.FlipBoard));
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public void FlipTurn()
        {
            if (CanPlay)
            {
                Busy = true;

                try
                {
                    Position.FlipTurn();

                    Position.GetMovesAndTakes();

                    SetDefaultStatus();
                }
                finally
                {
                    Busy = false;
                }
            }
        }

        public void ShowEditingMode()
        {
            foreach (FieldViewModel fieldViewModel in FieldViewModels)
            {
                fieldViewModel.StartEditingMode();
            }
        }

        #endregion

        #region Undo and Redo

        public void UndoLastCommand()
        {
            if (! Busy)
            {
              //Busy = true;

              //try
              //{
                    if (UndoCommandStack.Count > 0)
                    {
                        IUndoableCommand undoableCommand = UndoCommandStack.Pop();

                        undoableCommand.Undo();

                      //RedoCommandStack.Push(undoableCommand);
                    }
              //}
              //finally
              //{
              //    Busy = false;
              //}
            }
        }

        public void RedoLastCommand()
        {
            if (! Busy)
            {
              //Busy = true;

              //try
              //{
                    if (RedoCommandStack.Count > 0)
                    {
                        IUndoableCommand undoableCommand = UndoCommandStack.Pop();

                        undoableCommand.Redo();

                      //UndoCommandStack.Push(undoableCommand);
                    }
              //}
              //finally
              //{
              //    Busy = false;
              //}
            }
        }

        internal void UndoLastMove(Move move)
        {
            Position.UndoMoveInSitu(ref move);

            Position.GetMovesAndTakes();

            SetDefaultStatus();
        }

        internal void RedoLastMove(Move move)
        {
            Position.MoveInSitu(ref move);

            Position.GetMovesAndTakes();

            SetDefaultStatus();
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

            UndoCommandStack.Push(new UndoableMove(this, move));
        }

        internal void PushUndoStack(IUndoableCommand undoableCommand)
        {
            UndoCommandStack.Push(undoableCommand);
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

        private void HandleMove(Move move)
        {
            Position.MoveInSitu(ref move);

            Position.GetMovesAndTakes();

            UndoCommandStack.Push(new UndoableMove(this, move));

            SetDefaultStatus();
        }

        private async Task HandleMoveExt(Move move)
        {
            if ((RedoCommandStack.Count > 0) && ((RedoCommandStack.Peek() as UndoableMove)?.GetMove().Equals(ref move) == true))
            {
                RedoLastCommand();
            }
            else
            {
                HandleMove(move);
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

        private void SetDefaultStatus(FieldViewModel fieldViewModel = null)
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
                        case FieldStatusEnum.Editing:
                            result = Brushes.LightSteelBlue;
                            break;
                        case FieldStatusEnum.EditingMouseOver:
                            result = Brushes.Green;
                            break;
                        case FieldStatusEnum.EditingSelected:
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
