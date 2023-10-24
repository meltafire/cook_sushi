using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure
{
    public interface IRecepieParentTransformProvider : IParentTransformProvider
    {
        public void Toggle(bool isOn);
    }
}