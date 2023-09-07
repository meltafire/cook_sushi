using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour
    {
        [SerializeField]
        private Button _backButton;

        public event Action OnBackButtonClick;

        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClickHappen);
        }

        private void OnDisable()
        {
            _backButton.onClick.AddListener(OnBackButtonClickHappen);
        }

        private void OnBackButtonClickHappen()
        {
            OnBackButtonClick?.Invoke();
        }
    }
}