using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public abstract class BaseCookingIngridientsFacade : ContainerFacade
    {
        protected BaseCookingIngridientsFacade(Container container) : base(container)
        {
        }
    }

    public class CookingIngridientsFacade : BaseCookingIngridientsFacade
    {
        private static readonly string ContainerName = "CookingIngridientsFacade";

        private readonly CookingIngridientAssetInstantiator _ingridientAssetInstantiator;

        private IController _ingridientController;

        public CookingIngridientsFacade(
            CookingIngridientAssetInstantiator ingridientAssetInstantiator,
            Container container)
            : base(container)
        {
            _ingridientAssetInstantiator = ingridientAssetInstantiator;
        }

        protected override void ActAfterContainerDisposed()
        {
            _ingridientAssetInstantiator.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _ingridientController = ResolveFromChildContainer<BaseCookingIngridientController>();

            return _ingridientController.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _ingridientController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var ingridientView = await _ingridientAssetInstantiator.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(ingridientView, typeof(IngridientButtonView));
                descriptor.AddTransient(typeof(CookingIngridientController), typeof(BaseCookingIngridientController));
            });
        }
    }
}