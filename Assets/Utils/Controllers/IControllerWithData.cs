using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Utils.Controllers
{
    public interface IControllerWithData<U>
    {
        public UniTask Initialize(U data,CancellationToken token);
        public void Dispose();
    }
}