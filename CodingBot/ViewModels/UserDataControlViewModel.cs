using CodingBot.Common;
using CodingBot.ToolWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CodingBot.ViewModels
{
    public class UserDataControlViewModel : ViewModelBase
    {
        private event EventHandler<EventArgs> _buttonClickEventHandlers;
        private UserData GetUserData()
        {
            UserData userData = new UserData();
            if (!string.IsNullOrEmpty(this.InputPath))
            {
                List<string> inputPath = this.InputPath.Split(';').ToList();
                foreach (string path in inputPath)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        userData.InputPath.Add(path);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.OutputPath))
            {
                List<string> outputPath = this.OutputPath.Split(';').ToList();
                foreach (string path in outputPath)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        userData.OutputPath.Add(path);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.ResourcePath))
            {
                List<string> resourcePath = this.ResourcePath.Split(';').ToList();
                foreach (string path in resourcePath)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        userData.ResourcePath.Add(path);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.ReferencePath))
            {
                List<string> referencePath = this.ReferencePath.Split(';').ToList();
                foreach (string path in referencePath)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        userData.ReferencePath.Add(path);
                    }
                }
            }

            return userData;
        }

        public void RegisterButtonClickEvent(EventHandler<EventArgs> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            _buttonClickEventHandlers += eventHandler;
        }

        public void OnButtonClick(EventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            if (_buttonClickEventHandlers != null)
            {
                _buttonClickEventHandlers(this, eventArgs);
            }
        }

        #region Properties
        private string _inputpath;
        public string InputPath
        {
            get
            {
                return _inputpath;
            }
            set
            {
                _inputpath = value;
                NotifyPropertyChanged("InputPath");
            }
        }

        private string _outputpath;
        public string OutputPath
        {
            get
            {
                return _outputpath;
            }
            set
            {
                _outputpath = value;
                NotifyPropertyChanged("OutputPath");
            }
        }

        private string _referencepath;
        public string ReferencePath
        {
            get
            {
                return _referencepath;
            }
            set
            {
                _referencepath = value;
                NotifyPropertyChanged("ReferencePath");
            }
        }

        private string _resourcepath;
        public string ResourcePath
        {
            get
            {
                return _resourcepath;
            }
            set
            {
                _resourcepath = value;
                NotifyPropertyChanged("ResourcePath");
            }
        }

        public ICommand OK_Click
        {
            get
            {
                return new DelegateCommand<object>((object obj) =>
                {
                    UserData userData = GetUserData();

                    // Here should get Script from bot
                    var scriptWindow = CodingBotClient.Instance.ShowToolWindow(typeof(ScriptToolWindow));
                    if (scriptWindow != null && scriptWindow is ScriptToolWindow)
                    {
                        string script = @"searchlog =
    EXTRACT UID : int,
            StartTime : DateTime,
            Country : string,
            Query : string,
            Duration : int,
            Urls : string,
            ClickUrls : string
";
                        (scriptWindow as ScriptToolWindow).UpdateScript(script);
                    }

                    // Here should get TableStatus from bot
                    List<TableItem> tableItems = new List<TableItem>();
                    for (int j = 0; j < 2; j++)
                    {
                        TableItem tableItem = new TableItem();
                        tableItem.TableName = "Table Item" + j;
                        for (int i = 0; i < 4; i++)
                        {
                            tableItem.TableData.Add(new RowItem("xnl", "yunying"));
                        }
                        
                        tableItems.Add(tableItem);
                    }
                    
                    var dataStatusWindow = CodingBotClient.Instance.ShowToolWindow(typeof(DataStatusToolWindow));
                    if (dataStatusWindow != null && dataStatusWindow is DataStatusToolWindow)
                    {
                        (dataStatusWindow as DataStatusToolWindow).BindInputDataStatus(userData);
                        (dataStatusWindow as DataStatusToolWindow).BindTableDataStatus(tableItems);
                    }

                    this.OnButtonClick(new EventArgs());
                });
            }
        }
        #endregion 
    }
}
