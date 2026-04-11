using Check.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

#if DEBUG
using System.Diagnostics;
#pragma warning disable IDE0047
#endif

// ReSharper disable LocalizableElement
// ReSharper disable IdentifierTypo

namespace Check.Models
{
    internal class Position
    {
        // The implementation of class Position is completely determined by performance in both space and time.

        #region Constants

        private const int MaxNumberOfFields =  51;
        private const int MaxNumberOfMoves  = 100;
        private const int MaxNumberOfTakes  =  50;

        #endregion

        #region Enumerations

        public enum TurnEnum
        {
            White,
            Black
        }

        public enum FieldContentEnum
        {
            Empty    ,
            WhiteMan ,
            BlackMan ,
            WhiteKing,
            BlackKing,
            Taken
        }

        #endregion

        #region Fields

        private  TurnEnum _whiteOrBlacksTurn;

        private  int _numberOfMoves         ;
        private  int _numberOfTakesInMove   ;
        private  int _numberOfTakesInMoveMax;

        // ReSharper disable once InconsistentNaming
        internal readonly FieldContentEnum[] _fields = new FieldContentEnum[MaxNumberOfFields];
        private  readonly Move            [] _moves  = new Move            [MaxNumberOfMoves ];
        private  readonly int             [] _takes  = new int             [MaxNumberOfTakes ];

        private readonly int[] _upLefts    =
        {
             0,
             0,  0,  0,  0,  0,
             0,  1,  2,  3,  4,
             6,  7,  8,  9, 10,
             0, 11, 12, 13, 14,
            16, 17, 18, 19, 20,
             0, 21, 22, 23, 24,
            26, 27, 28, 29, 30,
             0, 31, 32, 33, 34,
            36, 37, 38, 39, 40,
             0, 41, 42, 43, 44
        } ;

        private readonly int[] _upRights   =
        {
             0,
             0,  0,  0,  0,  0,
             1,  2,  3,  4,  5,
             7,  8,  9, 10,  0,
            11, 12, 13, 14, 15,
            17, 18, 19, 20,  0,
            21, 22, 23, 24, 25,
            27, 28, 29, 30,  0,
            31, 32, 33, 34, 35,
            37, 38, 39, 40,  0,
            41, 42, 43, 44, 45
        } ;

        private readonly int[] _downLefts  =
        {
             0,
             6,  7,  8,  9, 10,
             0, 11, 12, 13, 14,
            16, 17, 18, 19, 20,
             0, 21, 22, 23, 24,
            26, 27, 28, 29, 30,
             0, 31, 32, 33, 34,
            36, 37, 38, 39, 40,
             0, 41, 42, 43, 44,
            46, 47, 48, 49, 50,
             0,  0,  0,  0,  0
        } ;

        private readonly int[] _downRights =
        {
             0,
             7,  8,  9, 10,  0,
            11, 12, 13, 14, 15,
            17, 18, 19, 20,  0,
            21, 22, 23, 24, 25,
            27, 28, 29, 30,  0,
            31, 32, 33, 34, 35,
            37, 38, 39, 40,  0,
            41, 42, 43, 44, 45,
            47, 48, 49, 50,  0,
             0,  0,  0,  0,  0
        } ;

        #endregion

        #region Constructors

