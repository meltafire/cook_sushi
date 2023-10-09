using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure
{
    public interface IIngridientsParentTransformProvider
    {
        public RectTransform IngridientsParentTransform { get; }
    }
}