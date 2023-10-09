using UnityEditor;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure
{
    public interface IIngridientsDispalyParentTransformProvider
    {
        public RectTransform IngridientsDispalyParentTransform { get; }
    }
}