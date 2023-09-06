using System;
using UnityEngine;
using Utils.Input;

namespace Sushi.Level.Workplace
{
    public class KitchenBoardView : MonoBehaviour, IClickable
    {
        public event Action OnClick;

        public void HandleClick()
        {
            Debug.Log("Click");

            OnClick?.Invoke();
        }
    }
}