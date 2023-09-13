using System;
using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingView : MonoBehaviour
    {
        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }
    }
}