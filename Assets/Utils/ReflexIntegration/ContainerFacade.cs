using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.Controllers;

public abstract class ContainerFacade : IController
{
    protected readonly Container Container;

    private Container _childContainer;

    public ContainerFacade(Container container)
    {
        Container = container;
    }

    public async UniTask Initialize(CancellationToken token)
    {
        _childContainer = await GenerateContainer(token);

        await ActAfterContainerInitialized(token);
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

    protected abstract UniTask<Container> GenerateContainer(CancellationToken token);
    protected abstract UniTask ActAfterContainerInitialized(CancellationToken token);
    protected abstract void ActBeforeContainerDisposed();
    protected abstract void ActAfterContainerDisposed();
}
