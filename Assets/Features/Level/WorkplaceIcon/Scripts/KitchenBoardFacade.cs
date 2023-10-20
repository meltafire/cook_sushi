using Assets.Utils.Ui;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.WorkplaceIcon;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.WorkplaceIcon.Scripts
{
    public class KitchenBoardFacade : BaseKitchenBoardFacade
    {
        private static string ContainerName = "KitchenBoard";

        private readonly KitchenBoardProvider _kitchenBoardProvider;

        private IController _buttonController;

        public KitchenBoardFacade(Container container, KitchenBoardProvider kitchenBoardProvider) : base(container)
        {
            _kitchenBoardProvider = kitchenBoardProvider;
        }

        protected override void ActAfterContainerDisposed()
        {
            _kitchenBoardProvider.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _buttonController = ResolveFromChildContainer<BaseKitchenBoardController>();
            return _buttonController.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _buttonController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _kitchenBoardProvider.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(IButtonView));
                descriptor.AddTransient(typeof(KitchenBoardController), typeof(BaseKitchenBoardController));
            });
        }
    }
}