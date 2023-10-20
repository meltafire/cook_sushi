using Assets.Utils.Ui;
using System;
using UnityEngine;
using Utils.Input;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardView : MonoBehaviour, IClickable, IButtonView
    {
        public event Action ButtonPressed;

        public void HandleClick()
        {
            ButtonPressed?.Invoke();
        }

        public void Toggle(bool isOn)
        {
        }
    }
}