using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour, ICookingUiView, IIngridientsDispalyParentTransformProvider, IIngridientsParentTransformProvider
    {
        [SerializeField]
        private Button _backButton;
        [SerializeField]
        private CookingRecepieUiView _cookingRecepieUiView;
        [SerializeField]
        private RectTransform _ingridientsDispalyParentTransform;
        [SerializeField]
        private RectTransform _ingridientsParentTransform;

        public event Action OnBackButtonClick;

        public CookingRecepieUiView CookingRecepieUiView => _cookingRecepieUiView;
        public RectTransform IngridientsDispalyParentTransform => _ingridientsDispalyParentTransform;
        public RectTransform IngridientsParentTransform => _ingridientsParentTransform;

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