using Assets.Features.GameData.Scripts.Data;
using System;

namespace Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure
{
    public interface IRecepieAccountingDrawSignals
    {
        public event Action<bool> DisplayMakiRecepie;
        public event Action<CookingIngridientType, int> DisplayIngridient;
        public event Action HideIngridient;
    }
}