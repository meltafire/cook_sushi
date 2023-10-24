using Assets.Features.GameData.Scripts.Data;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public abstract class BaseCookingDisplayIngridientFacade : ContainerFacade
    {
        protected BaseCookingDisplayIngridientFacade(Container container) : base(container)
        {
        }

        public abstract void Hide();
        public abstract void Show(CookingIngridientType step, int count);
    }

    public class CookingDisplayIngridientFacade : BaseCookingDisplayIngridientFacade
    {
        private static readonly string ContainerName = "CookingDisplayIngridientFacade";

        private readonly AssetInstantiator<CookingDisplayIngridientView> _displayIngridientInstantiator;

        private BaseCookingDisplayIngridientController _controller;

        public CookingDisplayIngridientFacade(Container container) : base(container)
        {
        }

        public CookingDisplayIngridientFacade(
            Container container,
            AssetInstantiator<CookingDisplayIngridientView> displayIngridientInstantiator)
            : base(container)
        {
            _displayIngridientInstantiator = displayIngridientInstantiator;
        }

        public override void Hide()
        {
            _controller.Hide();
        }

        public override void Show(CookingIngridientType step, int count)
        {
            _controller.Show(step, count);
        }

        protected override void ActAfterContainerDisposed()
        {
            _displayIngridientInstantiator.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _controller = ResolveFromChildContainer<BaseCookingDisplayIngridientController>();

            return _controller.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _controller.Dispose();
        }

        protected override async UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _displayIngridientInstantiator.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(CookingDisplayIngridientView));
                descriptor.AddSingleton(typeof(CookingDisplayIngridientController), typeof(BaseCookingDisplayIngridientController));
            }
            );
        }
    }
}