        public Position(bool startPosition = true)
        {
            if (startPosition)
            {
                for (int index =  1; index <= 20; index += 1) { _fields[index] = FieldContentEnum.BlackMan; }
                for (int index = 21; index <= 30; index += 1) { _fields[index] = FieldContentEnum.Empty   ; }
                for (int index = 31; index <= 50; index += 1) { _fields[index] = FieldContentEnum.WhiteMan; }
            }
            else
            {
              //for (int index =  1; index <= 50; index += 1) { _fields[index] = FieldContentEnum.Empty;    }

                _fields[ 1] = FieldContentEnum.BlackMan; _fields[ 2] = FieldContentEnum.Empty   ; _fields[ 3] = FieldContentEnum.Empty   ; _fields[ 4] = FieldContentEnum.Empty   ; _fields[ 5] = FieldContentEnum.Empty   ;
                _fields[ 6] = FieldContentEnum.Empty   ; _fields[ 7] = FieldContentEnum.Empty   ; _fields[ 8] = FieldContentEnum.BlackMan; _fields[ 9] = FieldContentEnum.BlackMan; _fields[10] = FieldContentEnum.BlackMan;
                _fields[11] = FieldContentEnum.WhiteMan; _fields[12] = FieldContentEnum.Empty   ; _fields[13] = FieldContentEnum.BlackMan; _fields[14] = FieldContentEnum.Empty   ; _fields[15] = FieldContentEnum.Empty   ;
                _fields[16] = FieldContentEnum.Empty   ; _fields[17] = FieldContentEnum.WhiteMan; _fields[18] = FieldContentEnum.Empty   ; _fields[19] = FieldContentEnum.BlackMan; _fields[20] = FieldContentEnum.BlackMan;
                _fields[21] = FieldContentEnum.WhiteMan; _fields[22] = FieldContentEnum.WhiteMan; _fields[23] = FieldContentEnum.BlackMan; _fields[24] = FieldContentEnum.BlackMan; _fields[25] = FieldContentEnum.Empty   ;
                _fields[26] = FieldContentEnum.Empty   ; _fields[27] = FieldContentEnum.WhiteMan; _fields[28] = FieldContentEnum.BlackMan; _fields[29] = FieldContentEnum.BlackMan; _fields[30] = FieldContentEnum.WhiteMan;
                _fields[31] = FieldContentEnum.Empty   ; _fields[32] = FieldContentEnum.Empty   ; _fields[33] = FieldContentEnum.BlackMan; _fields[34] = FieldContentEnum.WhiteMan; _fields[35] = FieldContentEnum.BlackMan;
                _fields[36] = FieldContentEnum.Empty   ; _fields[37] = FieldContentEnum.WhiteMan; _fields[38] = FieldContentEnum.WhiteMan; _fields[39] = FieldContentEnum.WhiteMan; _fields[40] = FieldContentEnum.WhiteMan;
                _fields[41] = FieldContentEnum.Empty   ; _fields[42] = FieldContentEnum.WhiteMan; _fields[43] = FieldContentEnum.WhiteMan; _fields[44] = FieldContentEnum.WhiteMan; _fields[45] = FieldContentEnum.BlackMan;
                _fields[46] = FieldContentEnum.Empty   ; _fields[47] = FieldContentEnum.Empty   ; _fields[48] = FieldContentEnum.Empty   ; _fields[49] = FieldContentEnum.Empty   ; _fields[50] = FieldContentEnum.Empty   ;
            }

            WhiteOrBlacksTurn = TurnEnum.White;
        }

        #endregion

        #region Public properties

        public IEnumerable<Move> PossibleMoves                         => _moves.Take(_numberOfMoves).Where(move => move.IsValid);
        public IEnumerable<Move> PossibleMovesFrom(int fromFieldIndex) => PossibleMoves.Where(move => move.FromField == fromFieldIndex);

        public TurnEnum WhiteOrBlacksTurn
        {
            get => _whiteOrBlacksTurn;
            set
            {
               _whiteOrBlacksTurn = value;

                GetMovesAndTakes();
            }
        }

        #endregion

        #region Get moves and takes

        public void GetMovesAndTakes()
        {
            DateTime now = DateTime.Now;

            GetTakes();

            if (_numberOfMoves == 0)
            {
                GetMoves();
            }

#if DEBUG
            Debug.WriteLine((DateTime.Now - now).Milliseconds + " mSec");
#else
            MessageBox.Show((DateTime.Now - now).Milliseconds + " mSec");
#endif
        }

