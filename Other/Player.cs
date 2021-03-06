﻿using GTA;

namespace FusionLibrary
{
    public delegate void OnPlayerCompleted();

    public abstract class Player
    {
        public Entity Entity { get; set; }

        public OnPlayerCompleted OnPlayerCompleted { get; set; }

        public bool IsPlaying { get; protected set; }

        public abstract void Play();

        public abstract void Tick();

        public abstract void Stop();

        public abstract void Dispose();
    }
}
