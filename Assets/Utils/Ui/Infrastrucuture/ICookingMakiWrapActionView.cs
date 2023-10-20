using System;

namespace Assets.Features.Level.Cooking.Scripts.Views.Actions
{
    public interface ICookingMakiWrapActionView
    {
        public event Action ButtonPressed;
        public void Toggle(bool isOn);
        public void SetAsLastSibling();
    }
}