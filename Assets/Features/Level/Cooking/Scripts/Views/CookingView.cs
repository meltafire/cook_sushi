using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingView : MonoBehaviour
    {
        [SerializeField]
        private Transform _cookingMakiTransform;

        public Transform CookingMakiTransform => _cookingMakiTransform;

        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }
    }
}