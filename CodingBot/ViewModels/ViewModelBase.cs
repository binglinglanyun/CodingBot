using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.ViewModels
{
    public class ViewModelBase
    {
        private event PropertyChangedEventHandler _propertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
