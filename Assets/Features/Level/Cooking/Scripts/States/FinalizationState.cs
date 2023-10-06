using Assets.Features.Level.Cooking.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class FinalizationState : ICookingControllerState
    {
        public UniTask<ControllerStatesType> Run(List<CookingAction> actions, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}