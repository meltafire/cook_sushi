using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Cooking
{
    public class CookingTypeMenuUiView : MonoBehaviour, IRecipeSelectionButtonEvents
    {
        [SerializeField]
        private Button _nigiriButton;
        [SerializeField]
        private Button _makiButton;

        public event Action<CookingScheme> SchemeChosen;

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

        public void ToggleNigiriButton(bool isOn)
        {
            _nigiriButton.gameObject.SetActive(isOn);
        }

        public void ToggleMakiButton(bool isOn)
        {
            _makiButton.gameObject.SetActive(isOn);
        }

        private void OnNigiriButtonClick()
        {
            SchemeChosen?.Invoke(CookingScheme.Nigiri);
        }

        private void OnMakiButtonClick()
        {
            SchemeChosen?.Invoke(CookingScheme.Maki);
        }

        public void ShowButtons(bool isOn)
        {
            transform.gameObject.SetActive(isOn);
        }
    }
}