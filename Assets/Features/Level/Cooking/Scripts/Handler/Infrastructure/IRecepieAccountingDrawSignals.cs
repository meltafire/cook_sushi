using Assets.Features.GameData.Scripts.Data;
using System;

namespace Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure
{
    public interface IRecepieAccountingDrawSignals
    {
        public event Action<bool> DisplayMakiRecepie;
        public event Action<bool> DisplayNigiriRecepie;
        public event Action<CookingIngridientType, int> DisplayIngridient;
        event Action AfterDisplayIngridient;
        public event Action<bool> DisplayWrapMaki;
        public event Action HideIngridient;
    }
}