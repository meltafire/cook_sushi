using Assets.Features.Level.Cooking.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure
{
    public interface IRecepieSchemeDrawer
    {
        public void ShowIngridient(CookingAction scheme);
        public void RevertIngridient();
    }
}