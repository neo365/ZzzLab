﻿namespace System
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() : base()
        {
        }

        public DataNotFoundException(string message) : base(message)
        {
        }
    }
}