using Check.Models;
using System.Diagnostics;
using System.Reflection;

namespace Check.ViewModels
{
    internal class FieldViewModel : BaseViewModel
    {
        #region Enumerations

        internal enum FieldStatusEnum
        {
            Dummy,
            Default,
            MouseOver,
            CanStart,
            Started,
            CanBeTaken,
            Taken
        }

        #endregion

        #region Constructors

        public FieldViewModel(PositionViewModel positionViewModel, int fieldIndex)
        {
            Debug.Assert(positionViewModel != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

            PositionViewModel = positionViewModel;

          //FieldIndex        = fieldIndex;

            PropertyInfo      = typeof(PositionViewModel).GetProperty("F" + fieldIndex.ToString("00"));
        }

        #endregion

        #region Public properties

        public PositionViewModel PositionViewModel { get;      }

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

        public Position.FieldContentEnum FieldContent
        {
            get
            {
                Debug.Assert(PropertyInfo != null);

                return (Position.FieldContentEnum) PropertyInfo.GetValue(PositionViewModel);
            }
            set
            {
                Debug.Assert(PropertyInfo != null);

                if (FieldContent != value)
                {
                    PropertyInfo.SetValue(PositionViewModel, value);

                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Private properties

      //private int          FieldIndex   { get; }

        private PropertyInfo PropertyInfo { get; }

        #endregion

        #region Private methods

        public void Refresh()
        {
            Position.FieldContentEnum fieldContent = FieldContent;
                     FieldStatusEnum  fieldStatus  = FieldStatus ;

            FieldContent = Position.FieldContentEnum.Dummy;
            FieldStatus  =          FieldStatusEnum .Dummy;

            FieldContent = fieldContent;
            FieldStatus  = fieldStatus ;
        }

        #endregion
    }
}
