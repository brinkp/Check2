using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Check.Views
{
    public partial class PieceView
    {
        #region Delegates and events

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // Do nothing
            }
        }

        #endregion

        #region Constructors

        public PieceView()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty   FillProperty = DependencyProperty.Register(nameof(Fill  ), typeof(Brush), typeof(PieceView));
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PieceView));

        #endregion

        #region Public properties

        public Brush Fill
        {
            get => (Brush) GetValue(  FillProperty);
            set
            {
                if (Fill != value)
                {
                    SetValue(FillProperty, value);

                    OnPropertyChanged();
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

                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
