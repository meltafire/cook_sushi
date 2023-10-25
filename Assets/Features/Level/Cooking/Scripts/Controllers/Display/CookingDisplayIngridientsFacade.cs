using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Pools;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Cooking;
using System.Collections.Generic;
using System.Threading;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public class CookingDisplayIngridientsFacade : ContainerFacade
    {
        private static readonly string ContainerName = "CookingDisplayIngridientFacade";

        private readonly IRecepieAccountingDrawSignals _ingridientSelectionExternalEvent;
        private readonly Stack<BaseCookingDisplayIngridientFacade> _ingridientDisplayFacades= new Stack<BaseCookingDisplayIngridientFacade>();

        private Pool<BaseCookingDisplayIngridientFacade> _pool;
        private CancellationToken _token;

        public CookingDisplayIngridientsFacade(
            IRecepieAccountingDrawSignals ingridientSelectionExternalEvent,
            Container container)
            : base(container)
        {
            _ingridientSelectionExternalEvent = ingridientSelectionExternalEvent;
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _token = token;
            _pool = ResolveFromChildContainer<Pool<BaseCookingDisplayIngridientFacade>>();

            _ingridientSelectionExternalEvent.DisplayIngridient += OnIngridientSelected;
            _ingridientSelectionExternalEvent.HideIngridient += OnHideIngridient;

            return _pool.Initialize(token);;
        }

        protected override void ActBeforeContainerDisposed()
        {
            _ingridientSelectionExternalEvent.DisplayIngridient -= OnIngridientSelected;
            _ingridientSelectionExternalEvent.HideIngridient -= OnHideIngridient;

            while (_ingridientDisplayFacades.Count > 0)
            {
                var facade = _ingridientDisplayFacades.Pop();

                _pool.Release(facade);
            }

            _pool.Dispose();
        }

        protected override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            return UniTask.FromResult(
                Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddTransient(typeof(CookingDisplayIngridientInstantiator), typeof(AssetInstantiator<CookingDisplayIngridientView>));

                descriptor.AddTransient(typeof(CookingDisplayIngridientFacade), typeof(BaseCookingDisplayIngridientFacade));
                descriptor.AddSingleton(typeof(DisplayIngridientsControllerPool), typeof(Pool<BaseCookingDisplayIngridientFacade>));
            }
            ));
        }

        private void OnIngridientSelected(CookingIngridientType type, int count)
        {
            var facade = _pool.Get();

            facade.Show(type, count);

            _ingridientDisplayFacades.Push(facade);
        }

        private void OnHideIngridient()
        {
            var facade = _ingridientDisplayFacades.Pop();

            facade.Hide();

            _pool.Release(facade);
        }
    }
}