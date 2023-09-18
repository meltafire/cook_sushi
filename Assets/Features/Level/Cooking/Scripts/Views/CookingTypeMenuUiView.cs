using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Cooking
{
    public class CookingTypeMenuUiView : MonoBehaviour
    {
        [SerializeField]
        private Button _nigiriButton;
        [SerializeField]
        private Button _makiButton;

        public event Action<DishType> OnButtonClick;

        private void OnEnable()
        {
            _nigiriButton.onClick.AddListener(OnNigiriButtonClick);
            _makiButton.onClick.AddListener(OnMakiButtonClick);
        }

        private void OnDisable()
        {
            _nigiriButton.onClick.RemoveListener(OnNigiriButtonClick);
            _makiButton.onClick.RemoveListener(OnMakiButtonClick);
        }

        public void Toggle(bool isOn)
        {
            transform.gameObject.SetActive(isOn);
        }

        private void OnNigiriButtonClick()
        {
            OnButtonClick?.Invoke(DishType.Nigiri);
        }

        private void OnMakiButtonClick()
        {
            OnButtonClick?.Invoke(DishType.Maki);
        }
    }
}