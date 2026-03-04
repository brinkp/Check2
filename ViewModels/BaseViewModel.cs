using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Check.ViewModels
{
    internal class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region Delegates and events

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
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
    }
}
