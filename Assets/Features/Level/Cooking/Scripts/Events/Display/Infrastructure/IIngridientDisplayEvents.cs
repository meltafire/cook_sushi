using Assets.Features.Level.Cooking.Scripts.Data;
using System;

namespace Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure
{
    public interface IIngridientDisplayEvents
    {
        public event Action<CookingStep, int> ShowRequest;
        public event Action HideRequest;
    }
}