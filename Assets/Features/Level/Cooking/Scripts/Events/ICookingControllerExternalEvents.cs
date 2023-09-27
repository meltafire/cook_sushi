using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerExternalEvents
    {
        public void RequestShow(bool toggle);

        public event Action BackButtonClicked;
    }
}