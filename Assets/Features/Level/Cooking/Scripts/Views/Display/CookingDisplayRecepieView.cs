using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using UnityEngine;

public class CookingDisplayRecepieView : MonoBehaviour,
    ICookingDisplayMakiStartRecepieView,
    ICookingDisplayMakiEndRecepieView,
    ICookingDisplayNigiriRecepieView,
    ICookingDisplayMakiWrapView
{
    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }

    public void Toggle(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
}
