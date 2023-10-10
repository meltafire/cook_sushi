using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure
{
    public interface IRecepieParentTransformProvider
    {
        public RectTransform Transform { get; }
    }
}