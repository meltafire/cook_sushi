using Assets.Features.Level.Cooking.Scripts.Views.Actions;
using Assets.Utils.Ui;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour, IButtonView, ICookingMakiWrapActionView
{
    [SerializeField]
    private Button _button;

    public event Action ButtonPressed;

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
        ButtonPressed?.Invoke();
    }

    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }
}
