using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Engine
{
    public class ErrorLogger
    {
        private static ErrorLogger instance;
        private ErrorLogger() { }

        /// <summary>
        /// This creates the Error logger singleton
        /// </summary>
        public static ErrorLogger Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ErrorLogger();
                }
                return instance;
            }
        }
        /// <summary>
        /// This Method loggs an error to to disk
        /// </summary>
        /// <param name="Error"></param>
        public void LogErrorToDisk(string ErrorString,string Errorfile)
        {
            try
            {
                System.IO.StreamWriter file = File.AppendText(Errorfile);
                
                if (file == null)
                {
                    return;
                }
                Log(ErrorString, file);
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        private static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("Report :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

    }
}
