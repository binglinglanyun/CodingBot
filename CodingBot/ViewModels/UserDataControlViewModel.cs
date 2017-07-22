using CodingBot.Common;
using CodingBot.ToolWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodingBot.ViewModels
{
    public class UserDataControlViewModel : ViewModelBase
    {
        private UserData GetUserData()
        {
            UserData userData = new UserData();
            if (!string.IsNullOrEmpty(this.InputPath))
            {
                userData.InputPath = this.InputPath.Split(';').ToList();
            }

            if (!string.IsNullOrEmpty(this.OutputPath))
            {
                userData.OutputPath = this.OutputPath.Split(';').ToList();
            }

            if (!string.IsNullOrEmpty(this.ResourcePath))
            {
                userData.ResourcePath = this.ResourcePath.Split(';').ToList();
            }

            if (!string.IsNullOrEmpty(this.ReferencePath))
            {
                userData.ReferencePath = this.ReferencePath.Split(';').ToList();
            }

            return userData;
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
                    var dataStatusWindow = CodingBotClient.Instance.ShowToolWindow(typeof(DataStatusToolWindow));
                    if (dataStatusWindow != null && dataStatusWindow is DataStatusToolWindow)
                    {
                        (dataStatusWindow as DataStatusToolWindow).BindInputDataStatus(userData);
                    }
                });
            }
        }
        #endregion 
    }
}
