using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Check.ViewModels;

#if RELEASE
using System.Windows;
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
            Empty         ,
            WhiteMan      ,
            BlackMan      ,
            WhiteKing     ,
            BlackKing     ,
            WhiteManTaken ,
            BlackManTaken ,
            WhiteKingTaken,
            BlackKingTaken
        }

        #endregion

        #region Fields

        private TurnEnum _whiteOrBlacksTurn;

        private      int _numberOfMoves         ;
        private      int _numberOfTakesInMove   ;
        private      int _numberOfTakesInMoveMax;

        private readonly FieldContentEnum[] _fields = new FieldContentEnum[MaxNumberOfFields];
        private readonly Move            [] _moves  = new Move            [MaxNumberOfMoves ];

        private readonly int             [] _takes = new int              [MaxNumberOfTakes ];
        private readonly int             [] _vias  = new int              [MaxNumberOfTakes ];

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
                _fields[11] = FieldContentEnum.WhiteMan; _fields[12] = FieldContentEnum.Empty   ; _fields[13] = FieldContentEnum.Empty   ; _fields[14] = FieldContentEnum.Empty   ; _fields[15] = FieldContentEnum.Empty   ;
                _fields[16] = FieldContentEnum.Empty   ; _fields[17] = FieldContentEnum.WhiteMan; _fields[18] = FieldContentEnum.BlackMan; _fields[19] = FieldContentEnum.BlackMan; _fields[20] = FieldContentEnum.BlackMan;
                _fields[21] = FieldContentEnum.WhiteMan; _fields[22] = FieldContentEnum.WhiteMan; _fields[23] = FieldContentEnum.BlackMan; _fields[24] = FieldContentEnum.Empty   ; _fields[25] = FieldContentEnum.Empty   ;
                _fields[26] = FieldContentEnum.Empty   ; _fields[27] = FieldContentEnum.WhiteMan; _fields[28] = FieldContentEnum.BlackMan; _fields[29] = FieldContentEnum.BlackMan; _fields[30] = FieldContentEnum.WhiteMan;
                _fields[31] = FieldContentEnum.Empty   ; _fields[32] = FieldContentEnum.Empty   ; _fields[33] = FieldContentEnum.BlackMan; _fields[34] = FieldContentEnum.WhiteMan; _fields[35] = FieldContentEnum.BlackMan;
                _fields[36] = FieldContentEnum.Empty   ; _fields[37] = FieldContentEnum.WhiteMan; _fields[38] = FieldContentEnum.WhiteMan; _fields[39] = FieldContentEnum.WhiteMan; _fields[40] = FieldContentEnum.WhiteMan;
                _fields[41] = FieldContentEnum.Empty   ; _fields[42] = FieldContentEnum.WhiteMan; _fields[43] = FieldContentEnum.WhiteMan; _fields[44] = FieldContentEnum.WhiteMan; _fields[45] = FieldContentEnum.BlackMan;
                _fields[46] = FieldContentEnum.Empty   ; _fields[47] = FieldContentEnum.Empty   ; _fields[48] = FieldContentEnum.Empty   ; _fields[49] = FieldContentEnum.Empty   ; _fields[50] = FieldContentEnum.Empty   ;
            }

            WhiteOrBlacksTurn = TurnEnum.Black;
        }

        #endregion

        #region Field properties

        public FieldContentEnum F01 { get => _fields[ 1]; set => _fields[ 1] = value; }
        public FieldContentEnum F02 { get => _fields[ 2]; set => _fields[ 2] = value; }
        public FieldContentEnum F03 { get => _fields[ 3]; set => _fields[ 3] = value; }
        public FieldContentEnum F04 { get => _fields[ 4]; set => _fields[ 4] = value; }
        public FieldContentEnum F05 { get => _fields[ 5]; set => _fields[ 5] = value; }
        public FieldContentEnum F06 { get => _fields[ 6]; set => _fields[ 6] = value; }
        public FieldContentEnum F07 { get => _fields[ 7]; set => _fields[ 7] = value; }
        public FieldContentEnum F08 { get => _fields[ 8]; set => _fields[ 8] = value; }
        public FieldContentEnum F09 { get => _fields[ 9]; set => _fields[ 9] = value; }
        public FieldContentEnum F10 { get => _fields[10]; set => _fields[10] = value; }
        public FieldContentEnum F11 { get => _fields[11]; set => _fields[11] = value; }
        public FieldContentEnum F12 { get => _fields[12]; set => _fields[12] = value; }
        public FieldContentEnum F13 { get => _fields[13]; set => _fields[13] = value; }
        public FieldContentEnum F14 { get => _fields[14]; set => _fields[14] = value; }
        public FieldContentEnum F15 { get => _fields[15]; set => _fields[15] = value; }
        public FieldContentEnum F16 { get => _fields[16]; set => _fields[16] = value; }
        public FieldContentEnum F17 { get => _fields[17]; set => _fields[17] = value; }
        public FieldContentEnum F18 { get => _fields[18]; set => _fields[18] = value; }
        public FieldContentEnum F19 { get => _fields[19]; set => _fields[19] = value; }
        public FieldContentEnum F20 { get => _fields[20]; set => _fields[20] = value; }
        public FieldContentEnum F21 { get => _fields[21]; set => _fields[21] = value; }
        public FieldContentEnum F22 { get => _fields[22]; set => _fields[22] = value; }
        public FieldContentEnum F23 { get => _fields[23]; set => _fields[23] = value; }
        public FieldContentEnum F24 { get => _fields[24]; set => _fields[24] = value; }
        public FieldContentEnum F25 { get => _fields[25]; set => _fields[25] = value; }
        public FieldContentEnum F26 { get => _fields[26]; set => _fields[26] = value; }
        public FieldContentEnum F27 { get => _fields[27]; set => _fields[27] = value; }
        public FieldContentEnum F28 { get => _fields[28]; set => _fields[28] = value; }
        public FieldContentEnum F29 { get => _fields[29]; set => _fields[29] = value; }
        public FieldContentEnum F30 { get => _fields[30]; set => _fields[30] = value; }
        public FieldContentEnum F31 { get => _fields[31]; set => _fields[31] = value; }
        public FieldContentEnum F32 { get => _fields[32]; set => _fields[32] = value; }
        public FieldContentEnum F33 { get => _fields[33]; set => _fields[33] = value; }
        public FieldContentEnum F34 { get => _fields[34]; set => _fields[34] = value; }
        public FieldContentEnum F35 { get => _fields[35]; set => _fields[35] = value; }
        public FieldContentEnum F36 { get => _fields[36]; set => _fields[36] = value; }
        public FieldContentEnum F37 { get => _fields[37]; set => _fields[37] = value; }
        public FieldContentEnum F38 { get => _fields[38]; set => _fields[38] = value; }
        public FieldContentEnum F39 { get => _fields[39]; set => _fields[39] = value; }
        public FieldContentEnum F40 { get => _fields[40]; set => _fields[40] = value; }
        public FieldContentEnum F41 { get => _fields[41]; set => _fields[41] = value; }
        public FieldContentEnum F42 { get => _fields[42]; set => _fields[42] = value; }
        public FieldContentEnum F43 { get => _fields[43]; set => _fields[43] = value; }
        public FieldContentEnum F44 { get => _fields[44]; set => _fields[44] = value; }
        public FieldContentEnum F45 { get => _fields[45]; set => _fields[45] = value; }
        public FieldContentEnum F46 { get => _fields[46]; set => _fields[46] = value; }
        public FieldContentEnum F47 { get => _fields[47]; set => _fields[47] = value; }
        public FieldContentEnum F48 { get => _fields[48]; set => _fields[48] = value; }
        public FieldContentEnum F49 { get => _fields[49]; set => _fields[49] = value; }
        public FieldContentEnum F50 { get => _fields[50]; set => _fields[50] = value; }

        #endregion

        #region Public properties

        public TurnEnum WhiteOrBlacksTurn
        {
            get => _whiteOrBlacksTurn;
            set
            {
               _whiteOrBlacksTurn = value;

                GetMovesAndTakes();
            }
        }

        public IEnumerable<Move> PossibleMoves => _moves.Take(_numberOfMoves);

        #endregion

        #region Get moves and takes

        public void GetMovesAndTakes()
        {
            GetTakes();

            if (_numberOfMoves == 0)
            {
                GetMoves();
            }
        }

        private void GetTakes()
        {
           _numberOfMoves          = 0;
           _numberOfTakesInMove    = 0;
           _numberOfTakesInMoveMax = 0;

            DateTime now = DateTime.Now;

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                for (int fromFieldIndex = 1; fromFieldIndex <= 50; fromFieldIndex += 1)
                {
                    switch (_fields[fromFieldIndex])
                    {
                        case FieldContentEnum.WhiteMan:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeWhite(fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.WhiteMan;
                            break;
                        case FieldContentEnum.WhiteKing:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeWhite(fromFieldIndex, 0, fromFieldIndex);

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

                            GetTakeBlack(fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.BlackMan;
                            break;
                        case FieldContentEnum.BlackKing:
                           _fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeBlack(fromFieldIndex, 0, fromFieldIndex);

                           _fields[fromFieldIndex] = FieldContentEnum.BlackKing;
                            break;
                    }
                }
            }

            if (_numberOfMoves > 0)
            {

            }

#if DEBUG
            Debug.WriteLine((DateTime.Now - now).Milliseconds + " mSec");
#else
            MessageBox.Show((DateTime.Now - now).Milliseconds + " mSec");
#endif
        }

        private bool GetTakeWhite(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool result = false;
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1:                                                                                                                                                                                                                                                                            if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  1,  7,  12)) result = true; break;
                case  2:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  2,  7, 11)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  2,  8,  13)) result = true; break;
                case  3:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  3,  8, 12)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  3,  9,  14)) result = true; break;
                case  4:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  4,  9, 13)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  4, 10,  15)) result = true; break;
                case  5:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  5, 10, 14)) result = true;                                                                                           break;
                case  6:                                                                                                                                                                                                                                                                            if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  6, 11,  17)) result = true; break;
                case  7:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11, 16)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 12,  18)) result = true; break;
                case  8:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12, 17)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 13,  19)) result = true; break;
                case  9:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13, 18)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 14,  20)) result = true; break;
                case 10:                                                                                                                                                                                   if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14, 19)) result = true;                                                                                           break;
                case 11:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7,  2)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 17,  22)) result = true; break;
                case 12: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  7,  1)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8,  3)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17, 21)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 18,  23)) result = true; break;
                case 13: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  8,  2)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9,  4)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18, 22)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 19,  24)) result = true; break;
                case 14: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  9,  3)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10,  5)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19, 23)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 20,  25)) result = true; break;
                case 15: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 15, 10,  4)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 15, 20, 24)) result = true;                                                                                           break;
                case 16:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 16, 11,  7)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 16, 21,  27)) result = true; break;
                case 17: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 11,  6)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12,  8)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21, 26)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 22,  28)) result = true; break;
                case 18: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 12,  7)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13,  9)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22, 27)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 23,  29)) result = true; break;
                case 19: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 13,  8)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14, 10)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23, 28)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 24,  30)) result = true; break;
                case 20: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 14,  9)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24, 29)) result = true;                                                                                           break;
                case 21:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17, 12)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 27,  32)) result = true; break;
                case 22: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 17, 11)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18, 13)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27, 31)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 28,  33)) result = true; break;
                case 23: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 18, 12)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19, 14)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28, 32)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 29,  34)) result = true; break;
                case 24: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 19, 13)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20, 15)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29, 33)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 30,  35)) result = true; break;
                case 25: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 25, 20, 14)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 25, 30, 34)) result = true;                                                                                           break;
                case 26:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 26, 21, 17)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 26, 31,  37)) result = true; break;
                case 27: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 21, 16)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22, 18)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31, 36)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 32,  38)) result = true; break;
                case 28: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 22, 17)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23, 19)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32, 37)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 33,  39)) result = true; break;
                case 29: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 23, 18)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24, 20)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33, 38)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 34,  40)) result = true; break;
                case 30: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 24, 19)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34, 39)) result = true;                                                                                           break;
                case 31:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27, 22)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 37,  42)) result = true; break;
                case 32: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 27, 21)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28, 23)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37, 41)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 38,  43)) result = true; break;
                case 33: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 28, 22)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29, 24)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38, 42)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 39,  44)) result = true; break;
                case 34: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 29, 23)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30, 25)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39, 43)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 40,  45)) result = true; break;
                case 35: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 35, 30, 24)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 35, 40, 44)) result = true;                                                                                           break;
                case 36:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 36, 31, 27)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 36, 41,  47)) result = true; break;
                case 37: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 31, 26)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32, 28)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41, 46)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 42,  48)) result = true; break;
                case 38: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 32, 27)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33, 29)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42, 47)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 43,  49)) result = true; break;
                case 39: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 33, 28)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34, 30)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43, 48)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 44,  50)) result = true; break;
                case 40: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 34, 29)) result = true;                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44, 49)) result = true;                                                                                           break;
                case 41:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37, 32)) result = true;                                                                                                                                                                                    break;
                case 42: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 37, 31)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38, 33)) result = true;                                                                                                                                                                                    break;
                case 43: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 38, 32)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39, 34)) result = true;                                                                                                                                                                                    break;
                case 44: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 39, 33)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40, 35)) result = true;                                                                                                                                                                                    break;
                case 45: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 45, 40, 34)) result = true;                                                                                                                                                                                                                                                                             break;
                case 46:                                                                                          if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 46, 41, 37)) result = true;                                                                                                                                                                                    break;
                case 47: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 47, 41, 36)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 47, 42, 38)) result = true;                                                                                                                                                                                    break;
                case 48: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 48, 42, 37)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 48, 43, 39)) result = true;                                                                                                                                                                                    break;
                case 49: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 49, 43, 38)) result = true; if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 49, 44, 40)) result = true;                                                                                                                                                                                    break;
                case 50: if (GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 50, 44, 39)) result = true;                                                                                                                                                                                                                                                                             break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }

            return result;
        }

        private bool GetTakeWhite(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom, int fieldIndexTakes, int fieldIndexTo)
        {
            bool result = false;

            if (_fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTakes])
                {
                    case FieldContentEnum.BlackMan:
                        fieldIndexEnd = fieldIndexTo;

                     //_fields[fieldIndexFrom      ] = FieldContentEnum.Empty        ;
                       _fields[fieldIndexTakes     ] = FieldContentEnum.BlackManTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;
                       _vias  [_numberOfTakesInMove] = fieldIndexTo   ;

                       _numberOfTakesInMove += 1;

                        result = GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTakes     ] = FieldContentEnum.BlackMan  ;
                     //_fields[fieldIndexFrom      ] = FieldContentEnum.WhitePiece;
                        break;
                    case FieldContentEnum.BlackKing:
                        fieldIndexEnd = fieldIndexTo;

                     //_fields[fieldIndexFrom      ] = FieldContentEnum.Empty         ;
                       _fields[fieldIndexTakes     ] = FieldContentEnum.BlackKingTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;
                       _vias  [_numberOfTakesInMove] = fieldIndexTo   ;

                       _numberOfTakesInMove += 1;

                        result = GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTakes     ] = FieldContentEnum.BlackKing ;
                     //_fields[fieldIndexFrom      ] = FieldContentEnum.WhitePiece;
                        break;
                    default:
                        result = true;

                        if (_numberOfTakesInMove > 0)
                        {
                            if (hadOne == false)
                            {
                                hadOne  = true ;

                                //if (_numberOfTakesInMoveMax < _numberOfTakesInMove) { _numberOfMoves = 0; }
                                //if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                                //{
                                //    _numberOfTakesInMoveMax = _numberOfTakesInMove;

                                    _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd, _numberOfTakesInMove, _takes, _vias);
                                //}
                            }
                        }
                        break;
                }
            }

            return result;
        }

        private bool GetTakeBlack(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool result = false;
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  1,  7, 12)) result = true;                                                                                                                                                                                                                                                                             break;
                case  2: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  2,  8, 13)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  2,  7, 11)) result = true;                                                                                                                                                                                    break;
                case  3: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  3,  9, 14)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  3,  8, 12)) result = true;                                                                                                                                                                                    break;
                case  4: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  4, 10, 15)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  4,  9, 13)) result = true;                                                                                                                                                                                    break;
                case  5:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  5, 10, 14)) result = true;                                                                                                                                                                                    break;
                case  6: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  6, 11, 17)) result = true;                                                                                                                                                                                                                                                                             break;
                case  7: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 12, 18)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11, 16)) result = true;                                                                                                                                                                                    break;
                case  8: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 13, 19)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12, 17)) result = true;                                                                                                                                                                                    break;
                case  9: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 14, 20)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13, 18)) result = true;                                                                                                                                                                                    break;
                case 10:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14, 19)) result = true;                                                                                                                                                                                    break;
                case 11: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 17, 22)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7,  2)) result = true;                                                                                           break;
                case 12: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 18, 23)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17, 21)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8,  3)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  7,   1)) result = true; break;
                case 13: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 19, 24)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18, 22)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9,  4)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  8,   2)) result = true; break;
                case 14: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 20, 25)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19, 23)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10,  5)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  9,   3)) result = true; break;
                case 15:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 15, 20, 24)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 15, 10,   4)) result = true; break;
                case 16: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 16, 21, 27)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 16, 11,  7)) result = true;                                                                                           break;
                case 17: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 22, 28)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21, 26)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12,  8)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 11,   6)) result = true; break;
                case 18: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 23, 29)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22, 27)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13,  9)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 12,   7)) result = true; break;
                case 19: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 24, 30)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23, 28)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14, 10)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 13,   8)) result = true; break;
                case 20:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24, 29)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 14,   9)) result = true; break;
                case 21: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 27, 32)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17, 12)) result = true;                                                                                           break;
                case 22: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 28, 33)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27, 31)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18, 13)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 17,  11)) result = true; break;
                case 23: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 29, 34)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28, 32)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19, 14)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 18,  12)) result = true; break;
                case 24: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 30, 35)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29, 33)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20, 15)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 19,  13)) result = true; break;
                case 25:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 25, 30, 34)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 25, 20,  14)) result = true; break;
                case 26: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 26, 31, 37)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 26, 21, 17)) result = true;                                                                                           break;
                case 27: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 32, 38)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31, 36)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22, 18)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 21,  16)) result = true; break;
                case 28: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 33, 39)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32, 37)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23, 19)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 22,  17)) result = true; break;
                case 29: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 34, 40)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33, 38)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24, 20)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 23,  18)) result = true; break;
                case 30:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34, 39)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 24,  19)) result = true; break;
                case 31: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 37, 42)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27, 22)) result = true;                                                                                           break;
                case 32: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 38, 43)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37, 41)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28, 23)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 27,  21)) result = true; break;
                case 33: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 39, 44)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38, 42)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29, 24)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 28,  22)) result = true; break;
                case 34: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 40, 45)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39, 43)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30, 25)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 29,  23)) result = true; break;
                case 35:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 35, 40, 44)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 35, 30,  24)) result = true; break;
                case 36: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 36, 41, 47)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 36, 31, 27)) result = true;                                                                                           break;
                case 37: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 42, 48)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41, 46)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32, 28)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 31,  26)) result = true; break;
                case 38: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 43, 49)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42, 47)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33, 29)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 32,  27)) result = true; break;
                case 39: if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 44, 50)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43, 48)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34, 30)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 33,  28)) result = true; break;
                case 40:                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44, 49)) result = true;                                                                                          if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 34,  29)) result = true; break;
                case 41:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37, 32)) result = true;                                                                                           break;
                case 42:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38, 33)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 37,  31)) result = true; break;
                case 43:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39, 34)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 38,  32)) result = true; break;
                case 44:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40, 35)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 39,  33)) result = true; break;
                case 45:                                                                                                                                                                                                                                                                            if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 45, 40,  34)) result = true; break;
                case 46:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 46, 41, 37)) result = true;                                                                                           break;
                case 47:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 47, 42, 38)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 47, 41,  36)) result = true; break;
                case 48:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 48, 43, 39)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 48, 42,  37)) result = true; break;
                case 49:                                                                                                                                                                                   if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 49, 44, 40)) result = true; if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 49, 43,  38)) result = true; break;
                case 50:                                                                                                                                                                                                                                                                            if (GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 50, 44,  39)) result = true; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }

            return result;
        }

        private bool GetTakeBlack(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom, int fieldIndexTakes, int fieldIndexTo)
        {
            bool result = false;

            if (_fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexTakes])
                {
                    case FieldContentEnum.WhiteMan:
                        fieldIndexEnd = fieldIndexTo;

                     //_fields[fieldIndexFrom      ] = FieldContentEnum.Empty        ;
                       _fields[fieldIndexTakes     ] = FieldContentEnum.WhiteManTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;
                       _vias  [_numberOfTakesInMove] = fieldIndexTo   ;

                       _numberOfTakesInMove += 1;

                        result = GetTakeBlack(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTakes     ] = FieldContentEnum.WhiteMan  ;
                     //_fields[fieldIndexFrom      ] = FieldContentEnum.BlackPiece;
                        break;
                    case FieldContentEnum.WhiteKing:
                        fieldIndexEnd = fieldIndexTo;

                     //_fields[fieldIndexFrom      ] = FieldContentEnum.Empty         ;
                       _fields[fieldIndexTakes     ] = FieldContentEnum.WhiteKingTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;
                       _vias  [_numberOfTakesInMove] = fieldIndexTo   ;

                       _numberOfTakesInMove += 1;

                        result = GetTakeBlack(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexTakes     ] = FieldContentEnum.WhiteKing ;
                     //_fields[fieldIndexFrom      ] = FieldContentEnum.BlackPiece;
                        break;
                    default:
                        result = true;

                        if (_numberOfTakesInMove > 0)
                        {
                            if (hadOne == false)
                            {
                                hadOne = true;

                                //if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                                //{
                                //    _numberOfTakesInMoveMax  = _numberOfTakesInMove;

                                _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd,
                                    _numberOfTakesInMove, _takes, _vias);
                                //}
                            }
                        }
                        break;
                }
            }

            return result;
        }

        private void GetMoves()
        {
           _numberOfMoves = 0; // Defensive

            DateTime now = DateTime.Now;

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

#if DEBUG
            Debug.WriteLine((DateTime.Now - now).Milliseconds + " mSec");
#else
            MessageBox.Show((DateTime.Now - now).Milliseconds + " mSec");
#endif
        }

        #endregion

        public void Move(int fromFieldIndex, int toFieldIndex)
        {
           _fields[  toFieldIndex] = _fields[fromFieldIndex];
           _fields[fromFieldIndex] =  FieldContentEnum.Empty;

           WhiteOrBlacksTurn = (WhiteOrBlacksTurn == TurnEnum.White) ? TurnEnum.Black : TurnEnum.White;
        }
    }
}
