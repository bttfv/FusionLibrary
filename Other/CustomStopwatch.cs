﻿using System;
using System.Diagnostics;

namespace FusionLibrary
{
    public class CustomStopwatch : Stopwatch
    {
        public CustomStopwatch() : base()
        {

        }

        public void StartNewRecord()
        {
            System.IO.File.AppendAllText("stopwatch.txt", Environment.NewLine + Environment.NewLine + "NEW RECORDINGS" + Environment.NewLine + Environment.NewLine);
            Reset();
            Start();
        }

        public void WriteAndReset(string name)
        {
            Stop();
            System.IO.File.AppendAllText("stopwatch.txt", name + " " + ElapsedTicks.ToString() + Environment.NewLine);
            Reset();
            Start();
        }
    }
}
