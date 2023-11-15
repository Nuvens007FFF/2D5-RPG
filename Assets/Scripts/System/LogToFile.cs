using UnityEngine;
using System.IO;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;

    private void Start()
    {
        logFilePath = Application.persistentDataPath + "/DebugLog.txt";
        Debug.Log("Logging to: " + logFilePath);

        // Redirect logs to a file
        Application.logMessageReceived += LogToFileHandler;
    }

    private void LogToFileHandler(string logString, string stackTrace, LogType type)
    {
        // Write logs to a file
        using (StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine("[" + type + "] " + logString);
            sw.WriteLine(stackTrace);
        }
    }

    private void OnDestroy()
    {
        // Remove the log handler when the script is destroyed
        Application.logMessageReceived -= LogToFileHandler;
    }
}