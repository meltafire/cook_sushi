using System;

namespace Assets.Features.Menu.Scripts.Views
{
    public interface IMenuView
    {
        public event Action OnButtonPressed;
        public void SetActive(bool isOn);
    }
}