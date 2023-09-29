using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public interface ICookingControllerState
    {
        public UniTask Run(CancellationToken token);
    }
}