using Assets.Features.App.LoadingScreen.Scripts.Views;
using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenView : MonoBehaviour, ILoadingScreenView
    {
        public void Toggle(bool isOn)
        {
            gameObject.SetActive(isOn);
        }
    }
}