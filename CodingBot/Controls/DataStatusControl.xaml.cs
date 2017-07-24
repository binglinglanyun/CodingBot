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

        public void BindTableDataStatus(List<TableItem> tableItems)
        {
            if (tableItems != null)
            {
                foreach (var tableItem in tableItems)
                {
                    GenerateTableData(tableItem);
                }
            }
        }

        private void GenerateTableData(TableItem tableItem)
        {
            if (tableItem != null)
            {
                DataGrid dataGrid = new DataGrid();
                dataGrid.Margin = new Thickness(20,0,20,0);
                dataGrid.ItemsSource = tableItem.TableData;
                dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(PreviewGrid_LoadingRow);

                Expander expander = new Expander() { Header = tableItem.TableName };
                expander.Margin = new Thickness(0,10,0,0);
                expander.Foreground = this.Foreground;
                expander.Content = dataGrid;

                this._tableStatus.Children.Add(expander);
            }
        }

        private void PreviewGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
