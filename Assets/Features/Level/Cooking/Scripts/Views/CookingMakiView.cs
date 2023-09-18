using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingMakiView : MonoBehaviour
    {
        public void Toggle(bool isOn)
        {
            transform.gameObject.SetActive(isOn);
        }
    }
}