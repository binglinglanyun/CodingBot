using CodingBot.Common;
using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bot initialization...");
            Bot bot = new Bot();
            bot.Init();

            // init all paths
            List<string> inputPaths = new List<string> { "input1", "input2" };
            List<string> outputPaths = new List<string> { "output1", "input2" };
            List<string> resourcePaths = new List<string> { "input1", "input2" };
            List<string> referencePaths = new List<string> { "input1", "input2" };

            UserData userData = new UserData();
            userData.InputPath = inputPaths;
            userData.OutputPath = outputPaths;
            userData.ResourcePath = resourcePaths;
            userData.ReferencePath = referencePaths;


            ResponseData responseData1 = bot.InitializeUserInputData(userData);
            Console.Write(responseData1.BotMessage);

            //filter
            ResponseData responseData2 = bot.GetResponse("1");
            Console.Write("\n");
            Console.Write(responseData2.BotMessage);
            //select one table
            ResponseData responseDataInterActive2_1 = bot.GetResponseInteractive(new List<List<string>> { new List<string> { "Table1" } });
            Console.Write("\n");
            Console.Write(responseDataInterActive2_1.BotMessage);
            //filter condition
            ResponseData responseData2_1 = bot.GetResponse("AdId != 2006");
            Console.Write("\n");
            Console.Write(responseData2_1.BotMessage);
            // choose to select columns
            ResponseData responseData2_2 = bot.GetResponse("yes");
            Console.Write("\n");
            Console.Write(responseData2_2.BotMessage);
            //select columns
            ResponseData responseDataInterActive2_2 = bot.GetResponseInteractive(new List<List<string>> { new List<string> { "AdId", "AdText" } });
            Console.Write("\n");
            Console.Write(responseDataInterActive2_2.BotMessage);
            // input new table name
            ResponseData responseData2_3 = bot.GetResponse("newtable1");
            Console.Write("\n");
            Console.Write(responseData2_3.BotMessage);


            ResponseData responseData3 = bot.GetResponse("2");

            Console.Write(responseData3.BotMessage);

            ResponseData responseData4 = bot.GetResponse("3");
            Console.Write(responseData4.BotMessage);

            ResponseData responseData5 = bot.GetResponse("4");
            Console.Write(responseData5.BotMessage);

            ResponseData responseData6 = bot.GetResponse("5");
            Console.Write(responseData6.BotMessage);

            ResponseData responseData7 = bot.GetResponse("6");
            Console.Write(responseData7.BotMessage);

            ResponseData responseData8 = bot.GetResponse("7");
            Console.Write(responseData8.BotMessage);

            ResponseData responseData9 = bot.GetResponse("8");
            Console.Write(responseData9.BotMessage);

            ResponseData responseData10 = bot.GetResponse("9");
            Console.Write(responseData10.BotMessage);


        }
    }
}
