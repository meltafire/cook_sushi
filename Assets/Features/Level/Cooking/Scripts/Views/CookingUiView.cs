using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour
    {
        [SerializeField]
        private Button _backButton;
        [SerializeField]
        private CookingTypeMenuUiView _cookingTypeMenuUiView;

        public event Action OnBackButtonClick;

        public CookingTypeMenuUiView CookingTypeMenuUiView => _cookingTypeMenuUiView;

        public void Toggle(bool shouldTurnOn)
        {
            gameObject.SetActive(shouldTurnOn);
        }

        public void ToggleBackButton(bool isOn)
        {
            _backButton.gameObject.SetActive(isOn);
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