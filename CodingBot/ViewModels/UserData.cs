using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.ViewModels
{
    public class UserData
    {
        public UserData()
        {
            this.InputPath = new List<string>();
            this.OutputPath = new List<string>();
            this.ReferencePath = new List<string>();
            this.ResourcePath = new List<string>();
        }

        public List<string> InputPath
        {
            get;
            set;
        }

        public List<string> OutputPath
        {
            get;
            set;
        }

        public List<string> ReferencePath
        {
            get;
            set;
        }

        public List<string> ResourcePath
        {
            get;
            set;
        }
    }
}
