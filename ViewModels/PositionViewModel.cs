using Check.Models;
// ReSharper disable InvertIf
// ReSharper disable LocalizableElement

namespace Check.ViewModels
{
    internal class PositionViewModel : BaseViewModel
    {
        private readonly Position _position;

        #region ENumerations

        public enum PositionStatusEnum
        {
            Default,
            MoveStarted,
            TakeInProgress
        }

        #endregion

        #region Constructors

        public PositionViewModel(Position position)
        {
            if (position == null) System.Diagnostics.Debugger.Break();

           _position = position;
        }

        #endregion

        #region Public properties

        public Position Position => _position;

        public PositionStatusEnum PositionStatus { get; set; } = PositionStatusEnum.Default;

        //public Position.FieldContentEnum F01 { get => _position.F01; set { if (_position.F01 != value) { _position.F01 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F02 { get => _position.F02; set { if (_position.F02 != value) { _position.F02 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F03 { get => _position.F03; set { if (_position.F03 != value) { _position.F03 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F04 { get => _position.F04; set { if (_position.F04 != value) { _position.F04 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F05 { get => _position.F05; set { if (_position.F05 != value) { _position.F05 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F06 { get => _position.F06; set { if (_position.F06 != value) { _position.F06 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F07 { get => _position.F07; set { if (_position.F07 != value) { _position.F07 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F08 { get => _position.F08; set { if (_position.F08 != value) { _position.F08 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F09 { get => _position.F09; set { if (_position.F09 != value) { _position.F09 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F10 { get => _position.F10; set { if (_position.F10 != value) { _position.F10 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F11 { get => _position.F11; set { if (_position.F11 != value) { _position.F11 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F12 { get => _position.F12; set { if (_position.F12 != value) { _position.F12 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F13 { get => _position.F13; set { if (_position.F13 != value) { _position.F13 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F14 { get => _position.F14; set { if (_position.F14 != value) { _position.F14 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F15 { get => _position.F15; set { if (_position.F15 != value) { _position.F15 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F16 { get => _position.F16; set { if (_position.F16 != value) { _position.F16 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F17 { get => _position.F17; set { if (_position.F17 != value) { _position.F17 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F18 { get => _position.F18; set { if (_position.F18 != value) { _position.F18 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F19 { get => _position.F19; set { if (_position.F19 != value) { _position.F19 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F20 { get => _position.F20; set { if (_position.F20 != value) { _position.F20 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F21 { get => _position.F21; set { if (_position.F21 != value) { _position.F21 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F22 { get => _position.F22; set { if (_position.F22 != value) { _position.F22 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F23 { get => _position.F23; set { if (_position.F23 != value) { _position.F23 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F24 { get => _position.F24; set { if (_position.F24 != value) { _position.F24 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F25 { get => _position.F25; set { if (_position.F25 != value) { _position.F25 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F26 { get => _position.F26; set { if (_position.F26 != value) { _position.F26 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F27 { get => _position.F27; set { if (_position.F27 != value) { _position.F27 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F28 { get => _position.F28; set { if (_position.F28 != value) { _position.F28 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F29 { get => _position.F29; set { if (_position.F29 != value) { _position.F29 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F30 { get => _position.F30; set { if (_position.F30 != value) { _position.F30 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F31 { get => _position.F31; set { if (_position.F31 != value) { _position.F31 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F32 { get => _position.F32; set { if (_position.F32 != value) { _position.F32 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F33 { get => _position.F33; set { if (_position.F33 != value) { _position.F33 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F34 { get => _position.F34; set { if (_position.F34 != value) { _position.F34 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F35 { get => _position.F35; set { if (_position.F35 != value) { _position.F35 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F36 { get => _position.F36; set { if (_position.F36 != value) { _position.F36 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F37 { get => _position.F37; set { if (_position.F37 != value) { _position.F37 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F38 { get => _position.F38; set { if (_position.F38 != value) { _position.F38 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F39 { get => _position.F39; set { if (_position.F39 != value) { _position.F39 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F40 { get => _position.F40; set { if (_position.F40 != value) { _position.F40 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F41 { get => _position.F41; set { if (_position.F41 != value) { _position.F41 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F42 { get => _position.F42; set { if (_position.F42 != value) { _position.F42 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F43 { get => _position.F43; set { if (_position.F43 != value) { _position.F43 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F44 { get => _position.F44; set { if (_position.F44 != value) { _position.F44 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F45 { get => _position.F45; set { if (_position.F45 != value) { _position.F45 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F46 { get => _position.F46; set { if (_position.F46 != value) { _position.F46 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F47 { get => _position.F47; set { if (_position.F47 != value) { _position.F47 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F48 { get => _position.F48; set { if (_position.F48 != value) { _position.F48 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F49 { get => _position.F49; set { if (_position.F49 != value) { _position.F49 = value; OnPropertyChanged(); } } }
        //public Position.FieldContentEnum F50 { get => _position.F50; set { if (_position.F50 != value) { _position.F50 = value; OnPropertyChanged(); } } }

        #endregion

        #region Public methods

        //public Position.FieldContentEnum* GetFieldContentAddress(int fieldIndex)
        //{
        //    Position.FieldContentEnum* result;

