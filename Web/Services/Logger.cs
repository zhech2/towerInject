﻿namespace Web.Services
{
    public class Logger : ILogger
    {
        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine($"Logger: {message}");
        }
    }
}