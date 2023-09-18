using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingView : MonoBehaviour
    {
        [SerializeField]
        private CookingMakiView _cookingMakiView;

        public CookingMakiView CookingMakiView => _cookingMakiView;

        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }
    }
}