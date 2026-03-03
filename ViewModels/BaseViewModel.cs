using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace Check.ViewModels
{
    internal class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public methods

        protected void NotifyPropertyChanged<T>(Expression<Func<int, T>> property)
        {
            var handler  = PropertyChanged;

            if (handler != null)
            {
                if (property != null)
                {
                    var memberExpression  = property.Body as MemberExpression;

                    if (memberExpression == null)
                    {
                        throw new Exception("MemberExpression == null");
                    }

                    handler(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }

        public void NotifyPropertyChangedAll()
        {
            var handler  = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(null));
            }
        }

        #endregion
    }
}
