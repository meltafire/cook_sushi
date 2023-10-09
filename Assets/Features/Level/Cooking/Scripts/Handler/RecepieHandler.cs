using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using System.Collections.Generic;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieHandler : IRecepieSchemeDrawer
    {
        private readonly IMakiDisplayExternalEvents _makiDisplayExternaEvents;
        private readonly Stack<CookingAction> _drawedElements = new Stack<CookingAction>();

        public RecepieHandler(IMakiDisplayExternalEvents makiDisplayExternaEvents)
        {
            _makiDisplayExternaEvents = makiDisplayExternaEvents;
        }

        public void ShowIngridient(CookingAction scheme)
        {
            _drawedElements.Push(scheme);

            Toggle(scheme, true);
        }

        public void RevertIngridient()
        {
            var scheme = _drawedElements.Pop();

            Toggle(scheme, false);
        }

        private void Toggle(CookingAction scheme, bool isOn)
        {
            if(scheme == CookingAction.Maki)
            {
                _makiDisplayExternaEvents.Show(isOn);
            }
        }
    }
}