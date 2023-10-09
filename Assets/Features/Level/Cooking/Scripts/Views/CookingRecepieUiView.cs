using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingRecepieUiView : MonoBehaviour
    {
        public void ShowButtons(bool isOn)
        {
            transform.gameObject.SetActive(isOn);
        }
    }
}