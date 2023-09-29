using System;
using System.Collections.Generic;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerEvents
    {
        public event Action<bool> ShowRequest;
        public event Action<bool> ToggleBackButton;
        public void ReportBackButtonClicked();
    }
}