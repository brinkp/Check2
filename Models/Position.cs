using Check.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#if DEBUG
using System.Diagnostics;
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

        private  TurnEnum _whiteOrBlacksTurn;

        private  int _numberOfMoves         ;
        private  int _numberOfTakesInMove   ;
        private  int _numberOfTakesInMoveMax;

        internal readonly FieldContentEnum[]  Fields = new FieldContentEnum[MaxNumberOfFields];
        private  readonly Move            [] _moves  = new Move            [MaxNumberOfMoves ];
        private  readonly int             [] _takes  = new int             [MaxNumberOfTakes ];

        private readonly int[] _upLefts     = new int [MaxNumberOfFields]
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

        private readonly int[] _upRights    = new int [MaxNumberOfFields]
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

        private readonly int[] _downLefts   = new int [MaxNumberOfFields]
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

        private readonly int[] _downRightss = new int [MaxNumberOfFields]
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

        #endregion

        #region Constructors

        public Position(bool startPosition = true)
        {
            if (startPosition)
            {
                for (int index =  1; index <= 20; index += 1) { Fields[index] = FieldContentEnum.BlackMan; }
                for (int index = 21; index <= 30; index += 1) { Fields[index] = FieldContentEnum.Empty   ; }
                for (int index = 31; index <= 50; index += 1) { Fields[index] = FieldContentEnum.WhiteMan; }
            }
            else
            {
              //for (int index =  1; index <= 50; index += 1) { _fields[index] = FieldContentEnum.Empty;    }

                Fields[ 1] = FieldContentEnum.BlackMan; Fields[ 2] = FieldContentEnum.Empty   ; Fields[ 3] = FieldContentEnum.Empty   ; Fields[ 4] = FieldContentEnum.Empty   ; Fields[ 5] = FieldContentEnum.Empty   ;
                Fields[ 6] = FieldContentEnum.Empty   ; Fields[ 7] = FieldContentEnum.Empty   ; Fields[ 8] = FieldContentEnum.BlackMan; Fields[ 9] = FieldContentEnum.BlackMan; Fields[10] = FieldContentEnum.BlackMan;
                Fields[11] = FieldContentEnum.WhiteMan; Fields[12] = FieldContentEnum.Empty   ; Fields[13] = FieldContentEnum.Empty   ; Fields[14] = FieldContentEnum.Empty   ; Fields[15] = FieldContentEnum.Empty   ;
                Fields[16] = FieldContentEnum.Empty   ; Fields[17] = FieldContentEnum.WhiteMan; Fields[18] = FieldContentEnum.BlackMan; Fields[19] = FieldContentEnum.BlackMan; Fields[20] = FieldContentEnum.BlackMan;
                Fields[21] = FieldContentEnum.WhiteMan; Fields[22] = FieldContentEnum.WhiteMan; Fields[23] = FieldContentEnum.BlackMan; Fields[24] = FieldContentEnum.Empty   ; Fields[25] = FieldContentEnum.Empty   ;
                Fields[26] = FieldContentEnum.Empty   ; Fields[27] = FieldContentEnum.WhiteMan; Fields[28] = FieldContentEnum.BlackMan; Fields[29] = FieldContentEnum.BlackMan; Fields[30] = FieldContentEnum.WhiteMan;
                Fields[31] = FieldContentEnum.Empty   ; Fields[32] = FieldContentEnum.Empty   ; Fields[33] = FieldContentEnum.BlackMan; Fields[34] = FieldContentEnum.WhiteMan; Fields[35] = FieldContentEnum.BlackMan;
                Fields[36] = FieldContentEnum.Empty   ; Fields[37] = FieldContentEnum.WhiteMan; Fields[38] = FieldContentEnum.WhiteMan; Fields[39] = FieldContentEnum.WhiteMan; Fields[40] = FieldContentEnum.WhiteMan;
                Fields[41] = FieldContentEnum.Empty   ; Fields[42] = FieldContentEnum.WhiteMan; Fields[43] = FieldContentEnum.WhiteMan; Fields[44] = FieldContentEnum.WhiteMan; Fields[45] = FieldContentEnum.BlackMan;
                Fields[46] = FieldContentEnum.Empty   ; Fields[47] = FieldContentEnum.Empty   ; Fields[48] = FieldContentEnum.Empty   ; Fields[49] = FieldContentEnum.Empty   ; Fields[50] = FieldContentEnum.Empty   ;
            }

            WhiteOrBlacksTurn = TurnEnum.White;
        }

        #endregion

        #region Field properties

        public FieldContentEnum F01 { get => Fields[ 1]; set => Fields[ 1] = value; }
        public FieldContentEnum F02 { get => Fields[ 2]; set => Fields[ 2] = value; }
        public FieldContentEnum F03 { get => Fields[ 3]; set => Fields[ 3] = value; }
        public FieldContentEnum F04 { get => Fields[ 4]; set => Fields[ 4] = value; }
        public FieldContentEnum F05 { get => Fields[ 5]; set => Fields[ 5] = value; }
        public FieldContentEnum F06 { get => Fields[ 6]; set => Fields[ 6] = value; }
        public FieldContentEnum F07 { get => Fields[ 7]; set => Fields[ 7] = value; }
        public FieldContentEnum F08 { get => Fields[ 8]; set => Fields[ 8] = value; }
        public FieldContentEnum F09 { get => Fields[ 9]; set => Fields[ 9] = value; }
        public FieldContentEnum F10 { get => Fields[10]; set => Fields[10] = value; }
        public FieldContentEnum F11 { get => Fields[11]; set => Fields[11] = value; }
        public FieldContentEnum F12 { get => Fields[12]; set => Fields[12] = value; }
        public FieldContentEnum F13 { get => Fields[13]; set => Fields[13] = value; }
        public FieldContentEnum F14 { get => Fields[14]; set => Fields[14] = value; }
        public FieldContentEnum F15 { get => Fields[15]; set => Fields[15] = value; }
        public FieldContentEnum F16 { get => Fields[16]; set => Fields[16] = value; }
        public FieldContentEnum F17 { get => Fields[17]; set => Fields[17] = value; }
        public FieldContentEnum F18 { get => Fields[18]; set => Fields[18] = value; }
        public FieldContentEnum F19 { get => Fields[19]; set => Fields[19] = value; }
        public FieldContentEnum F20 { get => Fields[20]; set => Fields[20] = value; }
        public FieldContentEnum F21 { get => Fields[21]; set => Fields[21] = value; }
        public FieldContentEnum F22 { get => Fields[22]; set => Fields[22] = value; }
        public FieldContentEnum F23 { get => Fields[23]; set => Fields[23] = value; }
        public FieldContentEnum F24 { get => Fields[24]; set => Fields[24] = value; }
        public FieldContentEnum F25 { get => Fields[25]; set => Fields[25] = value; }
        public FieldContentEnum F26 { get => Fields[26]; set => Fields[26] = value; }
        public FieldContentEnum F27 { get => Fields[27]; set => Fields[27] = value; }
        public FieldContentEnum F28 { get => Fields[28]; set => Fields[28] = value; }
        public FieldContentEnum F29 { get => Fields[29]; set => Fields[29] = value; }
        public FieldContentEnum F30 { get => Fields[30]; set => Fields[30] = value; }
        public FieldContentEnum F31 { get => Fields[31]; set => Fields[31] = value; }
        public FieldContentEnum F32 { get => Fields[32]; set => Fields[32] = value; }
        public FieldContentEnum F33 { get => Fields[33]; set => Fields[33] = value; }
        public FieldContentEnum F34 { get => Fields[34]; set => Fields[34] = value; }
        public FieldContentEnum F35 { get => Fields[35]; set => Fields[35] = value; }
        public FieldContentEnum F36 { get => Fields[36]; set => Fields[36] = value; }
        public FieldContentEnum F37 { get => Fields[37]; set => Fields[37] = value; }
        public FieldContentEnum F38 { get => Fields[38]; set => Fields[38] = value; }
        public FieldContentEnum F39 { get => Fields[39]; set => Fields[39] = value; }
        public FieldContentEnum F40 { get => Fields[40]; set => Fields[40] = value; }
        public FieldContentEnum F41 { get => Fields[41]; set => Fields[41] = value; }
        public FieldContentEnum F42 { get => Fields[42]; set => Fields[42] = value; }
        public FieldContentEnum F43 { get => Fields[43]; set => Fields[43] = value; }
        public FieldContentEnum F44 { get => Fields[44]; set => Fields[44] = value; }
        public FieldContentEnum F45 { get => Fields[45]; set => Fields[45] = value; }
        public FieldContentEnum F46 { get => Fields[46]; set => Fields[46] = value; }
        public FieldContentEnum F47 { get => Fields[47]; set => Fields[47] = value; }
        public FieldContentEnum F48 { get => Fields[48]; set => Fields[48] = value; }
        public FieldContentEnum F49 { get => Fields[49]; set => Fields[49] = value; }
        public FieldContentEnum F50 { get => Fields[50]; set => Fields[50] = value; }

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
                    switch (Fields[fromFieldIndex])
                    {
                        case FieldContentEnum.WhiteMan:
                           Fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeWhite(fromFieldIndex, 0, fromFieldIndex);

                           Fields[fromFieldIndex] = FieldContentEnum.WhiteMan;
                            break;
                        case FieldContentEnum.WhiteKing:
                           Fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeWhite(fromFieldIndex, 0, fromFieldIndex);

                           Fields[fromFieldIndex] = FieldContentEnum.WhiteKing;
                            break;
                    }
                }
            }
            else
            {
                for (int fromFieldIndex = 1; fromFieldIndex <= 50; fromFieldIndex += 1)
                {
                    switch (Fields[fromFieldIndex])
                    {
                        case FieldContentEnum.BlackMan:
                           Fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeBlack(fromFieldIndex, 0, fromFieldIndex);

                           Fields[fromFieldIndex] = FieldContentEnum.BlackMan;
                            break;
                        case FieldContentEnum.BlackKing:
                           Fields[fromFieldIndex] = FieldContentEnum.Empty;

                            GetTakeBlack(fromFieldIndex, 0, fromFieldIndex);

                           Fields[fromFieldIndex] = FieldContentEnum.BlackKing;
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

        private void GetTakeWhite(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1:                                                                                                                                                                                                       GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  12); break;
                case  2:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  13); break;
                case  3:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  14); break;
                case  4:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  15); break;
                case  5:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14);                                                                    break;
                case  6:                                                                                                                                                                                                       GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  17); break;
                case  7:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 16); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  18); break;
                case  8:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  19); break;
                case  9:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  20); break;
                case 10:                                                                                                                                     GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19);                                                                    break;
                case 11:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  2);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17,  22); break;
                case 12: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  1); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  3); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18,  23); break;
                case 13: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  2); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  4); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19,  24); break;
                case 14: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  3); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  5); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20,  25); break;
                case 15: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  4);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24);                                                                    break;
                case 16:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21,  27); break;
                case 17: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  6); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 26); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22,  28); break;
                case 18: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  7); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23,  29); break;
                case 19: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  8); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24,  30); break;
                case 20: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,  9);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29);                                                                    break;
                case 21:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27,  32); break;
                case 22: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 11); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28,  33); break;
                case 23: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 12); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29,  34); break;
                case 24: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 13); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 15); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30,  35); break;
                case 25: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 14);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34);                                                                    break;
                case 26:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31,  37); break;
                case 27: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 16); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 36); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32,  38); break;
                case 28: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 17); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33,  39); break;
                case 29: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 18); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34,  40); break;
                case 30: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 19);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39);                                                                    break;
                case 31:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37,  42); break;
                case 32: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 21); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38,  43); break;
                case 33: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 22); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39,  44); break;
                case 34: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 23); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 25); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40,  45); break;
                case 35: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 24);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44);                                                                    break;
                case 36:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 41,  47); break;
                case 37: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 26); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 46); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42,  48); break;
                case 38: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 27); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 47); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43,  49); break;
                case 39: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 28); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 48); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44,  50); break;
                case 40: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 29);                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 49);                                                                    break;
                case 41:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32);                                                                                                                                      break;
                case 42: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 31); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33);                                                                                                                                      break;
                case 43: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 32); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34);                                                                                                                                      break;
                case 44: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 33); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 35);                                                                                                                                      break;
                case 45: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 34);                                                                                                                                                                                                        break;
                case 46:                                                                   GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37);                                                                                                                                      break;
                case 47: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 36); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38);                                                                                                                                      break;
                case 48: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 37); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39);                                                                                                                                      break;
                case 49: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 38); GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40);                                                                                                                                      break;
                case 50: GetTakeWhite(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 39);                                                                                                                                                                                                        break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }
        }

        private void GetTakeWhite(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexTakes, int fieldIndexTo)
        {
            if (Fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (Fields[fieldIndexTakes])
                {
                    case FieldContentEnum.BlackMan:
                        fieldIndexEnd = fieldIndexTo;

                       Fields[fieldIndexTakes] = FieldContentEnum.BlackManTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;

                       _numberOfTakesInMove += 1;

                        GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       Fields[fieldIndexTakes] = FieldContentEnum.BlackMan;
                        break;
                    case FieldContentEnum.BlackKing:
                        fieldIndexEnd = fieldIndexTo;

                       Fields[fieldIndexTakes] = FieldContentEnum.BlackKingTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;

                       _numberOfTakesInMove += 1;

                        GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       Fields[fieldIndexTakes] = FieldContentEnum.BlackKing;
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

        private void GetTakeBlack(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom)
        {
            bool hadOne = false;

            switch (fieldIndexFrom)
            {
                case  1: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 12);                                                                                                                                                                                                        break;
                case  2: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 13); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7, 11);                                                                                                                                      break;
                case  3: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 14); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8, 12);                                                                                                                                      break;
                case  4: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 15); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9, 13);                                                                                                                                      break;
                case  5:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 10, 14);                                                                                                                                      break;
                case  6: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 17);                                                                                                                                                                                                        break;
                case  7: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 18); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11, 16);                                                                                                                                      break;
                case  8: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 19); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12, 17);                                                                                                                                      break;
                case  9: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 20); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13, 18);                                                                                                                                      break;
                case 10:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 19);                                                                                                                                      break;
                case 11: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 22);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,  2);                                                                    break;
                case 12: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 23); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 21); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,  3); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  7,   1); break;
                case 13: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 24); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 22); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,  4); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  8,   2); break;
                case 14: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 25); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 23); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,  5); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd,  9,   3); break;
                case 15:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 24);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 10,   4); break;
                case 16: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 27);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,  7);                                                                    break;
                case 17: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 28); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 26); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,  8); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 11,   6); break;
                case 18: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 29); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 27); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,  9); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 12,   7); break;
                case 19: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 30); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 28); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14, 10); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 13,   8); break;
                case 20:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 29);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 14,   9); break;
                case 21: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 32);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17, 12);                                                                    break;
                case 22: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 33); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 31); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18, 13); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 17,  11); break;
                case 23: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 34); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 32); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19, 14); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 18,  12); break;
                case 24: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 35); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 33); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20, 15); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 19,  13); break;
                case 25:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 34);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 20,  14); break;
                case 26: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 37);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21, 17);                                                                    break;
                case 27: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 38); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 36); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22, 18); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 21,  16); break;
                case 28: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 39); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 37); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23, 19); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 22,  17); break;
                case 29: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 40); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 38); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24, 20); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 23,  18); break;
                case 30:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 39);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 24,  19); break;
                case 31: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 42);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27, 22);                                                                    break;
                case 32: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 43); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 41); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28, 23); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 27,  21); break;
                case 33: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 44); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 42); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29, 24); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 28,  22); break;
                case 34: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 45); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 43); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30, 25); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 29,  23); break;
                case 35:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 44);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 30,  24); break;
                case 36: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 47);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31, 27);                                                                    break;
                case 37: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 48); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 46); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32, 28); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 31,  26); break;
                case 38: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 49); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 47); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33, 29); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 32,  27); break;
                case 39: GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 50); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 48); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34, 30); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 33,  28); break;
                case 40:                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 49);                                                                   GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 34,  29); break;
                case 41:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37, 32);                                                                    break;
                case 42:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38, 33); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 37,  31); break;
                case 43:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39, 34); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 38,  32); break;
                case 44:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40, 35); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 39,  33); break;
                case 45:                                                                                                                                                                                                       GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 40,  34); break;
                case 46:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 41, 37);                                                                    break;
                case 47:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42, 38); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 41,  36); break;
                case 48:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43, 39); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 42,  37); break;
                case 49:                                                                                                                                     GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44, 40); GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 43,  38); break;
                case 50:                                                                                                                                                                                                       GetTakeBlack(ref hadOne, fieldIndexStart, fieldIndexEnd, 44,  39); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }
        }

        private void GetTakeBlack(ref bool hadOne, int fieldIndexStart, int fieldIndexEnd, int fieldIndexTakes, int fieldIndexTo)
        {
            if (Fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (Fields[fieldIndexTakes])
                {
                    case FieldContentEnum.WhiteMan:
                        fieldIndexEnd = fieldIndexTo;

                       Fields[fieldIndexTakes] = FieldContentEnum.WhiteManTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;

                       _numberOfTakesInMove += 1;

                        GetTakeBlack(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       Fields[fieldIndexTakes] = FieldContentEnum.WhiteMan;
                        break;
                    case FieldContentEnum.WhiteKing:
                        fieldIndexEnd = fieldIndexTo;

                       Fields[fieldIndexTakes] = FieldContentEnum.WhiteKingTaken;

                       _takes [_numberOfTakesInMove] = fieldIndexTakes;

                       _numberOfTakesInMove += 1;

                        GetTakeBlack(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       Fields[fieldIndexTakes] = FieldContentEnum.WhiteKing;
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

        private void GetMoves()
        {
           _numberOfMoves = 0; // Defensive

            DateTime now = DateTime.Now;

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                if (Fields[ 6] == FieldContentEnum.WhiteMan) {                                                                                        if (Fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6,  1); }
                if (Fields[ 7] == FieldContentEnum.WhiteMan) { if (Fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  1); if (Fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  2); }
                if (Fields[ 8] == FieldContentEnum.WhiteMan) { if (Fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  2); if (Fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  3); }
                if (Fields[ 9] == FieldContentEnum.WhiteMan) { if (Fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  3); if (Fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  4); }
                if (Fields[10] == FieldContentEnum.WhiteMan) { if (Fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  4); if (Fields[ 5] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  5); }
                if (Fields[11] == FieldContentEnum.WhiteMan) { if (Fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  6); if (Fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  7); }
                if (Fields[12] == FieldContentEnum.WhiteMan) { if (Fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  7); if (Fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  8); }
                if (Fields[13] == FieldContentEnum.WhiteMan) { if (Fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  8); if (Fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  9); }
                if (Fields[14] == FieldContentEnum.WhiteMan) { if (Fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14,  9); if (Fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 10); }
                if (Fields[15] == FieldContentEnum.WhiteMan) { if (Fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 10);                                                                                        }
                if (Fields[16] == FieldContentEnum.WhiteMan) {                                                                                        if (Fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 11); }
                if (Fields[17] == FieldContentEnum.WhiteMan) { if (Fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 11); if (Fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 12); }
                if (Fields[18] == FieldContentEnum.WhiteMan) { if (Fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 12); if (Fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 13); }
                if (Fields[19] == FieldContentEnum.WhiteMan) { if (Fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 13); if (Fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 14); }
                if (Fields[20] == FieldContentEnum.WhiteMan) { if (Fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 14); if (Fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 15); }
                if (Fields[21] == FieldContentEnum.WhiteMan) { if (Fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 16); if (Fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 17); }
                if (Fields[22] == FieldContentEnum.WhiteMan) { if (Fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 17); if (Fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 18); }
                if (Fields[23] == FieldContentEnum.WhiteMan) { if (Fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 18); if (Fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 19); }
                if (Fields[24] == FieldContentEnum.WhiteMan) { if (Fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 19); if (Fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 20); }
                if (Fields[25] == FieldContentEnum.WhiteMan) { if (Fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 20);                                                                                        }
                if (Fields[26] == FieldContentEnum.WhiteMan) {                                                                                        if (Fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 21); }
                if (Fields[27] == FieldContentEnum.WhiteMan) { if (Fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 21); if (Fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 22); }
                if (Fields[28] == FieldContentEnum.WhiteMan) { if (Fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 22); if (Fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 23); }
                if (Fields[29] == FieldContentEnum.WhiteMan) { if (Fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 23); if (Fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 24); }
                if (Fields[30] == FieldContentEnum.WhiteMan) { if (Fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 24); if (Fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 25); }
                if (Fields[31] == FieldContentEnum.WhiteMan) { if (Fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 26); if (Fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 27); }
                if (Fields[32] == FieldContentEnum.WhiteMan) { if (Fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 27); if (Fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 28); }
                if (Fields[33] == FieldContentEnum.WhiteMan) { if (Fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 28); if (Fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 29); }
                if (Fields[34] == FieldContentEnum.WhiteMan) { if (Fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 29); if (Fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 30); }
                if (Fields[35] == FieldContentEnum.WhiteMan) { if (Fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 30);                                                                                        }
                if (Fields[36] == FieldContentEnum.WhiteMan) {                                                                                        if (Fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 31); }
                if (Fields[37] == FieldContentEnum.WhiteMan) { if (Fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 31); if (Fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 32); }
                if (Fields[38] == FieldContentEnum.WhiteMan) { if (Fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 32); if (Fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 33); }
                if (Fields[39] == FieldContentEnum.WhiteMan) { if (Fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 33); if (Fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 34); }
                if (Fields[40] == FieldContentEnum.WhiteMan) { if (Fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 34); if (Fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 35); }
                if (Fields[41] == FieldContentEnum.WhiteMan) { if (Fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 36); if (Fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 37); }
                if (Fields[42] == FieldContentEnum.WhiteMan) { if (Fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 37); if (Fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 38); }
                if (Fields[43] == FieldContentEnum.WhiteMan) { if (Fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 38); if (Fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 39); }
                if (Fields[44] == FieldContentEnum.WhiteMan) { if (Fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 39); if (Fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 40); }
                if (Fields[45] == FieldContentEnum.WhiteMan) { if (Fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 40);                                                                                        }
                if (Fields[46] == FieldContentEnum.WhiteMan) {                                                                                        if (Fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(46, 41); }
                if (Fields[47] == FieldContentEnum.WhiteMan) { if (Fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 41); if (Fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 42); }
                if (Fields[48] == FieldContentEnum.WhiteMan) { if (Fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 42); if (Fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 43); }
                if (Fields[49] == FieldContentEnum.WhiteMan) { if (Fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 43); if (Fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 44); }
                if (Fields[50] == FieldContentEnum.WhiteMan) { if (Fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 44); if (Fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 45); }
            }
            else
            {
                if (Fields[ 1] == FieldContentEnum.BlackMan) { if (Fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  7); if (Fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  6); }
                if (Fields[ 2] == FieldContentEnum.BlackMan) { if (Fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  8); if (Fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  7); }
                if (Fields[ 3] == FieldContentEnum.BlackMan) { if (Fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  9); if (Fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  8); }
                if (Fields[ 4] == FieldContentEnum.BlackMan) { if (Fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4, 10); if (Fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4,  9); }
                if (Fields[ 5] == FieldContentEnum.BlackMan) {                                                                                        if (Fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 5, 10); }
                if (Fields[ 6] == FieldContentEnum.BlackMan) { if (Fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6, 11);                                                                                        }
                if (Fields[ 7] == FieldContentEnum.BlackMan) { if (Fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 12); if (Fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 11); }
                if (Fields[ 8] == FieldContentEnum.BlackMan) { if (Fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 13); if (Fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 12); }
                if (Fields[ 9] == FieldContentEnum.BlackMan) { if (Fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 14); if (Fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 13); }
                if (Fields[10] == FieldContentEnum.BlackMan) { if (Fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 15); if (Fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 14); }
                if (Fields[11] == FieldContentEnum.BlackMan) { if (Fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 17); if (Fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 16); }
                if (Fields[12] == FieldContentEnum.BlackMan) { if (Fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 18); if (Fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 17); }
                if (Fields[13] == FieldContentEnum.BlackMan) { if (Fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 19); if (Fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 18); }
                if (Fields[14] == FieldContentEnum.BlackMan) { if (Fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 20); if (Fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 19); }
                if (Fields[15] == FieldContentEnum.BlackMan) {                                                                                        if (Fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 20); }
                if (Fields[16] == FieldContentEnum.BlackMan) { if (Fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 21);                                                                                        }
                if (Fields[17] == FieldContentEnum.BlackMan) { if (Fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 22); if (Fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 21); }
                if (Fields[18] == FieldContentEnum.BlackMan) { if (Fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 23); if (Fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 22); }
                if (Fields[19] == FieldContentEnum.BlackMan) { if (Fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 24); if (Fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 23); }
                if (Fields[20] == FieldContentEnum.BlackMan) { if (Fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 25); if (Fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 24); }
                if (Fields[21] == FieldContentEnum.BlackMan) { if (Fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 27); if (Fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 26); }
                if (Fields[22] == FieldContentEnum.BlackMan) { if (Fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 28); if (Fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 27); }
                if (Fields[23] == FieldContentEnum.BlackMan) { if (Fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 29); if (Fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 28); }
                if (Fields[24] == FieldContentEnum.BlackMan) { if (Fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 30); if (Fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 29); }
                if (Fields[25] == FieldContentEnum.BlackMan) {                                                                                        if (Fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 30); }
                if (Fields[26] == FieldContentEnum.BlackMan) { if (Fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 31);                                                                                        }
                if (Fields[27] == FieldContentEnum.BlackMan) { if (Fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 32); if (Fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 31); }
                if (Fields[28] == FieldContentEnum.BlackMan) { if (Fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 33); if (Fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 32); }
                if (Fields[29] == FieldContentEnum.BlackMan) { if (Fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 34); if (Fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 33); }
                if (Fields[30] == FieldContentEnum.BlackMan) { if (Fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 35); if (Fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 34); }
                if (Fields[31] == FieldContentEnum.BlackMan) { if (Fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 37); if (Fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 36); }
                if (Fields[32] == FieldContentEnum.BlackMan) { if (Fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 38); if (Fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 37); }
                if (Fields[33] == FieldContentEnum.BlackMan) { if (Fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 39); if (Fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 38); }
                if (Fields[34] == FieldContentEnum.BlackMan) { if (Fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 40); if (Fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 39); }
                if (Fields[35] == FieldContentEnum.BlackMan) {                                                                                        if (Fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 40); }
                if (Fields[36] == FieldContentEnum.BlackMan) { if (Fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 41);                                                                                        }
                if (Fields[37] == FieldContentEnum.BlackMan) { if (Fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 42); if (Fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 41); }
                if (Fields[38] == FieldContentEnum.BlackMan) { if (Fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 43); if (Fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 42); }
                if (Fields[39] == FieldContentEnum.BlackMan) { if (Fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 44); if (Fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 43); }
                if (Fields[40] == FieldContentEnum.BlackMan) { if (Fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 45); if (Fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 44); }
                if (Fields[41] == FieldContentEnum.BlackMan) { if (Fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 47); if (Fields[46] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 46); }
                if (Fields[42] == FieldContentEnum.BlackMan) { if (Fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 48); if (Fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 47); }
                if (Fields[43] == FieldContentEnum.BlackMan) { if (Fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 49); if (Fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 48); }
                if (Fields[44] == FieldContentEnum.BlackMan) { if (Fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 50); if (Fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 49); }
                if (Fields[45] == FieldContentEnum.BlackMan) {                                                                                        if (Fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 50); }
            }

#if DEBUG
            Debug.WriteLine((DateTime.Now - now).Milliseconds + " mSec");
#else
            MessageBox.Show((DateTime.Now - now).Milliseconds + " mSec");
#endif
        }

        #endregion

        #region Public methods

        public void Move(Move move)
        {
            int toFieldIndex = move.ToField;

           Fields[toFieldIndex  ] = Fields[move.FromField];
           Fields[move.FromField] = FieldContentEnum.Empty;

            if (move.TakeFields?.Count > 0)
            {
                foreach (int takeIndex in move.TakeFields)
                {
                   Fields[takeIndex] = FieldContentEnum.Empty;
                }
            }

            if (WhiteOrBlacksTurn == TurnEnum.White)
            {
                switch (toFieldIndex) { case  1: case  2: case  3: case  4: case  5: Fields[toFieldIndex] = FieldContentEnum.WhiteKing; break; }

                WhiteOrBlacksTurn = TurnEnum.Black;
            }
            else
            {
                switch (toFieldIndex) { case 46: case 47: case 48: case 49: case 50: Fields[toFieldIndex] = FieldContentEnum.BlackKing; break; }

                WhiteOrBlacksTurn = TurnEnum.White;
            }
        }

        #endregion
    }
}
