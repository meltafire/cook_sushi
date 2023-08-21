using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Menu.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        public event Action OnButtonPressed;

        public void Release()
        {
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            OnButtonPressed?.Invoke();
        }
    }
}