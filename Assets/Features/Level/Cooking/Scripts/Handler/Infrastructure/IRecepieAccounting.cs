using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure
{
    public interface IRecepieAccounting
    {
        public int IngridientsCount { get; }

        public void ShowIngridient(CookingAction scheme);
        public void ShowIngridient(CookingIngridientType scheme, int count);
        public void RevertIngridient();
    }
}