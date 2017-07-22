using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DataStatusControl.xaml
    /// </summary>
    public partial class DataStatusControl : UserControl
    {
        public DataStatusControl()
        {
            InitializeComponent();
        }

        public void BindInputDataStatus(UserData userData)
        {
            this._inputDataStatus.DataContext = userData;
        }

        public void BindTableStatus()
        {

        }
    }
}
