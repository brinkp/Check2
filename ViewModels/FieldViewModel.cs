using Check.Models;
using System.Diagnostics;
using System.Reflection;

namespace Check.ViewModels
{
    internal unsafe class FieldViewModel : BaseViewModel
    {
        #region Enumerations

        internal enum FieldStatusEnum
        {
            Default,
            CanStart,
            MouseOverCanStart,
            Started,
            CanBeTaken,
            Taken
        }

        #endregion

        #region Constructors

        public FieldViewModel(PositionViewModel positionViewModel, int fieldIndex, Position.FieldContentEnum* fieldContent)
        {
            Debug.Assert(positionViewModel != null);

            Debug.Assert((fieldIndex >= 1) && (fieldIndex <= 50));

            Debug.Assert(fieldContent != null);

            PositionViewModel = positionViewModel;

          //FieldIndex        = fieldIndex;

           _fieldContent      = fieldContent;

            PropertyInfo      = typeof(PositionViewModel).GetProperty("F" + fieldIndex.ToString("00"));
        }

        #endregion

        #region Fields

        private readonly Position.FieldContentEnum* _fieldContent;

        #endregion

        #region Public properties

        public PositionViewModel PositionViewModel { get; }

        //public Position.FieldContentEnum FieldContent
        //{
        //    get => *_fieldContent;
        //    set
        //    {
        //        if (FieldContent != value)
        //        {
        //          *_fieldContent  = value;

        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public Position.FieldContentEnum FieldContent
        {
            get
            {
                Debug.Assert(PropertyInfo != null);

                return (Position.FieldContentEnum)PropertyInfo.GetValue(PositionViewModel);
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

        #region Private properties

      //private int          FieldIndex   { get; }

        private PropertyInfo PropertyInfo { get; }

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
