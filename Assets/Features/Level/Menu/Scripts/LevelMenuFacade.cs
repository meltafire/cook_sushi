using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Menu;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Menu.Scripts
{
    public abstract class BaseLevelMenuFacade : ContainerFacade
    {
        protected BaseLevelMenuFacade(Container container) : base(container)
        {
        }
    }

    public class LevelMenuFacade : BaseLevelMenuFacade
    {
        private static readonly string ContainerName = "LevelMenuFacade";

        private readonly LevelMenuProvider _levelMenuProvider;

        private IController _levelMenuController;

        public LevelMenuFacade(LevelMenuProvider levelMenuProvider, Container container) : base(container)
        {
            _levelMenuProvider = levelMenuProvider;
        }

        protected override void ActAfterContainerDisposed()
        {
            _levelMenuProvider.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _levelMenuController = ResolveFromChildContainer<BaseLevelMenuController>();

            return _levelMenuController.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _levelMenuController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _levelMenuProvider.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(LevelMenuView));
                descriptor.AddTransient(typeof(LevelMenuController), typeof(BaseLevelMenuController));
            });
        }
    }
}