using System;
using UnityEngine;
using Utils.Input;

namespace Sushi.Level.WorkplaceIcon
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