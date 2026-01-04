
namespace Check.Models
{
    public class Position
    {
        #region Enumerations

        public enum Field
        {
            Empty,
            WhitePiece,
            BlackPiece,
            WhiteRook,
            BlackRook
        }

        #endregion

        #region Fields

        private readonly Field[] _fields = new Field[51];

        #endregion

        #region Constructors

        public Position(bool startPosition = true)
        {
            if (startPosition)
            {
                for (int index =  1; index <= 20; index += 1) { _fields[index] = Field.BlackPiece; }
                for (int index = 21; index <= 30; index += 1) { _fields[index] = Field.Empty     ; }
                for (int index = 31; index <= 50; index += 1) { _fields[index] = Field.WhitePiece; }
            }
            else
            {
                for (int index =  1; index <= 50; index += 1) { _fields[index] = Field.Empty;      }
            }
        }

        #endregion

        #region Public properties

        public Field F01 { get => _fields[ 1]; set => _fields[ 1] = value; }
        public Field F02 { get => _fields[ 2]; set => _fields[ 2] = value; }
        public Field F03 { get => _fields[ 3]; set => _fields[ 3] = value; }
        public Field F04 { get => _fields[ 4]; set => _fields[ 4] = value; }
        public Field F05 { get => _fields[ 5]; set => _fields[ 5] = value; }
        public Field F06 { get => _fields[ 6]; set => _fields[ 6] = value; }
        public Field F07 { get => _fields[ 7]; set => _fields[ 7] = value; }
        public Field F08 { get => _fields[ 8]; set => _fields[ 8] = value; }
        public Field F09 { get => _fields[ 9]; set => _fields[ 9] = value; }
        public Field F10 { get => _fields[10]; set => _fields[10] = value; }
        public Field F11 { get => _fields[11]; set => _fields[11] = value; }
        public Field F12 { get => _fields[12]; set => _fields[12] = value; }
        public Field F13 { get => _fields[13]; set => _fields[13] = value; }
        public Field F14 { get => _fields[14]; set => _fields[14] = value; }
        public Field F15 { get => _fields[15]; set => _fields[15] = value; }
        public Field F16 { get => _fields[16]; set => _fields[16] = value; }
        public Field F17 { get => _fields[17]; set => _fields[17] = value; }
        public Field F18 { get => _fields[18]; set => _fields[18] = value; }
        public Field F19 { get => _fields[19]; set => _fields[19] = value; }
        public Field F20 { get => _fields[20]; set => _fields[20] = value; }
        public Field F21 { get => _fields[21]; set => _fields[21] = value; }
        public Field F22 { get => _fields[22]; set => _fields[22] = value; }
        public Field F23 { get => _fields[23]; set => _fields[23] = value; }
        public Field F24 { get => _fields[24]; set => _fields[24] = value; }
        public Field F25 { get => _fields[25]; set => _fields[25] = value; }
        public Field F26 { get => _fields[26]; set => _fields[26] = value; }
        public Field F27 { get => _fields[27]; set => _fields[27] = value; }
        public Field F28 { get => _fields[28]; set => _fields[28] = value; }
        public Field F29 { get => _fields[29]; set => _fields[29] = value; }
        public Field F30 { get => _fields[30]; set => _fields[30] = value; }
        public Field F31 { get => _fields[31]; set => _fields[31] = value; }
        public Field F32 { get => _fields[32]; set => _fields[32] = value; }
        public Field F33 { get => _fields[33]; set => _fields[33] = value; }
        public Field F34 { get => _fields[34]; set => _fields[34] = value; }
        public Field F35 { get => _fields[35]; set => _fields[35] = value; }
        public Field F36 { get => _fields[36]; set => _fields[36] = value; }
        public Field F37 { get => _fields[37]; set => _fields[37] = value; }
        public Field F38 { get => _fields[38]; set => _fields[38] = value; }
        public Field F39 { get => _fields[39]; set => _fields[39] = value; }
        public Field F40 { get => _fields[40]; set => _fields[40] = value; }
        public Field F41 { get => _fields[41]; set => _fields[41] = value; }
        public Field F42 { get => _fields[42]; set => _fields[42] = value; }
        public Field F43 { get => _fields[43]; set => _fields[43] = value; }
        public Field F44 { get => _fields[44]; set => _fields[44] = value; }
        public Field F45 { get => _fields[45]; set => _fields[45] = value; }
        public Field F46 { get => _fields[46]; set => _fields[46] = value; }
        public Field F47 { get => _fields[47]; set => _fields[47] = value; }
        public Field F48 { get => _fields[48]; set => _fields[48] = value; }
        public Field F49 { get => _fields[49]; set => _fields[49] = value; }
        public Field F50 { get => _fields[50]; set => _fields[50] = value; }

        #endregion
    }
}
