using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views
{
    public class IngridientsParentTransformProvider : MonoBehaviour, IIngridientsParentTransformProvider
    {
        [SerializeField]
        private RectTransform _ingridientsParentTransform;

        public Transform Transform => _ingridientsParentTransform;

        public void Toggle(bool isOn)
        {
            _ingridientsParentTransform.gameObject.SetActive(isOn);
        }
    }
}