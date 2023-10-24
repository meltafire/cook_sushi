using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views
{
    public class IngridientsDisplayParentTransformProvider : MonoBehaviour, IIngridientsDisplayParentTransformProvider
    {
        [SerializeField]
        private RectTransform _ingridientsDisplayParentTransform;

        public Transform Transform => _ingridientsDisplayParentTransform;
    }
}