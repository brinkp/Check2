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
            Default,
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

        public FieldStatusEnum   FieldStatus       { get; set; } = FieldStatusEnum.Default;

        public Position.FieldContentEnum FieldContentEnum
        {
            get
            {
                Debug.Assert(PropertyInfo != null);

                return (Position.FieldContentEnum) PropertyInfo.GetValue(PositionViewModel);
            }
        }

        #endregion

        #region Private properties

      //private int          FieldIndex   { get; }

        private PropertyInfo PropertyInfo { get; }

        #endregion
    }
}
