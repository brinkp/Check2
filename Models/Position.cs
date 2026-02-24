using System.Collections.Generic;
using Check.ViewModels;

namespace Check.Models
{
    internal class Position
    {
        #region Enumerations

        public enum FieldContentEnum
        {
            Empty,
            WhitePiece,
            BlackPiece,
            WhiteRook,
            BlackRook
        }

        #endregion

        #region Fields

        private readonly FieldContentEnum[] _fields = new FieldContentEnum[51];

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
                _fields[11] = FieldContentEnum.WhitePiece; _fields[12] = FieldContentEnum.Empty     ; _fields[13] = FieldContentEnum.BlackPiece; _fields[14] = FieldContentEnum.Empty     ; _fields[15] = FieldContentEnum.Empty     ;
                _fields[16] = FieldContentEnum.Empty     ; _fields[17] = FieldContentEnum.WhitePiece; _fields[18] = FieldContentEnum.Empty     ; _fields[19] = FieldContentEnum.BlackPiece; _fields[20] = FieldContentEnum.BlackPiece;
                _fields[21] = FieldContentEnum.WhitePiece; _fields[22] = FieldContentEnum.WhitePiece; _fields[23] = FieldContentEnum.BlackPiece; _fields[24] = FieldContentEnum.BlackPiece; _fields[25] = FieldContentEnum.Empty     ;
                _fields[26] = FieldContentEnum.Empty     ; _fields[27] = FieldContentEnum.WhitePiece; _fields[28] = FieldContentEnum.BlackPiece; _fields[29] = FieldContentEnum.BlackPiece; _fields[30] = FieldContentEnum.WhitePiece;
                _fields[31] = FieldContentEnum.Empty     ; _fields[32] = FieldContentEnum.Empty     ; _fields[33] = FieldContentEnum.BlackPiece; _fields[34] = FieldContentEnum.WhitePiece; _fields[35] = FieldContentEnum.BlackPiece;
                _fields[36] = FieldContentEnum.Empty     ; _fields[37] = FieldContentEnum.WhitePiece; _fields[38] = FieldContentEnum.WhitePiece; _fields[39] = FieldContentEnum.WhitePiece; _fields[40] = FieldContentEnum.WhitePiece;
                _fields[41] = FieldContentEnum.Empty     ; _fields[42] = FieldContentEnum.WhitePiece; _fields[43] = FieldContentEnum.WhitePiece; _fields[44] = FieldContentEnum.WhitePiece; _fields[45] = FieldContentEnum.BlackPiece;
                _fields[46] = FieldContentEnum.Empty     ; _fields[47] = FieldContentEnum.Empty     ; _fields[48] = FieldContentEnum.Empty     ; _fields[49] = FieldContentEnum.Empty     ; _fields[50] = FieldContentEnum.Empty     ;
            }

            PossibleMoves = new List<Move>
            {
                new Move(11,  6),
                new Move(11,  7),
                new Move(17, 12),
                new Move(21, 16),
                new Move(22, 18),
                new Move(30, 25),
                new Move(37, 31),
                new Move(37, 32),
                new Move(38, 32)
            };
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

        public List<Move> PossibleMoves { get; set; }

        #endregion
    }
}
