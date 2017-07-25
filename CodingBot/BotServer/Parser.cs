using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodingBot
{
    public class Parser
    {
        public Parser()
        {
        }

        public bool Parse(string input, ref Issue issue)
        {
            // save description (all utterances from init state)
            issue.m_lDesc.Add(input);
            return true;
        }

    }
}
