using Check.Models;
using System.Diagnostics;

namespace Check.ViewModels
{
    internal class FieldViewModel : BaseViewModel
    {
        // The implementation of class FieldViewModel is completely determined by performance in both space and time.

        #region Enumerations

        internal enum FieldStatusEnum
        {
            Default           ,
            CanBeFrom         ,
            MouseOverCanBeFrom,
            CanBeTo           ,
            MouseOverCanBeTo  ,
            FromGiven
        }

        #endregion

        #region Constructors

        public FieldViewModel(PositionViewModel positionViewModel, int fieldIndex)
        {
            Debug.Assert(positionViewModel                 != null);
            Debug.Assert(positionViewModel.Position        != null);
            Debug.Assert(positionViewModel.Position._fields != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

           _fields            = positionViewModel.Position._fields;

           _fieldIndex        = fieldIndex;
        }

        #endregion

        #region Fields

        private readonly Position.FieldContentEnum[] _fields    ;
        private readonly int                         _fieldIndex;

        #endregion

        #region Public properties

        public Position.FieldContentEnum FieldContent
        {
            get => _fields[_fieldIndex]        ;
            set => _fields[_fieldIndex] = value;
        }

        private FieldStatusEnum _fieldStatus = FieldStatusEnum.Default;
        public  FieldStatusEnum  FieldStatus
        {
            get => _fieldStatus;
            set
            {
                if (_fieldStatus != value)
                {
                    _fieldStatus  = value;

                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Public methods

        public void ResetStatus()
        {
            FieldStatus = FieldStatusEnum.Default;

            Refresh();
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(FieldContent));
            OnPropertyChanged(nameof(FieldStatus ));
        }

        #endregion
    }
}
