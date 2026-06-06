using System;
using System.IO;

namespace PersonManagerApp.Services;
public class LogService
{
    private string path = "log.txt";

    public void WriteLog(string user, string action)
    {
        string message = $"{DateTime.Now} | User: {user} | Action: {action}";

        File.AppendAllText(path, message + Environment.NewLine);
    }
}
