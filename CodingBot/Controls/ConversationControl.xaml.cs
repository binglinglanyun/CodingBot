using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodingBot.Controls
{
    /// <summary>
    /// Interaction logic for ConversationControl.xaml
    /// </summary>
    public partial class ConversationControl : UserControl
    {
        public ConversationControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region Properties
        private UserDataControlViewModel _userDataViewModel = new UserDataControlViewModel();
        public UserDataControlViewModel UserDataViewModel
        {
            get
            {
                return _userDataViewModel;
            }
            set
            {
                _userDataViewModel = value;
                NotifyPropertyChanged("UserDataViewModel");
            }
        }
        #endregion

        #region Notify Prperty Changed
        private event PropertyChangedEventHandler _propertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
