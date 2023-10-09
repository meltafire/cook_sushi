using System;

namespace Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure
{
    public interface IMakiDisplayEvents
    {
        public event Action<bool> ToggleRequest;
    }
}