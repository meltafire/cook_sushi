using System;

namespace Assets.Utils.Ui
{
    public interface IButtonView
    {
        public event Action ButtonPressed;
        public void Toggle(bool isOn);
    }
}