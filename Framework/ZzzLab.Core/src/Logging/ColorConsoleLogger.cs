﻿using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace ZzzLab.Logging
{
    public class ColorConsoleLogger : LoggerBase, IZLogger
    {
        public override void Log(
            LogLevel level,
            object value,
            [CallerMemberName] string methodName = null
        )
        {
            string message = $"[{DateTime.Now.To24Hours()}: {level}] {methodName} | {value}";

            Console.WriteLine(message);
        }
    }
}