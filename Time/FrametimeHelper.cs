﻿using GTA;
using System;

namespace FusionLibrary
{
    public class FrameTimeHelper
    {
        private double remainder;
        public float FrameTime { get; }

        public int Count { get; private set; }

        public FrameTimeHelper(float frameTime)
        {
            FrameTime = frameTime;

            Reset();
        }

        public void Reset()
        {
            remainder = 0;
            Count = 0;
        }

        public void Tick()
        {
            float delta = Game.LastFrameTime / FrameTime;

            Count = (int)Math.Truncate(delta);

            remainder += delta - Count;

            if (remainder >= 1)
            {
                int _remainder = (int)Math.Truncate(remainder);

                Count += _remainder;
                remainder -= _remainder;
            }
        }
    }
}
