using Check.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

#if DEBUG
#pragma warning disable IDE0047
#endif

// ReSharper disable BadControlBracesIndent
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

        #region Static fields

        private static readonly int [] UpLefts =
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

        private static readonly int[] UpRights =
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

        private static readonly int[] DownLefts =
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

        private static readonly int[] DownRights =
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

        #region Fields

        private  int _numberOfMoves         ;
        private  int _numberOfTakesInMove   ;
        private  int _numberOfTakesInMoveMax;

        [JsonInclude]
        // ReSharper disable once InconsistentNaming
        internal readonly byte[] _fields = new byte[MaxNumberOfFields];
        private  readonly Move[] _moves  = new Move[MaxNumberOfMoves ];
        private  readonly int [] _takes  = new int [MaxNumberOfTakes ];

        #endregion

        #region Constructors and initialization

        public Position(bool startPosition = true)
        {
            Initialize(startPosition);
        }

        public void Initialize(bool startPosition)
        {
            if (startPosition)
            {
                for (int index =  1; index <= 20; index += 1) { _fields[index] = (byte) FieldContentEnum.BlackMan; }
                for (int index = 21; index <= 30; index += 1) { _fields[index] = (byte) FieldContentEnum.Empty   ; }
                for (int index = 31; index <= 50; index += 1) { _fields[index] = (byte) FieldContentEnum.WhiteMan; }
            }
            else
            {
              // Nice combination

                _fields[ 1] = (byte) FieldContentEnum.BlackMan; _fields[ 2] = (byte) FieldContentEnum.Empty   ; _fields[ 3] = (byte) FieldContentEnum.Empty   ; _fields[ 4] = (byte) FieldContentEnum.Empty   ; _fields[ 5] = (byte) FieldContentEnum.Empty   ;
                _fields[ 6] = (byte) FieldContentEnum.Empty   ; _fields[ 7] = (byte) FieldContentEnum.Empty   ; _fields[ 8] = (byte) FieldContentEnum.BlackMan; _fields[ 9] = (byte) FieldContentEnum.BlackMan; _fields[10] = (byte) FieldContentEnum.BlackMan;
                _fields[11] = (byte) FieldContentEnum.WhiteMan; _fields[12] = (byte) FieldContentEnum.Empty   ; _fields[13] = (byte) FieldContentEnum.BlackMan; _fields[14] = (byte) FieldContentEnum.Empty   ; _fields[15] = (byte) FieldContentEnum.Empty   ;
                _fields[16] = (byte) FieldContentEnum.Empty   ; _fields[17] = (byte) FieldContentEnum.WhiteMan; _fields[18] = (byte) FieldContentEnum.Empty   ; _fields[19] = (byte) FieldContentEnum.BlackMan; _fields[20] = (byte) FieldContentEnum.BlackMan;
                _fields[21] = (byte) FieldContentEnum.WhiteMan; _fields[22] = (byte) FieldContentEnum.WhiteMan; _fields[23] = (byte) FieldContentEnum.BlackMan; _fields[24] = (byte) FieldContentEnum.BlackMan; _fields[25] = (byte) FieldContentEnum.Empty   ;
                _fields[26] = (byte) FieldContentEnum.Empty   ; _fields[27] = (byte) FieldContentEnum.WhiteMan; _fields[28] = (byte) FieldContentEnum.BlackMan; _fields[29] = (byte) FieldContentEnum.BlackMan; _fields[30] = (byte) FieldContentEnum.WhiteMan;
                _fields[31] = (byte) FieldContentEnum.Empty   ; _fields[32] = (byte) FieldContentEnum.Empty   ; _fields[33] = (byte) FieldContentEnum.BlackMan; _fields[34] = (byte) FieldContentEnum.WhiteMan; _fields[35] = (byte) FieldContentEnum.BlackMan;
                _fields[36] = (byte) FieldContentEnum.Empty   ; _fields[37] = (byte) FieldContentEnum.WhiteMan; _fields[38] = (byte) FieldContentEnum.WhiteMan; _fields[39] = (byte) FieldContentEnum.WhiteMan; _fields[40] = (byte) FieldContentEnum.WhiteMan;
                _fields[41] = (byte) FieldContentEnum.Empty   ; _fields[42] = (byte) FieldContentEnum.WhiteMan; _fields[43] = (byte) FieldContentEnum.WhiteMan; _fields[44] = (byte) FieldContentEnum.WhiteMan; _fields[45] = (byte) FieldContentEnum.BlackMan;
                _fields[46] = (byte) FieldContentEnum.Empty   ; _fields[47] = (byte) FieldContentEnum.Empty   ; _fields[48] = (byte) FieldContentEnum.Empty   ; _fields[49] = (byte) FieldContentEnum.Empty   ; _fields[50] = (byte) FieldContentEnum.Empty   ;

              // Nice combination mirrored

              //_fields[ 1] = (byte) FieldContentEnum.Empty   ; _fields[ 2] = (byte) FieldContentEnum.Empty   ; _fields[ 3] = (byte) FieldContentEnum.Empty   ; _fields[ 4] = (byte) FieldContentEnum.Empty   ; _fields[ 5] = (byte) FieldContentEnum.Empty   ;
              //_fields[ 6] = (byte) FieldContentEnum.WhiteMan; _fields[ 7] = (byte) FieldContentEnum.BlackMan; _fields[ 8] = (byte) FieldContentEnum.BlackMan; _fields[ 9] = (byte) FieldContentEnum.BlackMan; _fields[10] = (byte) FieldContentEnum.Empty   ;
              //_fields[11] = (byte) FieldContentEnum.BlackMan; _fields[12] = (byte) FieldContentEnum.BlackMan; _fields[13] = (byte) FieldContentEnum.BlackMan; _fields[14] = (byte) FieldContentEnum.BlackMan; _fields[15] = (byte) FieldContentEnum.Empty   ;
              //_fields[16] = (byte) FieldContentEnum.WhiteMan; _fields[17] = (byte) FieldContentEnum.BlackMan; _fields[18] = (byte) FieldContentEnum.WhiteMan; _fields[19] = (byte) FieldContentEnum.Empty   ; _fields[20] = (byte) FieldContentEnum.Empty   ;
              //_fields[21] = (byte) FieldContentEnum.BlackMan; _fields[22] = (byte) FieldContentEnum.WhiteMan; _fields[23] = (byte) FieldContentEnum.WhiteMan; _fields[24] = (byte) FieldContentEnum.BlackMan; _fields[25] = (byte) FieldContentEnum.Empty   ;
              //_fields[26] = (byte) FieldContentEnum.Empty   ; _fields[27] = (byte) FieldContentEnum.WhiteMan; _fields[28] = (byte) FieldContentEnum.WhiteMan; _fields[29] = (byte) FieldContentEnum.BlackMan; _fields[30] = (byte) FieldContentEnum.BlackMan;
              //_fields[31] = (byte) FieldContentEnum.WhiteMan; _fields[32] = (byte) FieldContentEnum.WhiteMan; _fields[33] = (byte) FieldContentEnum.Empty   ; _fields[34] = (byte) FieldContentEnum.BlackMan; _fields[35] = (byte) FieldContentEnum.Empty   ;
              //_fields[36] = (byte) FieldContentEnum.Empty   ; _fields[37] = (byte) FieldContentEnum.Empty   ; _fields[38] = (byte) FieldContentEnum.WhiteMan; _fields[39] = (byte) FieldContentEnum.Empty   ; _fields[40] = (byte) FieldContentEnum.BlackMan;
              //_fields[41] = (byte) FieldContentEnum.WhiteMan; _fields[42] = (byte) FieldContentEnum.WhiteMan; _fields[43] = (byte) FieldContentEnum.WhiteMan; _fields[44] = (byte) FieldContentEnum.Empty   ; _fields[45] = (byte) FieldContentEnum.Empty   ;
              //_fields[46] = (byte) FieldContentEnum.Empty   ; _fields[47] = (byte) FieldContentEnum.Empty   ; _fields[48] = (byte) FieldContentEnum.Empty   ; _fields[49] = (byte) FieldContentEnum.Empty   ; _fields[50] = (byte) FieldContentEnum.WhiteMan;

              //_fields[ 1] = (byte) FieldContentEnum.Empty   ; _fields[ 2] = (byte) FieldContentEnum.Empty   ; _fields[ 3] = (byte) FieldContentEnum.Empty   ; _fields[ 4] = (byte) FieldContentEnum.Empty   ; _fields[ 5] = (byte) FieldContentEnum.Empty   ;
              //_fields[ 6] = (byte) FieldContentEnum.Empty   ; _fields[ 7] = (byte) FieldContentEnum.Empty   ; _fields[ 8] = (byte) FieldContentEnum.Empty   ; _fields[ 9] = (byte) FieldContentEnum.BlackMan; _fields[10] = (byte) FieldContentEnum.Empty   ;
              //_fields[11] = (byte) FieldContentEnum.Empty   ; _fields[12] = (byte) FieldContentEnum.Empty   ; _fields[13] = (byte) FieldContentEnum.Empty   ; _fields[14] = (byte) FieldContentEnum.Empty   ; _fields[15] = (byte) FieldContentEnum.Empty   ;
              //_fields[16] = (byte) FieldContentEnum.Empty   ; _fields[17] = (byte) FieldContentEnum.Empty   ; _fields[18] = (byte) FieldContentEnum.Empty   ; _fields[19] = (byte) FieldContentEnum.BlackMan; _fields[20] = (byte) FieldContentEnum.Empty   ;
              //_fields[21] = (byte) FieldContentEnum.Empty   ; _fields[22] = (byte) FieldContentEnum.Empty   ; _fields[23] = (byte) FieldContentEnum.BlackMan; _fields[24] = (byte) FieldContentEnum.BlackMan; _fields[25] = (byte) FieldContentEnum.Empty   ;
              //_fields[26] = (byte) FieldContentEnum.BlackMan; _fields[27] = (byte) FieldContentEnum.WhiteMan; _fields[28] = (byte) FieldContentEnum.WhiteMan; _fields[29] = (byte) FieldContentEnum.Empty   ; _fields[30] = (byte) FieldContentEnum.Empty   ;
              //_fields[31] = (byte) FieldContentEnum.Empty   ; _fields[32] = (byte) FieldContentEnum.Empty   ; _fields[33] = (byte) FieldContentEnum.WhiteMan; _fields[34] = (byte) FieldContentEnum.WhiteMan; _fields[35] = (byte) FieldContentEnum.Empty   ;
              //_fields[36] = (byte) FieldContentEnum.BlackMan; _fields[37] = (byte) FieldContentEnum.WhiteMan; _fields[38] = (byte) FieldContentEnum.Empty   ; _fields[39] = (byte) FieldContentEnum.Empty   ; _fields[40] = (byte) FieldContentEnum.Empty   ;
              //_fields[41] = (byte) FieldContentEnum.Empty   ; _fields[42] = (byte) FieldContentEnum.Empty   ; _fields[43] = (byte) FieldContentEnum.Empty   ; _fields[44] = (byte) FieldContentEnum.Empty   ; _fields[45] = (byte) FieldContentEnum.BlackMan;
              //_fields[46] = (byte) FieldContentEnum.Empty   ; _fields[47] = (byte) FieldContentEnum.WhiteMan; _fields[48] = (byte) FieldContentEnum.Empty   ; _fields[49] = (byte) FieldContentEnum.Empty   ; _fields[50] = (byte) FieldContentEnum.WhiteMan;
            }

            WhiteOrBlacksTurn = TurnEnum.White;

            GetMovesAndTakes();
        }

        #endregion

        #region Public properties

        public IEnumerable<Move> PossibleMoves                              => _moves.Take(_numberOfMoves).Where(move => move.IsValid);
        public IEnumerable<Move> PossibleMovesFrom     (int fromFieldIndex) =>  PossibleMoves.Where(move => move.FromField == fromFieldIndex);
        public int               PossibleMovesFromCount(int fromFieldIndex) =>  PossibleMoves.Count(move => move.FromField == fromFieldIndex);

      //public bool HasMoves      =>  NumberOfMoves > 0;
        public bool HasTakes      => _moves.Take(1).First(move => move.IsValid).IsTake;

        public int  NumberOfMoves => _moves.Take(_numberOfMoves).Count(move => move.IsValid);

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
#endif
        }

        internal void GetTakes()
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
                        case (byte) FieldContentEnum.WhiteMan:
                           _fields[fromFieldIndex] = (byte) FieldContentEnum.Empty;

                            GetTakesForWhiteMan (fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = (byte) FieldContentEnum.WhiteMan;
                            break;
                        case (byte) FieldContentEnum.WhiteKing:
                           _fields[fromFieldIndex] = (byte) FieldContentEnum.Empty;

                            GetTakesForKing(FieldContentEnum.BlackMan, FieldContentEnum.BlackKing, fromFieldIndex, fromFieldIndex);

                           _fields[fromFieldIndex] = (byte) FieldContentEnum.WhiteKing;
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
                        case (byte) FieldContentEnum.BlackMan:
                           _fields[fromFieldIndex] = (byte) FieldContentEnum.Empty;

                            GetTakesForBlackMan (fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = (byte) FieldContentEnum.BlackMan;
                            break;
                        case (byte) FieldContentEnum.BlackKing:
                           _fields[fromFieldIndex] = (byte) FieldContentEnum.Empty;

                            GetTakesForKing(FieldContentEnum.WhiteMan, FieldContentEnum.WhiteKing, fromFieldIndex, fromFieldIndex);

                           _fields[fromFieldIndex] = (byte) FieldContentEnum.BlackKing;
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
                                if (_moves[moveIndex1].Equals(ref _moves[moveIndex2]))
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
            if (_fields[fieldIndexTo] == (byte) FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTake])
                {
                    case (byte) FieldContentEnum.BlackMan:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForWhiteMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.BlackMan;
                        break;
                    case (byte) FieldContentEnum.BlackKing:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForWhiteMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.BlackKing;
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
            if (_fields[fieldIndexTo] == (byte) FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTake])
                {
                    case (byte) FieldContentEnum.WhiteMan:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForBlackMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.WhiteMan;
                        break;
                    case (byte) FieldContentEnum.WhiteKing:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.Taken;

                       _takes [_numberOfTakesInMove] = fieldIndexTake;

                       _numberOfTakesInMove += 1;

                        GetTakesForBlackMan(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTake] = (byte) FieldContentEnum.WhiteKing;
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

        private bool GetTakesForKing(FieldContentEnum manToTake, FieldContentEnum kingToTake, int fieldIndexStart, int fieldIndexFrom)
        {
            // ReSharper disable once ReplaceWithSingleAssignment.False
            bool result = false;

            // ReSharper disable once ConvertIfToOrExpression
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexFrom,   UpLefts )) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexFrom,   UpRights)) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexFrom, DownLefts )) result = true;
            if (GetTakesForKing(manToTake, kingToTake, fieldIndexStart, fieldIndexFrom, DownRights)) result = true;

            return result;
        }

        private bool GetTakesForKing(FieldContentEnum manToTake, FieldContentEnum kingToTake, int fieldIndexStart, int fieldIndexFrom, int[] tryFields)
        {
            bool result        = false         ;
            bool tryNext       = true          ;
            int  tryFieldIndex = fieldIndexFrom;

            while (tryNext && ((tryFieldIndex =  tryFields[tryFieldIndex]) != 0))
            {
                byte fieldContent = _fields   [tryFieldIndex];

                if ((fieldContent == (byte) manToTake) || (fieldContent == (byte) kingToTake))
                {
                    int fieldIndexTake = tryFieldIndex;
                    int tryFieldIndex2 = tryFieldIndex;

                    while ((tryFieldIndex2 = tryFields[tryFieldIndex2]) != 0)
                    {
                        if (_fields[tryFieldIndex2] == (byte) FieldContentEnum.Empty)
                        {
                            result = true;

                            int fieldIndexEnd = tryFieldIndex2;

                            _fields[fieldIndexTake] = (byte) FieldContentEnum.Taken;

                            _takes[_numberOfTakesInMove] = fieldIndexTake;

                            _numberOfTakesInMove += 1;

                            if (! GetTakesForKing(manToTake, kingToTake, fieldIndexStart, tryFieldIndex2))
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
                else if (fieldContent != (byte) FieldContentEnum.Empty)
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
                if (_fields[ 6] == (byte) FieldContentEnum.WhiteMan) {                                                                                                if (_fields[ 1] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6,  1); }
                if (_fields[ 7] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 1] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  1); if (_fields[ 2] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  2); }
                if (_fields[ 8] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 2] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  2); if (_fields[ 3] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  3); }
                if (_fields[ 9] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 3] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  3); if (_fields[ 4] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  4); }
                if (_fields[10] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 4] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  4); if (_fields[ 5] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  5); }
                if (_fields[11] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 6] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  6); if (_fields[ 7] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  7); }
                if (_fields[12] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 7] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  7); if (_fields[ 8] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  8); }
                if (_fields[13] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 8] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  8); if (_fields[ 9] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  9); }
                if (_fields[14] == (byte) FieldContentEnum.WhiteMan) { if (_fields[ 9] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14,  9); if (_fields[10] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 10); }
                if (_fields[15] == (byte) FieldContentEnum.WhiteMan) { if (_fields[10] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 10);                                                                                         }
                if (_fields[16] == (byte) FieldContentEnum.WhiteMan) {                                                                                                if (_fields[11] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 11); }
                if (_fields[17] == (byte) FieldContentEnum.WhiteMan) { if (_fields[11] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 11); if (_fields[12] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 12); }
                if (_fields[18] == (byte) FieldContentEnum.WhiteMan) { if (_fields[12] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 12); if (_fields[13] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 13); }
                if (_fields[19] == (byte) FieldContentEnum.WhiteMan) { if (_fields[13] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 13); if (_fields[14] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 14); }
                if (_fields[20] == (byte) FieldContentEnum.WhiteMan) { if (_fields[14] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 14); if (_fields[15] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 15); }
                if (_fields[21] == (byte) FieldContentEnum.WhiteMan) { if (_fields[16] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 16); if (_fields[17] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 17); }
                if (_fields[22] == (byte) FieldContentEnum.WhiteMan) { if (_fields[17] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 17); if (_fields[18] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 18); }
                if (_fields[23] == (byte) FieldContentEnum.WhiteMan) { if (_fields[18] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 18); if (_fields[19] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 19); }
                if (_fields[24] == (byte) FieldContentEnum.WhiteMan) { if (_fields[19] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 19); if (_fields[20] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 20); }
                if (_fields[25] == (byte) FieldContentEnum.WhiteMan) { if (_fields[20] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 20);                                                                                         }
                if (_fields[26] == (byte) FieldContentEnum.WhiteMan) {                                                                                                if (_fields[21] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 21); }
                if (_fields[27] == (byte) FieldContentEnum.WhiteMan) { if (_fields[21] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 21); if (_fields[22] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 22); }
                if (_fields[28] == (byte) FieldContentEnum.WhiteMan) { if (_fields[22] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 22); if (_fields[23] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 23); }
                if (_fields[29] == (byte) FieldContentEnum.WhiteMan) { if (_fields[23] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 23); if (_fields[24] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 24); }
                if (_fields[30] == (byte) FieldContentEnum.WhiteMan) { if (_fields[24] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 24); if (_fields[25] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 25); }
                if (_fields[31] == (byte) FieldContentEnum.WhiteMan) { if (_fields[26] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 26); if (_fields[27] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 27); }
                if (_fields[32] == (byte) FieldContentEnum.WhiteMan) { if (_fields[27] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 27); if (_fields[28] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 28); }
                if (_fields[33] == (byte) FieldContentEnum.WhiteMan) { if (_fields[28] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 28); if (_fields[29] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 29); }
                if (_fields[34] == (byte) FieldContentEnum.WhiteMan) { if (_fields[29] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 29); if (_fields[30] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 30); }
                if (_fields[35] == (byte) FieldContentEnum.WhiteMan) { if (_fields[30] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 30);                                                                                         }
                if (_fields[36] == (byte) FieldContentEnum.WhiteMan) {                                                                                                if (_fields[31] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 31); }
                if (_fields[37] == (byte) FieldContentEnum.WhiteMan) { if (_fields[31] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 31); if (_fields[32] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 32); }
                if (_fields[38] == (byte) FieldContentEnum.WhiteMan) { if (_fields[32] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 32); if (_fields[33] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 33); }
                if (_fields[39] == (byte) FieldContentEnum.WhiteMan) { if (_fields[33] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 33); if (_fields[34] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 34); }
                if (_fields[40] == (byte) FieldContentEnum.WhiteMan) { if (_fields[34] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 34); if (_fields[35] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 35); }
                if (_fields[41] == (byte) FieldContentEnum.WhiteMan) { if (_fields[36] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 36); if (_fields[37] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 37); }
                if (_fields[42] == (byte) FieldContentEnum.WhiteMan) { if (_fields[37] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 37); if (_fields[38] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 38); }
                if (_fields[43] == (byte) FieldContentEnum.WhiteMan) { if (_fields[38] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 38); if (_fields[39] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 39); }
                if (_fields[44] == (byte) FieldContentEnum.WhiteMan) { if (_fields[39] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 39); if (_fields[40] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 40); }
                if (_fields[45] == (byte) FieldContentEnum.WhiteMan) { if (_fields[40] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 40);                                                                                         }
                if (_fields[46] == (byte) FieldContentEnum.WhiteMan) {                                                                                                if (_fields[41] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(46, 41); }
                if (_fields[47] == (byte) FieldContentEnum.WhiteMan) { if (_fields[41] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 41); if (_fields[42] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 42); }
                if (_fields[48] == (byte) FieldContentEnum.WhiteMan) { if (_fields[42] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 42); if (_fields[43] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 43); }
                if (_fields[49] == (byte) FieldContentEnum.WhiteMan) { if (_fields[43] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 43); if (_fields[44] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 44); }
                if (_fields[50] == (byte) FieldContentEnum.WhiteMan) { if (_fields[44] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 44); if (_fields[45] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 45); }
            }
            else
            {
                if (_fields[ 1] == (byte) FieldContentEnum.BlackMan) { if (_fields[ 7] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  7); if (_fields[ 6] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  6); }
                if (_fields[ 2] == (byte) FieldContentEnum.BlackMan) { if (_fields[ 8] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  8); if (_fields[ 7] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  7); }
                if (_fields[ 3] == (byte) FieldContentEnum.BlackMan) { if (_fields[ 9] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  9); if (_fields[ 8] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  8); }
                if (_fields[ 4] == (byte) FieldContentEnum.BlackMan) { if (_fields[10] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4, 10); if (_fields[ 9] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4,  9); }
                if (_fields[ 5] == (byte) FieldContentEnum.BlackMan) {                                                                                                if (_fields[10] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 5, 10); }
                if (_fields[ 6] == (byte) FieldContentEnum.BlackMan) { if (_fields[11] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6, 11);                                                                                         }
                if (_fields[ 7] == (byte) FieldContentEnum.BlackMan) { if (_fields[12] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 12); if (_fields[11] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 11); }
                if (_fields[ 8] == (byte) FieldContentEnum.BlackMan) { if (_fields[13] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 13); if (_fields[12] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 12); }
                if (_fields[ 9] == (byte) FieldContentEnum.BlackMan) { if (_fields[14] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 14); if (_fields[13] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 13); }
                if (_fields[10] == (byte) FieldContentEnum.BlackMan) { if (_fields[15] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 15); if (_fields[14] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 14); }
                if (_fields[11] == (byte) FieldContentEnum.BlackMan) { if (_fields[17] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 17); if (_fields[16] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 16); }
                if (_fields[12] == (byte) FieldContentEnum.BlackMan) { if (_fields[18] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 18); if (_fields[17] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 17); }
                if (_fields[13] == (byte) FieldContentEnum.BlackMan) { if (_fields[19] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 19); if (_fields[18] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 18); }
                if (_fields[14] == (byte) FieldContentEnum.BlackMan) { if (_fields[20] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 20); if (_fields[19] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 19); }
                if (_fields[15] == (byte) FieldContentEnum.BlackMan) {                                                                                                if (_fields[20] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 20); }
                if (_fields[16] == (byte) FieldContentEnum.BlackMan) { if (_fields[21] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 21);                                                                                         }
                if (_fields[17] == (byte) FieldContentEnum.BlackMan) { if (_fields[22] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 22); if (_fields[21] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 21); }
                if (_fields[18] == (byte) FieldContentEnum.BlackMan) { if (_fields[23] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 23); if (_fields[22] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 22); }
                if (_fields[19] == (byte) FieldContentEnum.BlackMan) { if (_fields[24] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 24); if (_fields[23] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 23); }
                if (_fields[20] == (byte) FieldContentEnum.BlackMan) { if (_fields[25] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 25); if (_fields[24] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 24); }
                if (_fields[21] == (byte) FieldContentEnum.BlackMan) { if (_fields[27] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 27); if (_fields[26] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 26); }
                if (_fields[22] == (byte) FieldContentEnum.BlackMan) { if (_fields[28] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 28); if (_fields[27] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 27); }
                if (_fields[23] == (byte) FieldContentEnum.BlackMan) { if (_fields[29] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 29); if (_fields[28] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 28); }
                if (_fields[24] == (byte) FieldContentEnum.BlackMan) { if (_fields[30] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 30); if (_fields[29] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 29); }
                if (_fields[25] == (byte) FieldContentEnum.BlackMan) {                                                                                                if (_fields[30] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 30); }
                if (_fields[26] == (byte) FieldContentEnum.BlackMan) { if (_fields[31] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 31);                                                                                         }
                if (_fields[27] == (byte) FieldContentEnum.BlackMan) { if (_fields[32] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 32); if (_fields[31] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 31); }
                if (_fields[28] == (byte) FieldContentEnum.BlackMan) { if (_fields[33] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 33); if (_fields[32] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 32); }
                if (_fields[29] == (byte) FieldContentEnum.BlackMan) { if (_fields[34] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 34); if (_fields[33] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 33); }
                if (_fields[30] == (byte) FieldContentEnum.BlackMan) { if (_fields[35] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 35); if (_fields[34] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 34); }
                if (_fields[31] == (byte) FieldContentEnum.BlackMan) { if (_fields[37] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 37); if (_fields[36] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 36); }
                if (_fields[32] == (byte) FieldContentEnum.BlackMan) { if (_fields[38] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 38); if (_fields[37] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 37); }
                if (_fields[33] == (byte) FieldContentEnum.BlackMan) { if (_fields[39] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 39); if (_fields[38] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 38); }
                if (_fields[34] == (byte) FieldContentEnum.BlackMan) { if (_fields[40] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 40); if (_fields[39] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 39); }
                if (_fields[35] == (byte) FieldContentEnum.BlackMan) {                                                                                                if (_fields[40] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 40); }
                if (_fields[36] == (byte) FieldContentEnum.BlackMan) { if (_fields[41] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 41);                                                                                         }
                if (_fields[37] == (byte) FieldContentEnum.BlackMan) { if (_fields[42] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 42); if (_fields[41] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 41); }
                if (_fields[38] == (byte) FieldContentEnum.BlackMan) { if (_fields[43] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 43); if (_fields[42] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 42); }
                if (_fields[39] == (byte) FieldContentEnum.BlackMan) { if (_fields[44] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 44); if (_fields[43] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 43); }
                if (_fields[40] == (byte) FieldContentEnum.BlackMan) { if (_fields[45] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 45); if (_fields[44] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 44); }
                if (_fields[41] == (byte) FieldContentEnum.BlackMan) { if (_fields[47] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 47); if (_fields[46] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 46); }
                if (_fields[42] == (byte) FieldContentEnum.BlackMan) { if (_fields[48] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 48); if (_fields[47] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 47); }
                if (_fields[43] == (byte) FieldContentEnum.BlackMan) { if (_fields[49] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 49); if (_fields[48] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 48); }
                if (_fields[44] == (byte) FieldContentEnum.BlackMan) { if (_fields[50] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 50); if (_fields[49] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 49); }
                if (_fields[45] == (byte) FieldContentEnum.BlackMan) {                                                                                                if (_fields[50] == (byte) FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 50); }
            }
        }

        private void GetMovesForKing(FieldContentEnum fieldContentKingToProcess)
        {
            for (int fieldIndexFrom = 1; fieldIndexFrom <= 50; fieldIndexFrom += 1)
            {
                if (_fields[fieldIndexFrom] == (byte) fieldContentKingToProcess)
                {
                    int tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = UpLefts[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == (byte) FieldContentEnum.Empty)
                        {
                           _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = UpRights[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == (byte) FieldContentEnum.Empty)
                        {
                            _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = DownLefts[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == (byte) FieldContentEnum.Empty)
                        {
                            _moves[_numberOfMoves++] = new Move(fieldIndexFrom, tryFieldIndex);
                        }
                        else
                        {
                            break;
                        }
                    }

                    tryFieldIndex = fieldIndexFrom;

                    while ((tryFieldIndex = DownRights[tryFieldIndex]) != 0)
                    {
                        if (_fields[tryFieldIndex] == (byte) FieldContentEnum.Empty)
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

        public void Clear()
        {
            for (int index =  1; index <= 50; index += 1) { _fields[index] = (byte) FieldContentEnum.Empty; }
        }

        public void FlipTurn()
        {
            WhiteOrBlacksTurn = (WhiteOrBlacksTurn == TurnEnum.White) ? TurnEnum.Black : TurnEnum.White;
        }

        public void MoveInSitu(ref Move move)
        {
            int fromFieldIndex = move.FromField;
            int   toFieldIndex = move.  ToField;

            byte fieldContentFrom  =       _fields[fromFieldIndex];

            // Mind the order (says Alex)!
           _fields[fromFieldIndex] = (byte) FieldContentEnum.Empty;
           _fields[  toFieldIndex] =        fieldContentFrom;

            if (move.TakeFields?.Count > 0)
            {
                move.FieldContentsTaken.Clear();

                foreach (int takeIndex in move.TakeFields)
                {
                    move.FieldContentsTaken.Add((FieldContentEnum) _fields[takeIndex]);

                   _fields[takeIndex] = (byte) FieldContentEnum.Empty;
                }
            }

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                if (fieldContentFrom == (byte) FieldContentEnum.WhiteMan)
                {
                    switch (toFieldIndex)
                    {
                        case  1: case  2: case  3: case  4: case  5:
                            move.Promoted = true;

                           _fields[toFieldIndex] = (byte) FieldContentEnum.WhiteKing;
                            break;
                    }
                }

                WhiteOrBlacksTurn  = TurnEnum.Black;
            }
            else
            {
                if (fieldContentFrom == (byte) FieldContentEnum.BlackMan)
                {
                    switch (toFieldIndex)
                    {
                        case 46: case 47: case 48: case 49: case 50:
                            move.Promoted = true;

                           _fields[toFieldIndex] = (byte) FieldContentEnum.BlackKing;
                            break;
                    }
                }

                WhiteOrBlacksTurn  = TurnEnum.White;
            }
        }

        public void UndoMoveInSitu(ref Move move)
        {
            int fromFieldIndex = move.FromField;
            int   toFieldIndex = move.  ToField;

            if (WhiteOrBlacksTurn == TurnEnum.Black)
            {
                if (move.Promoted)
                {
                    move.Promoted = false;

                    Debug.Assert((toFieldIndex >= 1) && (toFieldIndex <= 5));

                   _fields[toFieldIndex] = (byte) FieldContentEnum.WhiteMan; 
                }

                WhiteOrBlacksTurn  = TurnEnum.White;
            }
            else
            {
                if (move.Promoted)
                {
                    move.Promoted = false;

                    Debug.Assert((toFieldIndex >= 46) && (toFieldIndex <= 50));

                   _fields[toFieldIndex] = (byte) FieldContentEnum.BlackMan; 
                }

                WhiteOrBlacksTurn  = TurnEnum.Black;
            }

            if (move.TakeFields?.Count > 0)
            {
                //Debug.Assert(move.FieldContentsTaken != null);

                int index = 0;

                foreach (int takeIndex in move.TakeFields)
                {
                   _fields[takeIndex] = (byte) move.FieldContentsTaken[index];

                    index += 1;
                }
            }

            byte toFieldContent    = _fields[toFieldIndex];

           _fields[  toFieldIndex] =  (byte) FieldContentEnum.Empty;
           _fields[fromFieldIndex] =         toFieldContent;
        }

        public void Save(string filename = "default.pos")
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.WriteLine(JsonSerializer.Serialize(_fields));

            sw.Close  ();
            sw.Dispose();
        }

        public void Load(string filename = "default.pos")
        {
            StreamReader sr = new StreamReader(filename);

            string json = sr.ReadToEnd();

            sr.Close  ();
            sr.Dispose();

            FieldContentEnum[] fields = JsonSerializer.Deserialize<FieldContentEnum[]>(json);

            Debug.Assert(fields.Length == _fields.Length);

            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
            {
                _fields[fieldIndex] = (byte) fields[fieldIndex];
            }

            WhiteOrBlacksTurn = (TurnEnum) _fields[0];

            GetMovesAndTakes();
        }

      //public bool PositionEquals(byte[] fields) => _fields.AsSpan().SequenceEqual(fields);
        public bool PositionEquals(byte[] fields)
        {
            bool result = true;

            for (int fieldIndex = 0; fieldIndex < MaxNumberOfFields; fieldIndex += 1)
            {
                if (_fields[fieldIndex] != fields[fieldIndex])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public byte[] CopyFields()
        {
            byte[] fields = new byte[MaxNumberOfFields];

           _fields.AsSpan().CopyTo(fields);

            return fields;
        }

        public void CopyBackFields(byte[] fields) => fields.AsSpan().CopyTo(_fields);

        public double Evaluate()
        {
            int whiteManCount  = 0;
            int whiteKingCount = 0;
            int blackManCount  = 0;
            int blackKingCount = 0;

            for (int index = 1; index <= 50; index += 1)
            {
                switch (_fields[index])
                {
                    case (byte) FieldContentEnum.WhiteMan : whiteManCount  += 1; break;
                    case (byte) FieldContentEnum.WhiteKing: whiteKingCount += 1; break;
                    case (byte) FieldContentEnum.BlackMan : blackManCount  += 1; break;
                    case (byte) FieldContentEnum.BlackKing: blackKingCount += 1; break;
                }
            }

            return whiteManCount - blackManCount + whiteKingCount * 3d - blackKingCount * 3d;
        }

        #endregion

        #region Private properties

        private TurnEnum WhiteOrBlacksTurn
        {
            get => (TurnEnum) _fields[0];
            set =>            _fields[0] = (byte) value;
        }

        #endregion
    }
}
