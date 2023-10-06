using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieHandler : IRecepieSchemeDrawer
    {
        private readonly IMakiDisplayExternaEvents _makiDisplayExternaEvents;

        public void ShowIngridient(CookingScheme scheme)
        {
            Toggle(scheme, true);
        }

        public void HideIngridient(CookingScheme scheme)
        {
            Toggle(scheme, false);
        }

        private void Toggle(CookingScheme scheme, bool isOn)
        {
            if(scheme == CookingScheme.Maki)
            {
                _makiDisplayExternaEvents.Show(isOn);
            }
        }
    }

    public interface IMakiDisplayExternaEvents
    {
        public void Show(bool isOn);
    }
}