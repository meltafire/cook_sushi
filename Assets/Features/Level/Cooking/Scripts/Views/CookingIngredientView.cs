using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingIngredientView : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    public event Action OnClick;

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
