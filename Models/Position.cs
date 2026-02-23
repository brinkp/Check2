
using System.Collections.Generic;
using Check.ViewModels;

namespace Check.Models
{
    public class Position
    {
        #region Enumerations

        public enum FieldContent
        {
            Empty,
            WhitePiece,
            BlackPiece,
            WhiteRook,
            BlackRook
        }

        #endregion

        #region Fields

        private readonly FieldContent[] _fields = new FieldContent[51];

        #endregion

        #region Constructors

        public Position(bool startPosition = true)
        {
            if (startPosition)
            {
                for (int index =  1; index <= 20; index += 1) { _fields[index] = FieldContent.BlackPiece; }
                for (int index = 21; index <= 30; index += 1) { _fields[index] = FieldContent.Empty     ; }
                for (int index = 31; index <= 50; index += 1) { _fields[index] = FieldContent.WhitePiece; }
            }
            else
            {
                //for (int index =  1; index <= 50; index += 1) { _fields[index] = Field.Empty;      }

                _fields[ 1] = FieldContent.BlackPiece; _fields[ 2] = FieldContent.Empty     ; _fields[ 3] = FieldContent.Empty     ; _fields[ 4] = FieldContent.Empty     ; _fields[ 5] = FieldContent.Empty     ;
                _fields[ 6] = FieldContent.Empty     ; _fields[ 7] = FieldContent.Empty     ; _fields[ 8] = FieldContent.BlackPiece; _fields[ 9] = FieldContent.BlackPiece; _fields[10] = FieldContent.BlackPiece;
                _fields[11] = FieldContent.WhitePiece; _fields[12] = FieldContent.Empty     ; _fields[13] = FieldContent.BlackPiece; _fields[14] = FieldContent.Empty     ; _fields[15] = FieldContent.Empty     ;
                _fields[16] = FieldContent.Empty     ; _fields[17] = FieldContent.WhitePiece; _fields[18] = FieldContent.Empty     ; _fields[19] = FieldContent.BlackPiece; _fields[20] = FieldContent.BlackPiece;
                _fields[21] = FieldContent.WhitePiece; _fields[22] = FieldContent.WhitePiece; _fields[23] = FieldContent.BlackPiece; _fields[24] = FieldContent.BlackPiece; _fields[25] = FieldContent.Empty     ;
                _fields[26] = FieldContent.Empty     ; _fields[27] = FieldContent.WhitePiece; _fields[28] = FieldContent.BlackPiece; _fields[29] = FieldContent.BlackPiece; _fields[30] = FieldContent.WhitePiece;
                _fields[31] = FieldContent.Empty     ; _fields[32] = FieldContent.Empty     ; _fields[33] = FieldContent.BlackPiece; _fields[34] = FieldContent.WhitePiece; _fields[35] = FieldContent.BlackPiece;
                _fields[36] = FieldContent.Empty     ; _fields[37] = FieldContent.WhitePiece; _fields[38] = FieldContent.WhitePiece; _fields[39] = FieldContent.WhitePiece; _fields[40] = FieldContent.WhitePiece;
                _fields[41] = FieldContent.Empty     ; _fields[42] = FieldContent.WhitePiece; _fields[43] = FieldContent.WhitePiece; _fields[44] = FieldContent.WhitePiece; _fields[45] = FieldContent.BlackPiece;
                _fields[46] = FieldContent.Empty     ; _fields[47] = FieldContent.Empty     ; _fields[48] = FieldContent.Empty     ; _fields[49] = FieldContent.Empty     ; _fields[50] = FieldContent.Empty     ;
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

        public FieldContent F01 { get => _fields[ 1]; set => _fields[ 1] = value; }
        public FieldContent F02 { get => _fields[ 2]; set => _fields[ 2] = value; }
        public FieldContent F03 { get => _fields[ 3]; set => _fields[ 3] = value; }
        public FieldContent F04 { get => _fields[ 4]; set => _fields[ 4] = value; }
        public FieldContent F05 { get => _fields[ 5]; set => _fields[ 5] = value; }
        public FieldContent F06 { get => _fields[ 6]; set => _fields[ 6] = value; }
        public FieldContent F07 { get => _fields[ 7]; set => _fields[ 7] = value; }
        public FieldContent F08 { get => _fields[ 8]; set => _fields[ 8] = value; }
        public FieldContent F09 { get => _fields[ 9]; set => _fields[ 9] = value; }
        public FieldContent F10 { get => _fields[10]; set => _fields[10] = value; }
        public FieldContent F11 { get => _fields[11]; set => _fields[11] = value; }
        public FieldContent F12 { get => _fields[12]; set => _fields[12] = value; }
        public FieldContent F13 { get => _fields[13]; set => _fields[13] = value; }
        public FieldContent F14 { get => _fields[14]; set => _fields[14] = value; }
        public FieldContent F15 { get => _fields[15]; set => _fields[15] = value; }
        public FieldContent F16 { get => _fields[16]; set => _fields[16] = value; }
        public FieldContent F17 { get => _fields[17]; set => _fields[17] = value; }
        public FieldContent F18 { get => _fields[18]; set => _fields[18] = value; }
        public FieldContent F19 { get => _fields[19]; set => _fields[19] = value; }
        public FieldContent F20 { get => _fields[20]; set => _fields[20] = value; }
        public FieldContent F21 { get => _fields[21]; set => _fields[21] = value; }
        public FieldContent F22 { get => _fields[22]; set => _fields[22] = value; }
        public FieldContent F23 { get => _fields[23]; set => _fields[23] = value; }
        public FieldContent F24 { get => _fields[24]; set => _fields[24] = value; }
        public FieldContent F25 { get => _fields[25]; set => _fields[25] = value; }
        public FieldContent F26 { get => _fields[26]; set => _fields[26] = value; }
        public FieldContent F27 { get => _fields[27]; set => _fields[27] = value; }
        public FieldContent F28 { get => _fields[28]; set => _fields[28] = value; }
        public FieldContent F29 { get => _fields[29]; set => _fields[29] = value; }
        public FieldContent F30 { get => _fields[30]; set => _fields[30] = value; }
        public FieldContent F31 { get => _fields[31]; set => _fields[31] = value; }
        public FieldContent F32 { get => _fields[32]; set => _fields[32] = value; }
        public FieldContent F33 { get => _fields[33]; set => _fields[33] = value; }
        public FieldContent F34 { get => _fields[34]; set => _fields[34] = value; }
        public FieldContent F35 { get => _fields[35]; set => _fields[35] = value; }
        public FieldContent F36 { get => _fields[36]; set => _fields[36] = value; }
        public FieldContent F37 { get => _fields[37]; set => _fields[37] = value; }
        public FieldContent F38 { get => _fields[38]; set => _fields[38] = value; }
        public FieldContent F39 { get => _fields[39]; set => _fields[39] = value; }
        public FieldContent F40 { get => _fields[40]; set => _fields[40] = value; }
        public FieldContent F41 { get => _fields[41]; set => _fields[41] = value; }
        public FieldContent F42 { get => _fields[42]; set => _fields[42] = value; }
        public FieldContent F43 { get => _fields[43]; set => _fields[43] = value; }
        public FieldContent F44 { get => _fields[44]; set => _fields[44] = value; }
        public FieldContent F45 { get => _fields[45]; set => _fields[45] = value; }
        public FieldContent F46 { get => _fields[46]; set => _fields[46] = value; }
        public FieldContent F47 { get => _fields[47]; set => _fields[47] = value; }
        public FieldContent F48 { get => _fields[48]; set => _fields[48] = value; }
        public FieldContent F49 { get => _fields[49]; set => _fields[49] = value; }
        public FieldContent F50 { get => _fields[50]; set => _fields[50] = value; }

        #endregion

        #region Public properties

        public List<Move> PossibleMoves { get; set; }

        #endregion
    }
}
