using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour, ICookingUiView, IIngridientsDispalyParentTransformProvider, IIngridientsParentTransformProvider
    {
        [SerializeField]
        private ButtonView _backButton;
        [SerializeField]
        private ButtonView _revertButton;
        [SerializeField]
        private ButtonView _doneButton;
        [SerializeField]
        private CookingRecepieUiView _cookingRecepieUiView;
        [SerializeField]
        private RectTransform _ingridientsDispalyParentTransform;
        [SerializeField]
        private RectTransform _ingridientsParentTransform;

        public CookingRecepieUiView CookingRecepieUiView => _cookingRecepieUiView;
        public RectTransform IngridientsDispalyParentTransform => _ingridientsDispalyParentTransform;
        public RectTransform IngridientsParentTransform => _ingridientsParentTransform;

        public ButtonView BackButtonView => _backButton;
        public ButtonView DoneButtonView => _doneButton;
        public ButtonView RevertButtonView => _revertButton;

        public void Toggle(bool isOn)
        {
            gameObject.SetActive(isOn);
        }
    }
}