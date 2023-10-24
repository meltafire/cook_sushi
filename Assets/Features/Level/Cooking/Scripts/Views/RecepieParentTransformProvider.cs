using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views
{
    public class RecepieParentTransformProvider : MonoBehaviour, IRecepieParentTransformProvider
    {
        [SerializeField]
        private RectTransform _recepieParentTransform;

        public Transform Transform => _recepieParentTransform;

        public void Toggle(bool isOn)
        {
            _recepieParentTransform.gameObject.SetActive(isOn);
        }
    }
}