        //    switch (fieldIndex)
        //    {
        //        case  1: fixed (Position.FieldContentEnum* p = &_position.Fields[ 1]) result = p; break;
        //        case  2: fixed (Position.FieldContentEnum* p = &_position.Fields[ 2]) result = p; break;
        //        case  3: fixed (Position.FieldContentEnum* p = &_position.Fields[ 3]) result = p; break;
        //        case  4: fixed (Position.FieldContentEnum* p = &_position.Fields[ 4]) result = p; break;
        //        case  5: fixed (Position.FieldContentEnum* p = &_position.Fields[ 5]) result = p; break;
        //        case  6: fixed (Position.FieldContentEnum* p = &_position.Fields[ 6]) result = p; break;
        //        case  7: fixed (Position.FieldContentEnum* p = &_position.Fields[ 7]) result = p; break;
        //        case  8: fixed (Position.FieldContentEnum* p = &_position.Fields[ 8]) result = p; break;
        //        case  9: fixed (Position.FieldContentEnum* p = &_position.Fields[ 9]) result = p; break;
        //        case 10: fixed (Position.FieldContentEnum* p = &_position.Fields[10]) result = p; break;
        //        case 11: fixed (Position.FieldContentEnum* p = &_position.Fields[11]) result = p; break;
        //        case 12: fixed (Position.FieldContentEnum* p = &_position.Fields[12]) result = p; break;
        //        case 13: fixed (Position.FieldContentEnum* p = &_position.Fields[13]) result = p; break;
        //        case 14: fixed (Position.FieldContentEnum* p = &_position.Fields[14]) result = p; break;
        //        case 15: fixed (Position.FieldContentEnum* p = &_position.Fields[15]) result = p; break;
        //        case 16: fixed (Position.FieldContentEnum* p = &_position.Fields[16]) result = p; break;
        //        case 17: fixed (Position.FieldContentEnum* p = &_position.Fields[17]) result = p; break;
        //        case 18: fixed (Position.FieldContentEnum* p = &_position.Fields[18]) result = p; break;
        //        case 19: fixed (Position.FieldContentEnum* p = &_position.Fields[19]) result = p; break;
        //        case 20: fixed (Position.FieldContentEnum* p = &_position.Fields[20]) result = p; break;
        //        case 21: fixed (Position.FieldContentEnum* p = &_position.Fields[21]) result = p; break;
        //        case 22: fixed (Position.FieldContentEnum* p = &_position.Fields[22]) result = p; break;
        //        case 23: fixed (Position.FieldContentEnum* p = &_position.Fields[23]) result = p; break;
        //        case 24: fixed (Position.FieldContentEnum* p = &_position.Fields[24]) result = p; break;
        //        case 25: fixed (Position.FieldContentEnum* p = &_position.Fields[25]) result = p; break;
        //        case 26: fixed (Position.FieldContentEnum* p = &_position.Fields[26]) result = p; break;
        //        case 27: fixed (Position.FieldContentEnum* p = &_position.Fields[27]) result = p; break;
        //        case 28: fixed (Position.FieldContentEnum* p = &_position.Fields[28]) result = p; break;
        //        case 29: fixed (Position.FieldContentEnum* p = &_position.Fields[29]) result = p; break;
        //        case 30: fixed (Position.FieldContentEnum* p = &_position.Fields[30]) result = p; break;
        //        case 31: fixed (Position.FieldContentEnum* p = &_position.Fields[31]) result = p; break;
        //        case 32: fixed (Position.FieldContentEnum* p = &_position.Fields[32]) result = p; break;
        //        case 33: fixed (Position.FieldContentEnum* p = &_position.Fields[33]) result = p; break;
        //        case 34: fixed (Position.FieldContentEnum* p = &_position.Fields[34]) result = p; break;
        //        case 35: fixed (Position.FieldContentEnum* p = &_position.Fields[35]) result = p; break;
        //        case 36: fixed (Position.FieldContentEnum* p = &_position.Fields[36]) result = p; break;
        //        case 37: fixed (Position.FieldContentEnum* p = &_position.Fields[37]) result = p; break;
        //        case 38: fixed (Position.FieldContentEnum* p = &_position.Fields[38]) result = p; break;
        //        case 39: fixed (Position.FieldContentEnum* p = &_position.Fields[39]) result = p; break;
        //        case 40: fixed (Position.FieldContentEnum* p = &_position.Fields[40]) result = p; break;
        //        case 41: fixed (Position.FieldContentEnum* p = &_position.Fields[41]) result = p; break;
        //        case 42: fixed (Position.FieldContentEnum* p = &_position.Fields[42]) result = p; break;
        //        case 43: fixed (Position.FieldContentEnum* p = &_position.Fields[43]) result = p; break;
        //        case 44: fixed (Position.FieldContentEnum* p = &_position.Fields[44]) result = p; break;
        //        case 45: fixed (Position.FieldContentEnum* p = &_position.Fields[45]) result = p; break;
        //        case 46: fixed (Position.FieldContentEnum* p = &_position.Fields[46]) result = p; break;
        //        case 47: fixed (Position.FieldContentEnum* p = &_position.Fields[47]) result = p; break;
        //        case 48: fixed (Position.FieldContentEnum* p = &_position.Fields[48]) result = p; break;
        //        case 49: fixed (Position.FieldContentEnum* p = &_position.Fields[49]) result = p; break;
        //        case 50: fixed (Position.FieldContentEnum* p = &_position.Fields[50]) result = p; break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(fieldIndex), "Invalid switch value");
        //    }
        //
        //    return result;
        //}

        #endregion
    }
}
