using CodingBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot
{
    public class Dispatcher
    {
        public Dispatcher()
        {
            m_Parser = new Parser();
            m_Issue = new Issue();
            //m_nPreviousState = 0; // Idle
        }
        
        /// <summary>
        /// Parsing user input, and save the parsing result in m_Issue
        /// </summary>
        /// <param name="input">String user input</param>
        /// <returns>Success flag</returns>
        public bool Parse(string input)
        {
            m_Parser.Parse(input, ref m_Issue);
            return true;
        }

        /// <summary>
        /// Generating action utterances based on user conversation history
        /// </summary>
        /// <returns>String array for utterances</returns>
        public ResponseData Action()
        {
            Taxonomy taxonomy = new Taxonomy();
            ResponseData responseData = taxonomy.getResponseData(ref m_Issue); //if no one found, the currentid=themeid


            return responseData;

        }

 
        // Dialogue state
        public byte m_nPreviousState;

        public Issue m_Issue;
        private Parser m_Parser;
    }
}
