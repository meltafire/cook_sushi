using UnityEngine;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure
{
    public interface IIngridientsParentTransformProvider : IParentTransformProvider
    {
        public void Toggle(bool isOn);
    }
}