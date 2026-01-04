using Check.Models;
// ReSharper disable InvertIf

namespace Check.ViewModels
{
    public class PositionViewModel : BaseViewModel
    {
        #region Fields

        private readonly Position _position;

        #endregion

        #region Constructors

        public PositionViewModel(Position position)
        {
            if (position == null) System.Diagnostics.Debugger.Break();

           _position = position;
        }

        #endregion

        #region Public properties

        public Position.Field F01 { get => _position.F01; set { if (_position.F01 != value) { _position.F01 = value; NotifyPropertyChanged(x => F01); } } }
        public Position.Field F02 { get => _position.F02; set { if (_position.F02 != value) { _position.F02 = value; NotifyPropertyChanged(x => F02); } } }
        public Position.Field F03 { get => _position.F03; set { if (_position.F03 != value) { _position.F03 = value; NotifyPropertyChanged(x => F03); } } }
        public Position.Field F04 { get => _position.F04; set { if (_position.F04 != value) { _position.F04 = value; NotifyPropertyChanged(x => F04); } } }
        public Position.Field F05 { get => _position.F05; set { if (_position.F05 != value) { _position.F05 = value; NotifyPropertyChanged(x => F05); } } }
        public Position.Field F06 { get => _position.F06; set { if (_position.F06 != value) { _position.F06 = value; NotifyPropertyChanged(x => F06); } } }
        public Position.Field F07 { get => _position.F07; set { if (_position.F07 != value) { _position.F07 = value; NotifyPropertyChanged(x => F07); } } }
        public Position.Field F08 { get => _position.F08; set { if (_position.F08 != value) { _position.F08 = value; NotifyPropertyChanged(x => F08); } } }
        public Position.Field F09 { get => _position.F09; set { if (_position.F09 != value) { _position.F09 = value; NotifyPropertyChanged(x => F09); } } }
        public Position.Field F10 { get => _position.F10; set { if (_position.F10 != value) { _position.F10 = value; NotifyPropertyChanged(x => F10); } } }
        public Position.Field F11 { get => _position.F11; set { if (_position.F11 != value) { _position.F11 = value; NotifyPropertyChanged(x => F11); } } }
        public Position.Field F12 { get => _position.F12; set { if (_position.F12 != value) { _position.F12 = value; NotifyPropertyChanged(x => F12); } } }
        public Position.Field F13 { get => _position.F13; set { if (_position.F13 != value) { _position.F13 = value; NotifyPropertyChanged(x => F13); } } }
        public Position.Field F14 { get => _position.F14; set { if (_position.F14 != value) { _position.F14 = value; NotifyPropertyChanged(x => F14); } } }
        public Position.Field F15 { get => _position.F15; set { if (_position.F15 != value) { _position.F15 = value; NotifyPropertyChanged(x => F15); } } }
        public Position.Field F16 { get => _position.F16; set { if (_position.F16 != value) { _position.F16 = value; NotifyPropertyChanged(x => F16); } } }
        public Position.Field F17 { get => _position.F17; set { if (_position.F17 != value) { _position.F17 = value; NotifyPropertyChanged(x => F17); } } }
        public Position.Field F18 { get => _position.F18; set { if (_position.F18 != value) { _position.F18 = value; NotifyPropertyChanged(x => F18); } } }
        public Position.Field F19 { get => _position.F19; set { if (_position.F19 != value) { _position.F19 = value; NotifyPropertyChanged(x => F19); } } }
        public Position.Field F20 { get => _position.F20; set { if (_position.F20 != value) { _position.F20 = value; NotifyPropertyChanged(x => F20); } } }
        public Position.Field F21 { get => _position.F21; set { if (_position.F21 != value) { _position.F21 = value; NotifyPropertyChanged(x => F21); } } }
        public Position.Field F22 { get => _position.F22; set { if (_position.F22 != value) { _position.F22 = value; NotifyPropertyChanged(x => F22); } } }
        public Position.Field F23 { get => _position.F23; set { if (_position.F23 != value) { _position.F23 = value; NotifyPropertyChanged(x => F23); } } }
        public Position.Field F24 { get => _position.F24; set { if (_position.F24 != value) { _position.F24 = value; NotifyPropertyChanged(x => F24); } } }
        public Position.Field F25 { get => _position.F25; set { if (_position.F25 != value) { _position.F25 = value; NotifyPropertyChanged(x => F25); } } }
        public Position.Field F26 { get => _position.F26; set { if (_position.F26 != value) { _position.F26 = value; NotifyPropertyChanged(x => F26); } } }
        public Position.Field F27 { get => _position.F27; set { if (_position.F27 != value) { _position.F27 = value; NotifyPropertyChanged(x => F27); } } }
        public Position.Field F28 { get => _position.F28; set { if (_position.F28 != value) { _position.F28 = value; NotifyPropertyChanged(x => F28); } } }
        public Position.Field F29 { get => _position.F29; set { if (_position.F29 != value) { _position.F29 = value; NotifyPropertyChanged(x => F29); } } }
        public Position.Field F30 { get => _position.F30; set { if (_position.F30 != value) { _position.F30 = value; NotifyPropertyChanged(x => F30); } } }
        public Position.Field F31 { get => _position.F31; set { if (_position.F31 != value) { _position.F31 = value; NotifyPropertyChanged(x => F31); } } }
        public Position.Field F32 { get => _position.F32; set { if (_position.F32 != value) { _position.F32 = value; NotifyPropertyChanged(x => F32); } } }
        public Position.Field F33 { get => _position.F33; set { if (_position.F33 != value) { _position.F33 = value; NotifyPropertyChanged(x => F33); } } }
        public Position.Field F34 { get => _position.F34; set { if (_position.F34 != value) { _position.F34 = value; NotifyPropertyChanged(x => F34); } } }
        public Position.Field F35 { get => _position.F35; set { if (_position.F35 != value) { _position.F35 = value; NotifyPropertyChanged(x => F35); } } }
        public Position.Field F36 { get => _position.F36; set { if (_position.F36 != value) { _position.F36 = value; NotifyPropertyChanged(x => F36); } } }
        public Position.Field F37 { get => _position.F37; set { if (_position.F37 != value) { _position.F37 = value; NotifyPropertyChanged(x => F37); } } }
        public Position.Field F38 { get => _position.F38; set { if (_position.F38 != value) { _position.F38 = value; NotifyPropertyChanged(x => F38); } } }
        public Position.Field F39 { get => _position.F39; set { if (_position.F39 != value) { _position.F39 = value; NotifyPropertyChanged(x => F39); } } }
        public Position.Field F40 { get => _position.F40; set { if (_position.F40 != value) { _position.F40 = value; NotifyPropertyChanged(x => F40); } } }
        public Position.Field F41 { get => _position.F41; set { if (_position.F41 != value) { _position.F41 = value; NotifyPropertyChanged(x => F41); } } }
        public Position.Field F42 { get => _position.F42; set { if (_position.F42 != value) { _position.F42 = value; NotifyPropertyChanged(x => F42); } } }
        public Position.Field F43 { get => _position.F43; set { if (_position.F43 != value) { _position.F43 = value; NotifyPropertyChanged(x => F43); } } }
        public Position.Field F44 { get => _position.F44; set { if (_position.F44 != value) { _position.F44 = value; NotifyPropertyChanged(x => F44); } } }
        public Position.Field F45 { get => _position.F45; set { if (_position.F45 != value) { _position.F45 = value; NotifyPropertyChanged(x => F45); } } }
        public Position.Field F46 { get => _position.F46; set { if (_position.F46 != value) { _position.F46 = value; NotifyPropertyChanged(x => F46); } } }
        public Position.Field F47 { get => _position.F47; set { if (_position.F47 != value) { _position.F47 = value; NotifyPropertyChanged(x => F47); } } }
        public Position.Field F48 { get => _position.F48; set { if (_position.F48 != value) { _position.F48 = value; NotifyPropertyChanged(x => F48); } } }
        public Position.Field F49 { get => _position.F49; set { if (_position.F49 != value) { _position.F49 = value; NotifyPropertyChanged(x => F49); } } }
        public Position.Field F50 { get => _position.F50; set { if (_position.F50 != value) { _position.F50 = value; NotifyPropertyChanged(x => F50); } } }

        #endregion

    }
}