        private void GetTakes()
        {
           _numberOfMoves          = 0;
           _numberOfTakesInMove    = 0;
           _numberOfTakesInMoveMax = 0;

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                for (int fromFieldIndex = 1; fromFieldIndex <= 50; fromFieldIndex += 1)
                {
                    switch (_fields[fromFieldIndex])
                    {
                        case FieldContentEnum.WhiteMan:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakesForWhiteMan (fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.WhiteMan;
                            break;
                        case FieldContentEnum.WhiteKing:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakesForKing(FieldContentEnum.BlackMan, FieldContentEnum.BlackKing, fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.WhiteKing;
                            break;
                    }
                }
            }
            else
            {
                for (int fromFieldIndex = 1; fromFieldIndex <= 50; fromFieldIndex += 1)
                {
                    switch (_fields[fromFieldIndex])
                    {
                        case FieldContentEnum.BlackMan:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakesForBlackMan (fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.BlackMan;
                            break;
                        case FieldContentEnum.BlackKing:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakesForKing(FieldContentEnum.WhiteMan, FieldContentEnum.WhiteKing, fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.BlackKing;
                            break;
                    }
                }
            }

            if (_numberOfMoves > 1)
            {
                for (int moveIndex1 = 0; moveIndex1 < _numberOfMoves - 1; moveIndex1 += 1)
                {
                    if (_moves[moveIndex1].IsValid)
                    {
                        for (int moveIndex2 = moveIndex1 + 1; moveIndex2 < _numberOfMoves; moveIndex2 += 1)
                        {
                            if (_moves[moveIndex2].IsValid)
                            {
                                if (_moves[moveIndex1].Equals(_moves[moveIndex2]))
                                {
                                    _moves[moveIndex2].Invalidate();
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Get takes for men

        private void GetTakesForWhiteMan(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1:                                                                                                                                                                                                                            GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  12); break;
                case  2:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  13); break;
                case  3:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  14); break;
                case  4:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  15); break;
                case  5:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14);                                                                           break;
                case  6:                                                                                                                                                                                                                            GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  17); break;
                case  7:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 16); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  18); break;
                case  8:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  19); break;
                case  9:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  20); break;
                case 10:                                                                                                                                                   GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19);                                                                           break;
                case 11:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  2);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17,  22); break;
                case 12: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  1); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  3); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18,  23); break;
                case 13: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  2); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  4); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19,  24); break;
                case 14: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  3); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  5); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20,  25); break;
                case 15: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  4);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24);                                                                           break;
                case 16:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21,  27); break;
                case 17: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  6); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 26); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22,  28); break;
                case 18: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  7); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23,  29); break;
                case 19: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  8); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24,  30); break;
                case 20: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  9);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29);                                                                           break;
                case 21:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27,  32); break;
                case 22: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 11); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28,  33); break;
                case 23: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 12); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29,  34); break;
                case 24: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 13); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 15); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30,  35); break;
                case 25: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 14);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34);                                                                           break;
                case 26:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31,  37); break;
                case 27: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 16); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 36); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32,  38); break;
                case 28: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 17); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33,  39); break;
                case 29: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 18); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34,  40); break;
                case 30: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 19);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39);                                                                           break;
                case 31:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37,  42); break;
                case 32: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 21); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38,  43); break;
                case 33: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 22); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39,  44); break;
                case 34: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 23); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 25); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40,  45); break;
                case 35: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 24);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44);                                                                           break;
                case 36:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41,  47); break;
                case 37: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 26); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 46); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42,  48); break;
                case 38: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 27); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 47); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43,  49); break;
                case 39: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 28); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 48); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44,  50); break;
                case 40: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 29);                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 49);                                                                           break;
                case 41:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32);                                                                                                                                                    break;
                case 42: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 31); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33);                                                                                                                                                    break;
                case 43: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 32); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34);                                                                                                                                                    break;
                case 44: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 33); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 35);                                                                                                                                                    break;
                case 45: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 34);                                                                                                                                                                                                                             break;
                case 46:                                                                          GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37);                                                                                                                                                    break;
                case 47: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 36); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38);                                                                                                                                                    break;
                case 48: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 37); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39);                                                                                                                                                    break;
                case 49: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 38); GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40);                                                                                                                                                    break;
                case 50: GetTakesForWhiteMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 39);                                                                                                                                                                                                                             break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }
        }

        private void GetTakesForWhiteMan(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexTake, int fieldIndexTo)
        {
            if (_fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTake])
                {
                    case FieldContentEnum.BlackMan:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForWhiteMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = FieldContentEnum.BlackMan;
                        break;
                    case FieldContentEnum.BlackKing:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForWhiteMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = FieldContentEnum.BlackKing;
                        break;
                    default:
                        if (_numberOfTakesInMove > 0)
                        {
                            if (hadOne == false)
                            {
                                hadOne  = true ;

                                if (_numberOfTakesInMoveMax  < _numberOfTakesInMove)
                                {
                                    _numberOfTakesInMoveMax  = _numberOfTakesInMove;
                                    _numberOfMoves           =                    0;
                                }

                                if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                                {
                                    _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd, _numberOfTakesInMove, _takes); //, _vias);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void GetTakesForBlackMan(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 12);                                                                                                                                                                                                                             break;
                case  2: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 13); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11);                                                                                                                                                    break;
                case  3: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 14); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12);                                                                                                                                                    break;
                case  4: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 15); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13);                                                                                                                                                    break;
                case  5:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14);                                                                                                                                                    break;
                case  6: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 17);                                                                                                                                                                                                                             break;
                case  7: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 18); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 16);                                                                                                                                                    break;
                case  8: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 19); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17);                                                                                                                                                    break;
                case  9: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 20); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18);                                                                                                                                                    break;
                case 10:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19);                                                                                                                                                    break;
                case 11: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 22);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  2);                                                                           break;
                case 12: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 23); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  3); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,   1); break;
                case 13: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 24); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  4); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,   2); break;
                case 14: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 25); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  5); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,   3); break;
                case 15:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,   4); break;
                case 16: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 27);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7);                                                                           break;
                case 17: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 28); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 26); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,   6); break;
                case 18: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 29); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,   7); break;
                case 19: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 30); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,   8); break;
                case 20:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,   9); break;
                case 21: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 32);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12);                                                                           break;
                case 22: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 33); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 17,  11); break;
                case 23: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 34); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 18,  12); break;
                case 24: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 35); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 15); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 19,  13); break;
                case 25:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 20,  14); break;
                case 26: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 37);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17);                                                                           break;
                case 27: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 38); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 36); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 21,  16); break;
                case 28: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 39); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 22,  17); break;
                case 29: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 40); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 23,  18); break;
                case 30:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 24,  19); break;
                case 31: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 42);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22);                                                                           break;
                case 32: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 43); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 27,  21); break;
                case 33: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 44); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 28,  22); break;
                case 34: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 45); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 25); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 29,  23); break;
                case 35:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 30,  24); break;
                case 36: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 47);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27);                                                                           break;
                case 37: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 48); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 46); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 31,  26); break;
                case 38: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 49); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 47); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 32,  27); break;
                case 39: GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 50); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 48); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 33,  28); break;
                case 40:                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 49);                                                                          GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 34,  29); break;
                case 41:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32);                                                                           break;
                case 42:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 37,  31); break;
                case 43:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 38,  32); break;
                case 44:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 35); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 39,  33); break;
                case 45:                                                                                                                                                                                                                            GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 40,  34); break;
                case 46:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37);                                                                           break;
                case 47:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 41,  36); break;
                case 48:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 42,  37); break;
                case 49:                                                                                                                                                   GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40); GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 43,  38); break;
                case 50:                                                                                                                                                                                                                            GetTakesForBlackMan(ref hadOne, fieldIndexStart, fieldIndexEnd, 44,  39); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }
        }

        private void GetTakesForBlackMan(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexTake, int fieldIndexTo)
        {
            if (_fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTake])
                {
                    case FieldContentEnum.WhiteMan:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForBlackMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = FieldContentEnum.WhiteMan;
                        break;
                    case FieldContentEnum.WhiteKing:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForBlackMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = FieldContentEnum.WhiteKing;
                        break;
                    default:
                        if (_numberOfTakesInMove > 0)
                        {
                            if (hadOne == false)
                            {
                                hadOne  = true ;

                                if (_numberOfTakesInMoveMax  < _numberOfTakesInMove)
                                {
                                    _numberOfTakesInMoveMax  = _numberOfTakesInMove;
                                    _numberOfMoves           =                    0;
                                }

                                if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                                {
                                    _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd, _numberOfTakesInMove, _takes);
                                }
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        #region Get takes for king

        private bool GetTakesForKing(FieldContentEnum manToTake, FieldContentEnum kingToTake, int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            // ReSharper disable once ReplaceWithSingleAssignment.False
            bool result = false;

            // ReSharper disable once ConvertIfToOrExpression
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexEnd, fieldIndexFrom,   _upLefts )) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexEnd, fieldIndexFrom,   _upRights)) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexEnd, fieldIndexFrom, _downLefts )) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexEnd, fieldIndexFrom, _downRights)) result = true;

            return result;
        }

        private bool GetTakesForKing(FieldContentEnum manToTake, FieldContentEnum kingToTake, int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom, int[] tryFields)
        {
            bool result        = false         ;
            bool tryNext       = true          ;
            int  tryFieldIndex = fieldIndexFrom;

            while (tryNext && ((tryFieldIndex =  tryFields[tryFieldIndex]) != 0))
            {
                FieldContentEnum fieldContent = _fields   [tryFieldIndex];

                if ((fieldContent == manToTake) || (fieldContent == kingToTake))
                {
                    int fieldIndexTake = tryFieldIndex;
                    int tryFieldIndex2 = tryFieldIndex;

                    while ((tryFieldIndex2 = tryFields[tryFieldIndex2]) != 0)
                    {
                        if (_fields[tryFieldIndex2] == FieldContentEnum.Empty)
                        {
                            result = true;
                            fieldIndexEnd = tryFieldIndex2;

                            _fields[fieldIndexTake] = FieldContentEnum.Taken;

                            _takes[_numberOfTakesInMove] = fieldIndexTake;

                            _numberOfTakesInMove += 1;

                            if (!GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexEnd, tryFieldIndex2))
                            {
                                if (_numberOfTakesInMove > 0)
                                {
                                    if (_numberOfTakesInMoveMax < _numberOfTakesInMove)
                                    {
                                        _numberOfTakesInMoveMax = _numberOfTakesInMove;
                                        _numberOfMoves = 0;
                                    }

                                    if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                                    {
                                        _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd, _numberOfTakesInMove, _takes); //, _vias);
                                    }
                                }
                            }

                            _numberOfTakesInMove -= 1;

                            _fields[fieldIndexTake] = fieldContent;
                        }
                        else
                        {
                            tryNext = false;
                            break;
                        }
                    }
                }
                else if (fieldContent != FieldContentEnum.Empty)
                {
                    tryNext = false;
                }
            }

            return result;
        }

        #endregion

        #region Get moves

        private void GetMoves()
        {
           _numberOfMoves = 0; // Defensive

            GetMovesForMan ();
            GetMovesForKing((WhiteOrBlacksTurn == TurnEnum.White) ? FieldContentEnum.WhiteKing : FieldContentEnum.BlackKing);
        }

        private void GetMovesForMan()
        {
            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                if (_fields[ 6] == FieldContentEnum.WhiteMan) {                                                                                         if (_fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6,  1); }
                if (_fields[ 7] == FieldContentEnum.WhiteMan) { if (_fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  1); if (_fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  2); }
                if (_fields[ 8] == FieldContentEnum.WhiteMan) { if (_fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  2); if (_fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  3); }
                if (_fields[ 9] == FieldContentEnum.WhiteMan) { if (_fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  3); if (_fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  4); }
                if (_fields[10] == FieldContentEnum.WhiteMan) { if (_fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  4); if (_fields[ 5] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  5); }
                if (_fields[11] == FieldContentEnum.WhiteMan) { if (_fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  6); if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  7); }
                if (_fields[12] == FieldContentEnum.WhiteMan) { if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  7); if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  8); }
                if (_fields[13] == FieldContentEnum.WhiteMan) { if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  8); if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  9); }
                if (_fields[14] == FieldContentEnum.WhiteMan) { if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14,  9); if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 10); }
                if (_fields[15] == FieldContentEnum.WhiteMan) { if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 10);                                                                                         }
                if (_fields[16] == FieldContentEnum.WhiteMan) {                                                                                         if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 11); }
                if (_fields[17] == FieldContentEnum.WhiteMan) { if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 11); if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 12); }
                if (_fields[18] == FieldContentEnum.WhiteMan) { if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 12); if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 13); }
                if (_fields[19] == FieldContentEnum.WhiteMan) { if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 13); if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 14); }
                if (_fields[20] == FieldContentEnum.WhiteMan) { if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 14); if (_fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 15); }
                if (_fields[21] == FieldContentEnum.WhiteMan) { if (_fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 16); if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 17); }
                if (_fields[22] == FieldContentEnum.WhiteMan) { if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 17); if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 18); }
                if (_fields[23] == FieldContentEnum.WhiteMan) { if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 18); if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 19); }
                if (_fields[24] == FieldContentEnum.WhiteMan) { if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 19); if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 20); }
                if (_fields[25] == FieldContentEnum.WhiteMan) { if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 20);                                                                                         }
                if (_fields[26] == FieldContentEnum.WhiteMan) {                                                                                         if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 21); }
                if (_fields[27] == FieldContentEnum.WhiteMan) { if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 21); if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 22); }
                if (_fields[28] == FieldContentEnum.WhiteMan) { if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 22); if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 23); }
                if (_fields[29] == FieldContentEnum.WhiteMan) { if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 23); if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 24); }
                if (_fields[30] == FieldContentEnum.WhiteMan) { if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 24); if (_fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 25); }
                if (_fields[31] == FieldContentEnum.WhiteMan) { if (_fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 26); if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 27); }
                if (_fields[32] == FieldContentEnum.WhiteMan) { if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 27); if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 28); }
                if (_fields[33] == FieldContentEnum.WhiteMan) { if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 28); if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 29); }
                if (_fields[34] == FieldContentEnum.WhiteMan) { if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 29); if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 30); }
                if (_fields[35] == FieldContentEnum.WhiteMan) { if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 30);                                                                                         }
                if (_fields[36] == FieldContentEnum.WhiteMan) {                                                                                         if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 31); }
                if (_fields[37] == FieldContentEnum.WhiteMan) { if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 31); if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 32); }
                if (_fields[38] == FieldContentEnum.WhiteMan) { if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 32); if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 33); }
                if (_fields[39] == FieldContentEnum.WhiteMan) { if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 33); if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 34); }
                if (_fields[40] == FieldContentEnum.WhiteMan) { if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 34); if (_fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 35); }
                if (_fields[41] == FieldContentEnum.WhiteMan) { if (_fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 36); if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 37); }
                if (_fields[42] == FieldContentEnum.WhiteMan) { if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 37); if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 38); }
                if (_fields[43] == FieldContentEnum.WhiteMan) { if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 38); if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 39); }
                if (_fields[44] == FieldContentEnum.WhiteMan) { if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 39); if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 40); }
                if (_fields[45] == FieldContentEnum.WhiteMan) { if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 40);                                                                                         }
                if (_fields[46] == FieldContentEnum.WhiteMan) {                                                                                         if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(46, 41); }
                if (_fields[47] == FieldContentEnum.WhiteMan) { if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 41); if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 42); }
                if (_fields[48] == FieldContentEnum.WhiteMan) { if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 42); if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 43); }
                if (_fields[49] == FieldContentEnum.WhiteMan) { if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 43); if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 44); }
                if (_fields[50] == FieldContentEnum.WhiteMan) { if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 44); if (_fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 45); }
            }
            else
            {
                if (_fields[ 1] == FieldContentEnum.BlackMan) { if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  7); if (_fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  6); }
                if (_fields[ 2] == FieldContentEnum.BlackMan) { if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  8); if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  7); }
                if (_fields[ 3] == FieldContentEnum.BlackMan) { if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  9); if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  8); }
                if (_fields[ 4] == FieldContentEnum.BlackMan) { if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4, 10); if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4,  9); }
                if (_fields[ 5] == FieldContentEnum.BlackMan) {                                                                                         if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 5, 10); }
                if (_fields[ 6] == FieldContentEnum.BlackMan) { if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6, 11);                                                                                         }
                if (_fields[ 7] == FieldContentEnum.BlackMan) { if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 12); if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 11); }
                if (_fields[ 8] == FieldContentEnum.BlackMan) { if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 13); if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 12); }
                if (_fields[ 9] == FieldContentEnum.BlackMan) { if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 14); if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 13); }
                if (_fields[10] == FieldContentEnum.BlackMan) { if (_fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 15); if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 14); }
                if (_fields[11] == FieldContentEnum.BlackMan) { if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 17); if (_fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 16); }
                if (_fields[12] == FieldContentEnum.BlackMan) { if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 18); if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 17); }
                if (_fields[13] == FieldContentEnum.BlackMan) { if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 19); if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 18); }
                if (_fields[14] == FieldContentEnum.BlackMan) { if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 20); if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 19); }
                if (_fields[15] == FieldContentEnum.BlackMan) {                                                                                         if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 20); }
                if (_fields[16] == FieldContentEnum.BlackMan) { if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 21);                                                                                         }
                if (_fields[17] == FieldContentEnum.BlackMan) { if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 22); if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 21); }
                if (_fields[18] == FieldContentEnum.BlackMan) { if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 23); if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 22); }
                if (_fields[19] == FieldContentEnum.BlackMan) { if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 24); if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 23); }
                if (_fields[20] == FieldContentEnum.BlackMan) { if (_fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 25); if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 24); }
                if (_fields[21] == FieldContentEnum.BlackMan) { if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 27); if (_fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 26); }
                if (_fields[22] == FieldContentEnum.BlackMan) { if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 28); if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 27); }
                if (_fields[23] == FieldContentEnum.BlackMan) { if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 29); if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 28); }
                if (_fields[24] == FieldContentEnum.BlackMan) { if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 30); if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 29); }
                if (_fields[25] == FieldContentEnum.BlackMan) {                                                                                         if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 30); }
                if (_fields[26] == FieldContentEnum.BlackMan) { if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 31);                                                                                         }
                if (_fields[27] == FieldContentEnum.BlackMan) { if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 32); if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 31); }
                if (_fields[28] == FieldContentEnum.BlackMan) { if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 33); if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 32); }
                if (_fields[29] == FieldContentEnum.BlackMan) { if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 34); if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 33); }
                if (_fields[30] == FieldContentEnum.BlackMan) { if (_fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 35); if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 34); }
                if (_fields[31] == FieldContentEnum.BlackMan) { if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 37); if (_fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 36); }
                if (_fields[32] == FieldContentEnum.BlackMan) { if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 38); if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 37); }
                if (_fields[33] == FieldContentEnum.BlackMan) { if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 39); if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 38); }
                if (_fields[34] == FieldContentEnum.BlackMan) { if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 40); if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 39); }
                if (_fields[35] == FieldContentEnum.BlackMan) {                                                                                         if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 40); }
                if (_fields[36] == FieldContentEnum.BlackMan) { if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 41);                                                                                         }
                if (_fields[37] == FieldContentEnum.BlackMan) { if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 42); if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 41); }
                if (_fields[38] == FieldContentEnum.BlackMan) { if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 43); if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 42); }
                if (_fields[39] == FieldContentEnum.BlackMan) { if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 44); if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 43); }
                if (_fields[40] == FieldContentEnum.BlackMan) { if (_fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 45); if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 44); }
                if (_fields[41] == FieldContentEnum.BlackMan) { if (_fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 47); if (_fields[46] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 46); }
                if (_fields[42] == FieldContentEnum.BlackMan) { if (_fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 48); if (_fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 47); }
                if (_fields[43] == FieldContentEnum.BlackMan) { if (_fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 49); if (_fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 48); }
                if (_fields[44] == FieldContentEnum.BlackMan) { if (_fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 50); if (_fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 49); }
                if (_fields[45] == FieldContentEnum.BlackMan) {                                                                                         if (_fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 50); }
            }
        }

        private void GetMovesForKing(FieldContentEnum fieldContentKingToProcess)
        {
            for (int fieldIndexFrom = 1; fieldIndexFrom <= 50; fieldIndexFrom += 1)
            {
                if (_fields[fieldIndexFrom] == fieldContentKingToProcess)
                {
                    int tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = _upLefts[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == FieldContentEnum.Empty)
                        {
                           _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = _upRights[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == FieldContentEnum.Empty)
                        {
                            _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = _downLefts[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == FieldContentEnum.Empty)
                        {
                            _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = _downRights[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == FieldContentEnum.Empty)
                        {
                            _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Public methods

        public void Move(Move move)
        {
            int toFieldIndex = move.ToField;

           _fields[toFieldIndex  ] = _fields[move.FromField];
           _fields[move.FromField] = FieldContentEnum.Empty;

            if (move.TakeFields?.Count > 0)
            {
                foreach (int takeIndex in move.TakeFields)
                {
                   _fields[takeIndex] = FieldContentEnum.Empty;
                }
            }

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                switch (toFieldIndex) { case  1: case  2: case  3: case  4: case  5: _fields[toFieldIndex] = FieldContentEnum.WhiteKing; break; }

                WhiteOrBlacksTurn = TurnEnum.Black;
            }
            else
            {
                switch (toFieldIndex) { case 46: case 47: case 48: case 49: case 50: _fields[toFieldIndex] = FieldContentEnum.BlackKing; break; }

                WhiteOrBlacksTurn = TurnEnum.White;
            }
        }

        #endregion
    }
}
