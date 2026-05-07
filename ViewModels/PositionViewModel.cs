#if DEBUG
#define CHECK_MOVES
#endif

using System;
using Check.Models;
using Check.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Check.ViewModels
{
    internal class PositionViewModel : BaseViewModel
    {
        #region Enumerations

        public enum PositionStatusEnum
        {
            Default,
            FromGiven,
            TakeInProgress
        }

        #endregion

        #region Delegates

        private delegate void Callback(Stack<Move> ownMoves, Stack<Move> opponentsMoves);

        #endregion

        #region Constructors

        public PositionViewModel(Position position)
        {
            if (position == null) System.Diagnostics.Debugger.Break();

           Position = position;
        }

        #endregion

        #region Public properties

        public Position Position { get; }

        public PositionStatusEnum PositionStatus { get; set; } = PositionStatusEnum.Default;

        #endregion

        #region Public methods

        public Move MoveRandom()
        {
            Move result = new Move();

            int count = Position.PossibleMoves.Count();

            if (count > 0)
            {
                int randomIndex = Random.Next(count);

                result = Position.PossibleMoves.ElementAt(randomIndex);

                Position.MoveInSitu(ref result);
                Position.GetMovesAndTakes();
            }

            return result;
        }

        public async Task PlayRandom(Func<Move, Task> updateUiIfRequired)
        {
            int count = Position.PossibleMoves.Count();

            while (count > 0)
            {
                int randomIndex = Random.Next(count);

                Move move = Position.PossibleMoves.ElementAt(randomIndex);

                Position.MoveInSitu(ref move);
                Position.GetMovesAndTakes();

                if (updateUiIfRequired != null) await updateUiIfRequired(move);

                count = Position.PossibleMoves.Count();
            }
        }

        public async Task<Move> SolveCombinationForWhite(Func<Task> updateUiIfRequired)
        {
            Move result = new Move();

            Stack<Move> ownMoves       = new Stack<Move>();
            Stack<Move> opponentsMoves = new Stack<Move>();

            Dictionary<byte[], RecursionResult> alreadyHandledPositions = new Dictionary<byte[], RecursionResult>(new ByteArrayComparer());

            await SolveCombination(0, ownMoves, opponentsMoves, alreadyHandledPositions, (ownMovesSolution, opponentsMovesSolution) =>
            {
                if (ownMoves.Count > 0)
                {
                    result = ownMoves.Peek();
                }
            } , updateUiIfRequired);

            return result;
        }

        #endregion

        #region Private properties

        private Random Random { get; } = new Random();

        #endregion

        #region Private methods

        private List<Move> CopyListOfMoves(IEnumerable<Move> moves)
        {
            List<Move> result = new List<Move>();

            if (moves != null)
            {
                result.AddRange(moves.Select(move => move.Copy()));
            }

            return result;
        }

        private enum RecursionResult
        {
            DoesNotLeadToForcedWin,
            DoesLeadToForcedWin
        }

        private async Task<RecursionResult> SolveCombination(int depth, Stack<Move> ownMoves, Stack<Move> opponentsMoves, Dictionary<byte[], RecursionResult> alreadyHandledPositions, Callback callback, Func<Task> updateUiIfRequired)
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

                    ownMoves.Push(ownMove);
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
                        if (updateUiIfRequired != null) await updateUiIfRequired();

                        Position.GetMovesAndTakes();

                        bool forcedMove = false;

                        switch (Position.NumberOfMoves)
                        {
                            case 0:
                                continueOwnMoves = false;

                                result = RecursionResult.DoesLeadToForcedWin;

                                if (depth == 1) callback(ownMoves, opponentsMoves);
                                break;
                            case 1:
                                forcedMove = true;
                                break;
                            default:
                                forcedMove = Position.HasTakes;
                                break;
                        }

                        if (forcedMove)
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
                                    opponentsMoves.Push(opponentsMove);

                                    Position.    MoveInSitu(ref opponentsMove);

                                    RecursionResult recursionResult;

                                    if (alreadyHandledPositions.TryGetValue(Position._fields, out var recursionResult2))
                                    {
                                        recursionResult = recursionResult2;
                                    }
                                    else
                                    {
                                        if (updateUiIfRequired != null) await updateUiIfRequired();

                                        Position.GetMovesAndTakes();

                                        recursionResult = await SolveCombination(depth, ownMoves, opponentsMoves, alreadyHandledPositions, callback, updateUiIfRequired);

                                        if (updateUiIfRequired != null) await updateUiIfRequired();

                                        alreadyHandledPositions.Add(Position._fields, recursionResult);
                                    }

                                    if (recursionResult == RecursionResult.DoesNotLeadToForcedWin)
                                    {
                                        allMovesLeadToForcedWin = false;
                                    }

                                    Position.UndoMoveInSitu(ref opponentsMove);
#if CHECK_MOVES
                                    if (! Position.PositionEquals(fieldsBeforeTake)) throw new Exception();
#endif
                                    opponentsMoves.Pop();
                                }
                            }

                            if (allMovesLeadToForcedWin)
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
                    if (updateUiIfRequired != null) await updateUiIfRequired();

                    ownMoves.Pop();
                }
            }

            return result;
        }

        #endregion
    }
}
