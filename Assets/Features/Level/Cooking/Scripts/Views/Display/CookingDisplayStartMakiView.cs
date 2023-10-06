using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views.Display
{
    public class CookingDisplayStartMakiView : MonoBehaviour
    {
        public void Show(bool isOn)
        {
            gameObject.SetActive(isOn);
        }
    }
}