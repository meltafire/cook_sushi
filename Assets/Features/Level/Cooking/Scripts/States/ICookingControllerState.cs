using Assets.Features.Level.Cooking.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public interface ICookingControllerState
    {
        public UniTask<ControllerStatesType> Run(Stack<CookingAction> actions, CancellationToken token);
    }
}