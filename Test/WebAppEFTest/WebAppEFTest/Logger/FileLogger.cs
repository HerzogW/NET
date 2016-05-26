using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebAppEFTest.Logger
{
    public class FileLogger
    {
        public void LogException(Exception e)
        {
            File.WriteAllLines(
                "C://Error//error.txt",
                new string[]
                {
                    "Message:" + e.Message,
                    "Stacktrace:" + e.StackTrace });
        }
    }
}