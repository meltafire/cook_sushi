using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour, IIngridientsDispalyParentTransformProvider
    {
        [SerializeField]
        private ButtonView _backButton;
        [SerializeField]
        private ButtonView _revertButton;
        [SerializeField]
        private ButtonView _doneButton;
        [SerializeField]
        private RectTransform _ingridientsDispalyParentTransform;
        [SerializeField]
        private RectTransform _recepieParentTransform;
        [SerializeField]
        private RectTransform _ingridientsParentTransform;

        public RectTransform IngridientsDispalyParentTransform => _ingridientsDispalyParentTransform;
        public RectTransform RecepieParentTransform => _recepieParentTransform;
        public RectTransform IngridientsParentTransform => _ingridientsParentTransform;

        public ButtonView BackButtonView => _backButton;
        public ButtonView DoneButtonView => _doneButton;
        public ButtonView RevertButtonView => _revertButton;

        public void Toggle(bool isOn)
        {
            gameObject.SetActive(isOn);
        }

        public void ToggleIngridients(bool isOn)
        {
            _ingridientsParentTransform.gameObject.SetActive(isOn);
        }

        public void ToggleRecepies(bool isOn)
        {
            _recepieParentTransform.gameObject.SetActive(isOn);
        }
    }
}