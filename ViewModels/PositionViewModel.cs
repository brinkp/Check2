using Check.Models;
// ReSharper disable InvertIf
// ReSharper disable LocalizableElement

namespace Check.ViewModels
{
    internal class PositionViewModel : BaseViewModel
    {
        #region ENumerations

        public enum PositionStatusEnum
        {
            Default,
            FromGiven,
            TakeInProgress
        }

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
