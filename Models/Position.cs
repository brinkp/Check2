using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Check.ViewModels;
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
            WhitesTurn,
            BlacksTurn
        }

        public enum FieldContentEnum
        {
            Empty          ,
            WhitePiece     ,
            BlackPiece     ,
            WhiteRook      ,
            BlackRook      ,
            WhitePieceTaken,
            BlackPieceTaken,
            WhiteRookTaken ,
            BlackRookTaken
        }

        #endregion

        #region Fields

        private TurnEnum _whiteOrBlacksTurn;

        private      int _numberOfMoves         ;
        private      int _numberOfTakesInMove   ;
      //private      int _numberOfTakesInMoveMax;

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
                for (int index =  1; index <= 20; index += 1) { _fields[index] = FieldContentEnum.BlackPiece; }
                for (int index = 21; index <= 30; index += 1) { _fields[index] = FieldContentEnum.Empty     ; }
                for (int index = 31; index <= 50; index += 1) { _fields[index] = FieldContentEnum.WhitePiece; }
            }
            else
            {
              //for (int index =  1; index <= 50; index += 1) { _fields[index] = FieldContentEnum.Empty;      }

                _fields[ 1] = FieldContentEnum.BlackPiece; _fields[ 2] = FieldContentEnum.Empty     ; _fields[ 3] = FieldContentEnum.Empty     ; _fields[ 4] = FieldContentEnum.Empty     ; _fields[ 5] = FieldContentEnum.Empty     ;
                _fields[ 6] = FieldContentEnum.Empty     ; _fields[ 7] = FieldContentEnum.Empty     ; _fields[ 8] = FieldContentEnum.BlackPiece; _fields[ 9] = FieldContentEnum.BlackPiece; _fields[10] = FieldContentEnum.BlackPiece;
                _fields[11] = FieldContentEnum.WhitePiece; _fields[12] = FieldContentEnum.Empty     ; _fields[13] = FieldContentEnum.Empty     ; _fields[14] = FieldContentEnum.Empty     ; _fields[15] = FieldContentEnum.Empty     ;
                _fields[16] = FieldContentEnum.Empty     ; _fields[17] = FieldContentEnum.WhitePiece; _fields[18] = FieldContentEnum.BlackPiece; _fields[19] = FieldContentEnum.BlackPiece; _fields[20] = FieldContentEnum.BlackPiece;
                _fields[21] = FieldContentEnum.WhitePiece; _fields[22] = FieldContentEnum.WhitePiece; _fields[23] = FieldContentEnum.BlackPiece; _fields[24] = FieldContentEnum.Empty     ; _fields[25] = FieldContentEnum.Empty     ;
                _fields[26] = FieldContentEnum.Empty     ; _fields[27] = FieldContentEnum.WhitePiece; _fields[28] = FieldContentEnum.BlackPiece; _fields[29] = FieldContentEnum.BlackPiece; _fields[30] = FieldContentEnum.WhitePiece;
                _fields[31] = FieldContentEnum.Empty     ; _fields[32] = FieldContentEnum.Empty     ; _fields[33] = FieldContentEnum.BlackPiece; _fields[34] = FieldContentEnum.WhitePiece; _fields[35] = FieldContentEnum.BlackPiece;
                _fields[36] = FieldContentEnum.Empty     ; _fields[37] = FieldContentEnum.WhitePiece; _fields[38] = FieldContentEnum.WhitePiece; _fields[39] = FieldContentEnum.WhitePiece; _fields[40] = FieldContentEnum.WhitePiece;
                _fields[41] = FieldContentEnum.Empty     ; _fields[42] = FieldContentEnum.WhitePiece; _fields[43] = FieldContentEnum.WhitePiece; _fields[44] = FieldContentEnum.WhitePiece; _fields[45] = FieldContentEnum.BlackPiece;
                _fields[46] = FieldContentEnum.Empty     ; _fields[47] = FieldContentEnum.Empty     ; _fields[48] = FieldContentEnum.Empty     ; _fields[49] = FieldContentEnum.Empty     ; _fields[50] = FieldContentEnum.Empty     ;
            }

            WhiteOrBlacksTurn = TurnEnum.WhitesTurn;
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
         //_numberOfTakesInMoveMax = 0;

            DateTime now = DateTime.Now;

            if (WhiteOrBlacksTurn == TurnEnum.WhitesTurn) { for (int fromFieldIndex = 6; fromFieldIndex <= 50; fromFieldIndex += 1) { if (_fields[fromFieldIndex] == FieldContentEnum.WhitePiece) GetTakeWhite(fromFieldIndex, 0, fromFieldIndex); } }
            else                                          { for (int fromFieldIndex = 1; fromFieldIndex <= 45; fromFieldIndex += 1) { if (_fields[fromFieldIndex] == FieldContentEnum.BlackPiece) GetTakeWhite(fromFieldIndex, 0, fromFieldIndex); } }

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
            switch (fieldIndexFrom)
            {
                case  1: break;
                case  2:                                                                                        GetTakeWhite(fieldIndexStart, fieldIndexEnd,  2,  7, 11); GetTakeWhite(fieldIndexStart, fieldIndexEnd,  2,  8,  13); break;
                case  3: break;
                case  4:                                                                                        GetTakeWhite(fieldIndexStart, fieldIndexEnd,  4,  9, 13); GetTakeWhite(fieldIndexStart, fieldIndexEnd,  4, 10,  15); break;
                case  5: break;
                case  6: break;
                case  7: break;
                case  8: break;
                case  9: break;
                case 10: break;
                case 11: break;
                case 12: GetTakeWhite(fieldIndexStart, fieldIndexEnd, 12,  7,  1); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 12,  8,   3); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 12, 17, 21); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 12, 18,  23); break;
                case 13: GetTakeWhite(fieldIndexStart, fieldIndexEnd, 13,  8,  2); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 13,  9,   4); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 13, 18, 22); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 13, 19,  24); break;
                case 14: break;
                case 15: GetTakeWhite(fieldIndexStart, fieldIndexEnd, 15, 10,  4);                                                            GetTakeWhite(fieldIndexStart, fieldIndexEnd, 15, 20, 24);                                                            break;
                case 16: break;
                case 17: break;
                case 18: break;
                case 19: break;
                case 20: break;
                case 21: break;
                case 22: GetTakeWhite(fieldIndexStart, fieldIndexEnd, 22, 17, 11); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 22, 18, 13); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 22, 27, 31); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 22, 28,  33); break; 
                case 23: break;
                case 24: GetTakeWhite(fieldIndexStart, fieldIndexEnd, 24, 19, 13); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 24, 10, 15); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 24, 29, 33); GetTakeWhite(fieldIndexStart, fieldIndexEnd, 24, 30,  35); break;
                case 25: break;
                case 26: break;
                case 27: break;
                case 28: break;
                case 29: break;
                case 30: break;
                case 31: break;
                case 32: break;
                case 33: break;
                case 34: break;
                case 35: break;
                case 36: break;
                case 37: break;
                case 38: break;
                case 39: break;
                case 40: break;
                case 41: break;
                case 42: break;
                case 43: break;
                case 44: break;
                case 45: break;
                case 46: break;
                case 47: break;
                case 48: break;
                case 49: break;
                case 50: break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldIndexFrom), "Invalid switch value");
            }
        }

        private void GetTakeWhite(int fieldIndexStart, int fieldIndexEnd, int fieldIndexFrom, int fieldIndexVia, int fieldIndexTo)
        {
            if (_fields[fieldIndexTo] == FieldContentEnum.Empty)
            {
                switch (_fields[fieldIndexVia])
                {
                    case FieldContentEnum.BlackPiece:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexFrom]  = FieldContentEnum.Empty          ;
                       _fields[fieldIndexVia ]  = FieldContentEnum.BlackPieceTaken;

                       _vias  [_numberOfTakesInMove]  = fieldIndexVia;
                       _takes [_numberOfTakesInMove]  = fieldIndexTo;

                       _numberOfTakesInMove += 1;

                        GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexVia]   = FieldContentEnum.BlackPiece;
                       _fields[fieldIndexFrom]  = FieldContentEnum.WhitePiece;
                        break;
                    case FieldContentEnum.BlackRook:
                        fieldIndexEnd = fieldIndexTo;

                       _fields[fieldIndexFrom]  = FieldContentEnum.Empty         ;
                       _fields[fieldIndexVia ]  = FieldContentEnum.BlackRookTaken;

                       _vias  [_numberOfTakesInMove]  = fieldIndexVia;
                       _takes [_numberOfTakesInMove]  = fieldIndexTo;

                       _numberOfTakesInMove += 1;

                        GetTakeWhite(fieldIndexStart, fieldIndexEnd, fieldIndexTo);

                       _numberOfTakesInMove -= 1;

                       _fields[fieldIndexVia]   = FieldContentEnum.BlackRook ;
                       _fields[fieldIndexFrom]  = FieldContentEnum.WhitePiece;
                        break;
                    default:
                    {
                        if (_numberOfTakesInMove > 0)
                        {
                          //if (_numberOfTakesInMoveMax <= _numberOfTakesInMove)
                          //{
                          //    _numberOfTakesInMoveMax  = _numberOfTakesInMove;

                                _moves[_numberOfMoves++] = new Move(fieldIndexStart, fieldIndexEnd, _numberOfTakesInMove, _takes, _vias);
                          //}
                        }
                        break;
                    }
                }
            }
        }

        private void GetMoves()
        {
           _numberOfMoves = 0; // Defensive

            DateTime now = DateTime.Now;

            if (WhiteOrBlacksTurn == TurnEnum.WhitesTurn)
            {
                if (_fields[ 6] == FieldContentEnum.WhitePiece) {                                                                                         if (_fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6,  1); }
                if (_fields[ 7] == FieldContentEnum.WhitePiece) { if (_fields[ 1] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  1); if (_fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7,  2); }
                if (_fields[ 8] == FieldContentEnum.WhitePiece) { if (_fields[ 2] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  2); if (_fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8,  3); }
                if (_fields[ 9] == FieldContentEnum.WhitePiece) { if (_fields[ 3] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  3); if (_fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9,  4); }
                if (_fields[10] == FieldContentEnum.WhitePiece) { if (_fields[ 4] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  4); if (_fields[ 5] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10,  5); }
                if (_fields[11] == FieldContentEnum.WhitePiece) { if (_fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  6); if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11,  7); }
                if (_fields[12] == FieldContentEnum.WhitePiece) { if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  7); if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12,  8); }
                if (_fields[13] == FieldContentEnum.WhitePiece) { if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  8); if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13,  9); }
                if (_fields[14] == FieldContentEnum.WhitePiece) { if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14,  9); if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 10); }
                if (_fields[15] == FieldContentEnum.WhitePiece) { if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 10);                                                                                         }
                if (_fields[16] == FieldContentEnum.WhitePiece) {                                                                                         if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 11); }
                if (_fields[17] == FieldContentEnum.WhitePiece) { if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 11); if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 12); }
                if (_fields[18] == FieldContentEnum.WhitePiece) { if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 12); if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 13); }
                if (_fields[19] == FieldContentEnum.WhitePiece) { if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 13); if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 14); }
                if (_fields[20] == FieldContentEnum.WhitePiece) { if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 14); if (_fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 15); }
                if (_fields[21] == FieldContentEnum.WhitePiece) { if (_fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 16); if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 17); }
                if (_fields[22] == FieldContentEnum.WhitePiece) { if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 17); if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 18); }
                if (_fields[23] == FieldContentEnum.WhitePiece) { if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 18); if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 19); }
                if (_fields[24] == FieldContentEnum.WhitePiece) { if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 19); if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 20); }
                if (_fields[25] == FieldContentEnum.WhitePiece) { if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 20);                                                                                         }
                if (_fields[26] == FieldContentEnum.WhitePiece) {                                                                                         if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 21); }
                if (_fields[27] == FieldContentEnum.WhitePiece) { if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 21); if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 22); }
                if (_fields[28] == FieldContentEnum.WhitePiece) { if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 22); if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 23); }
                if (_fields[29] == FieldContentEnum.WhitePiece) { if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 23); if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 24); }
                if (_fields[30] == FieldContentEnum.WhitePiece) { if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 24); if (_fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 25); }
                if (_fields[31] == FieldContentEnum.WhitePiece) { if (_fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 26); if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 27); }
                if (_fields[32] == FieldContentEnum.WhitePiece) { if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 27); if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 28); }
                if (_fields[33] == FieldContentEnum.WhitePiece) { if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 28); if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 29); }
                if (_fields[34] == FieldContentEnum.WhitePiece) { if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 29); if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 30); }
                if (_fields[35] == FieldContentEnum.WhitePiece) { if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 30);                                                                                         }
                if (_fields[36] == FieldContentEnum.WhitePiece) {                                                                                         if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 31); }
                if (_fields[37] == FieldContentEnum.WhitePiece) { if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 31); if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 32); }
                if (_fields[38] == FieldContentEnum.WhitePiece) { if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 32); if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 33); }
                if (_fields[39] == FieldContentEnum.WhitePiece) { if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 33); if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 34); }
                if (_fields[40] == FieldContentEnum.WhitePiece) { if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 34); if (_fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 35); }
                if (_fields[41] == FieldContentEnum.WhitePiece) { if (_fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 36); if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 37); }
                if (_fields[42] == FieldContentEnum.WhitePiece) { if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 37); if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 38); }
                if (_fields[43] == FieldContentEnum.WhitePiece) { if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 38); if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 39); }
                if (_fields[44] == FieldContentEnum.WhitePiece) { if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 39); if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 40); }
                if (_fields[45] == FieldContentEnum.WhitePiece) { if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 40);                                                                                         }
                if (_fields[46] == FieldContentEnum.WhitePiece) {                                                                                         if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(46, 41); }
                if (_fields[47] == FieldContentEnum.WhitePiece) { if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 41); if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(47, 42); }
                if (_fields[48] == FieldContentEnum.WhitePiece) { if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 42); if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(48, 43); }
                if (_fields[49] == FieldContentEnum.WhitePiece) { if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 43); if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(49, 44); }
                if (_fields[50] == FieldContentEnum.WhitePiece) { if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 44); if (_fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(50, 45); }
            }
            else
            {
                if (_fields[ 1] == FieldContentEnum.BlackPiece) { if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  7); if (_fields[ 6] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 1,  6); }
                if (_fields[ 2] == FieldContentEnum.BlackPiece) { if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  8); if (_fields[ 7] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 2,  7); }
                if (_fields[ 3] == FieldContentEnum.BlackPiece) { if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  9); if (_fields[ 8] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 3,  8); }
                if (_fields[ 4] == FieldContentEnum.BlackPiece) { if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4, 10); if (_fields[ 9] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 4,  9); }
                if (_fields[ 5] == FieldContentEnum.BlackPiece) {                                                                                         if (_fields[10] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 5, 10); }
                if (_fields[ 6] == FieldContentEnum.BlackPiece) { if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 6, 11);                                                                                         }
                if (_fields[ 7] == FieldContentEnum.BlackPiece) { if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 12); if (_fields[11] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 7, 11); }
                if (_fields[ 8] == FieldContentEnum.BlackPiece) { if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 13); if (_fields[12] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 8, 12); }
                if (_fields[ 9] == FieldContentEnum.BlackPiece) { if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 14); if (_fields[13] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move( 9, 13); }
                if (_fields[10] == FieldContentEnum.BlackPiece) { if (_fields[15] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 15); if (_fields[14] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(10, 14); }
                if (_fields[11] == FieldContentEnum.BlackPiece) { if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 17); if (_fields[16] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(11, 16); }
                if (_fields[12] == FieldContentEnum.BlackPiece) { if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 18); if (_fields[17] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(12, 17); }
                if (_fields[13] == FieldContentEnum.BlackPiece) { if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 19); if (_fields[18] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(13, 18); }
                if (_fields[14] == FieldContentEnum.BlackPiece) { if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 20); if (_fields[19] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(14, 19); }
                if (_fields[15] == FieldContentEnum.BlackPiece) {                                                                                         if (_fields[20] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(15, 20); }
                if (_fields[16] == FieldContentEnum.BlackPiece) { if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(16, 21);                                                                                         }
                if (_fields[17] == FieldContentEnum.BlackPiece) { if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 22); if (_fields[21] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(17, 21); }
                if (_fields[18] == FieldContentEnum.BlackPiece) { if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 23); if (_fields[22] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(18, 22); }
                if (_fields[19] == FieldContentEnum.BlackPiece) { if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 24); if (_fields[23] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(19, 23); }
                if (_fields[20] == FieldContentEnum.BlackPiece) { if (_fields[25] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 25); if (_fields[24] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(20, 24); }
                if (_fields[21] == FieldContentEnum.BlackPiece) { if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 27); if (_fields[26] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(21, 26); }
                if (_fields[22] == FieldContentEnum.BlackPiece) { if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 28); if (_fields[27] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(22, 27); }
                if (_fields[23] == FieldContentEnum.BlackPiece) { if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 29); if (_fields[28] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(23, 28); }
                if (_fields[24] == FieldContentEnum.BlackPiece) { if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 30); if (_fields[29] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(24, 29); }
                if (_fields[25] == FieldContentEnum.BlackPiece) {                                                                                         if (_fields[30] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(25, 30); }
                if (_fields[26] == FieldContentEnum.BlackPiece) { if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(26, 31);                                                                                         }
                if (_fields[27] == FieldContentEnum.BlackPiece) { if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 32); if (_fields[31] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(27, 31); }
                if (_fields[28] == FieldContentEnum.BlackPiece) { if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 33); if (_fields[32] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(28, 32); }
                if (_fields[29] == FieldContentEnum.BlackPiece) { if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 34); if (_fields[33] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(29, 33); }
                if (_fields[30] == FieldContentEnum.BlackPiece) { if (_fields[35] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 35); if (_fields[34] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(30, 34); }
                if (_fields[31] == FieldContentEnum.BlackPiece) { if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 37); if (_fields[36] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(31, 36); }
                if (_fields[32] == FieldContentEnum.BlackPiece) { if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 38); if (_fields[37] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(32, 37); }
                if (_fields[33] == FieldContentEnum.BlackPiece) { if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 39); if (_fields[38] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(33, 38); }
                if (_fields[34] == FieldContentEnum.BlackPiece) { if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 40); if (_fields[39] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(34, 39); }
                if (_fields[35] == FieldContentEnum.BlackPiece) {                                                                                         if (_fields[40] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(35, 40); }
                if (_fields[36] == FieldContentEnum.BlackPiece) { if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(36, 41);                                                                                         }
                if (_fields[37] == FieldContentEnum.BlackPiece) { if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 42); if (_fields[41] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(37, 41); }
                if (_fields[38] == FieldContentEnum.BlackPiece) { if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 43); if (_fields[42] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(38, 42); }
                if (_fields[39] == FieldContentEnum.BlackPiece) { if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 44); if (_fields[43] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(39, 43); }
                if (_fields[40] == FieldContentEnum.BlackPiece) { if (_fields[45] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 45); if (_fields[44] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(40, 44); }
                if (_fields[41] == FieldContentEnum.BlackPiece) { if (_fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 47); if (_fields[46] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(41, 46); }
                if (_fields[42] == FieldContentEnum.BlackPiece) { if (_fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 48); if (_fields[47] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(42, 47); }
                if (_fields[43] == FieldContentEnum.BlackPiece) { if (_fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 49); if (_fields[48] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(43, 48); }
                if (_fields[44] == FieldContentEnum.BlackPiece) { if (_fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 50); if (_fields[49] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(44, 49); }
                if (_fields[45] == FieldContentEnum.BlackPiece) {                                                                                         if (_fields[50] == FieldContentEnum.Empty) _moves[_numberOfMoves++] = new Move(45, 50); }
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

           WhiteOrBlacksTurn = (WhiteOrBlacksTurn == TurnEnum.WhitesTurn) ? TurnEnum.BlacksTurn : TurnEnum.WhitesTurn;
        }
    }
}
