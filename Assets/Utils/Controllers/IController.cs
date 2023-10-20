using Cysharp.Threading.Tasks;
using System.Threading;

namespace Utils.Controllers
{    public interface IController
    {
        public UniTask Initialize(CancellationToken token);
        public void Dispose();
    }
}