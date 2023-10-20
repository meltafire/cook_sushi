using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Menu.Scripts.Controllers
{
    public abstract class BaseMenuController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }
}