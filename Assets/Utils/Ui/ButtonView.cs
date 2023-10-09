using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    public event Action OnButtonPressed;

    public void Toggle(bool isOn)
    {
        gameObject.SetActive(isOn);
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
