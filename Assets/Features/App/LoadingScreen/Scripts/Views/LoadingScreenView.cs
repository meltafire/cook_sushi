using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenView : MonoBehaviour
    {
        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }
    }
}