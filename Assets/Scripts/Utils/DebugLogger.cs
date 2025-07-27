using System;
using System.IO;
using UnityEngine;

namespace BaristaSimulator.Utils
{
    public static class DebugLogger
    {
        private static string logFilePath;
        private static bool isInitialized = false;
        
        public static void Initialize()
        {
            if (isInitialized) return;
            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logFileName = $"Debug_Log_{timestamp}.txt";
            
            // Create Logs directory in project folder
            string logsDirectory;
            try
            {
                logsDirectory = Path.Combine(Application.dataPath, "..", "Logs");
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }
            }
            catch (System.Exception e)
            {
                // Fallback to persistent data path if project folder is not writable
                logsDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }
                Debug.LogWarning($"Could not create logs in project folder, using: {logsDirectory}. Error: {e.Message}");
            }
            
            logFilePath = Path.Combine(logsDirectory, logFileName);
            
            // Write header
            File.WriteAllText(logFilePath, $"=== Barista Simulator Debug Log ===\n");
            File.AppendAllText(logFilePath, $"Created: {DateTime.Now}\n");
            File.AppendAllText(logFilePath, $"Unity Version: {Application.unityVersion}\n");
            File.AppendAllText(logFilePath, $"Platform: {Application.platform}\n");
            File.AppendAllText(logFilePath, $"=====================================\n\n");
            
            isInitialized = true;
            
            Debug.Log($"Debug log created at: {logFilePath}");
        }
        
        public static void Log(string message, string category = "General")
        {
            if (!isInitialized) Initialize();
            
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logEntry = $"[{timestamp}] [{category}] {message}\n";
            
            try
            {
                File.AppendAllText(logFilePath, logEntry);
                Debug.Log($"[{category}] {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to log file: {e.Message}");
            }
        }
        
        public static void LogError(string message, string category = "Error")
        {
            if (!isInitialized) Initialize();
            
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logEntry = $"[{timestamp}] [ERROR-{category}] {message}\n";
            
            try
            {
                File.AppendAllText(logFilePath, logEntry);
                Debug.LogError($"[{category}] {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to log file: {e.Message}");
            }
        }
        
        public static void LogSeparator()
        {
            if (!isInitialized) Initialize();
            
            try
            {
                File.AppendAllText(logFilePath, "----------------------------------------\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to log file: {e.Message}");
            }
        }
        
        public static string GetLogFilePath()
        {
            return logFilePath;
        }
    }
}