using Check.Views;
using System.Windows;
using System.Windows.Media;

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

        public FieldViewModel(FieldStatusEnum fieldStatusEnum = FieldStatusEnum.Default)
        {
            FieldStatus = fieldStatusEnum;
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty   FillProperty = DependencyProperty.Register(nameof(Fill  ), typeof(Brush), typeof(FieldView));
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(FieldView));

        public Brush Fill
        {
            get => (Brush) GetValue(  FillProperty);
            set
            {
                if (Fill != value)
                {
                    SetValue(FillProperty, value);

                    NotifyPropertyChanged(x => Fill);
                }
            }
        }

        public Brush Stroke
        {
            get => (Brush) GetValue(StrokeProperty);
            set
            {
                if (Stroke != value)
                {
                    SetValue(StrokeProperty, value);

                    NotifyPropertyChanged(x => Stroke);
                }
            }
        }

        #endregion

        #region Public properties

        public FieldStatusEnum FieldStatus { get; set; }

        #endregion
    }
}
