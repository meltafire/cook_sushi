using UnityEngine;

namespace Sushi.Level.Cooking
{
    public class CookingUiView : MonoBehaviour
    {
        [SerializeField]
        private ButtonView _backButton;
        [SerializeField]
        private ButtonView _revertButton;
        [SerializeField]
        private ButtonView _doneButton;

        public ButtonView BackButtonView => _backButton;
        public ButtonView DoneButtonView => _doneButton;
        public ButtonView RevertButtonView => _revertButton;

        public void Toggle(bool isOn)
        {
            gameObject.SetActive(isOn);
        }
    }
}