using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sushi.Level.Menu
{
    public class LevelMenuView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        public event Action OnButtonClick;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickHappened);
        }

        private void OnDisable()
        {
            _button.onClick.AddListener(OnClickHappened);
        }

        private void OnClickHappened()
        {
            OnButtonClick?.Invoke();
        }
    }
}