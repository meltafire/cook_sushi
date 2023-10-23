using Assets.Utils.Controllers;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;

namespace Assets.Utils.ReflexIntegration
{
public abstract class ContainerWithDataFacade<U> : IControllerWithData<U>
{
    protected readonly Container Container;

    private Container _childContainer;

    public ContainerWithDataFacade(Container container)
    {
        Container = container;
    }

    public async UniTask Initialize(U data, CancellationToken token)
    {
        _childContainer = await GenerateContainer(data, token);

        await ActAfterContainerInitialized(data, token);
    }

    public void Dispose()
    {
        ActBeforeContainerDisposed();

        _childContainer.Dispose();

        ActAfterContainerDisposed();
    }

    protected T ResolveFromChildContainer<T>()
    {
        return _childContainer.Resolve<T>();
    }

    protected abstract UniTask<Container> GenerateContainer(U data, CancellationToken token);
    protected abstract UniTask ActAfterContainerInitialized(U data, CancellationToken token);
    protected abstract void ActBeforeContainerDisposed();
    protected abstract void ActAfterContainerDisposed();
    }
}