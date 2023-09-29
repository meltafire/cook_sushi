using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Utils.AddressablesLoader;

namespace Utils.Controllers
{
    public abstract class ResourcefulController : IController
    {
        private readonly Queue<IAssetUnloader> _resourseQueue = new Queue<IAssetUnloader>();

        public abstract UniTask Initialzie(CancellationToken token);

        public virtual void Dispose()
        {
            DisposeResources();
        }

        protected void AttachResource(IAssetUnloader unloader)
        {
            _resourseQueue.Enqueue(unloader);
        }

        private void DisposeResources()
        {
            while(_resourseQueue.Count > 0)
            {
                var resource = _resourseQueue.Dequeue();
                resource.Unload();
            }
        }
    }

    public interface ILaunchableController : IController
    {
        public UniTask Launch(CancellationToken token);
    }

    public interface IController
    {
        public UniTask Initialzie(CancellationToken token);
        public void Dispose();
    }
}