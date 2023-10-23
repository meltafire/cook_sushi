using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public abstract class BaseCookingRecepiesFacade : ContainerFacade
    {
        protected BaseCookingRecepiesFacade(Container container) : base(container)
        {
        }
    }

    public class CookingRecepiesFacade : BaseCookingRecepiesFacade
    {
        private static readonly string ContainerName = "CookingRecepiesFacade";

        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly List<IController> _dynamicControllers = new List<IController>();

        private UniqueVcFacade<CookingMakiRecepieController, IngridientButtonView> _makiRecepieFacade;
        private UniqueVcFacade<CookingNigiriRecepieController, IngridientButtonView> _nigiriRecepieFacade;

        public CookingRecepiesFacade(
            ILevelDishesTypeProvider levelDishesTypeProvider,
            Container container)
            : base(container)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _makiRecepieFacade = ResolveFromChildContainer<UniqueVcFacade<CookingMakiRecepieController, IngridientButtonView>>();
            _nigiriRecepieFacade = ResolveFromChildContainer<UniqueVcFacade<CookingNigiriRecepieController, IngridientButtonView>>();

            return InitializeInternal(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            foreach (var controller in _dynamicControllers)
            {
                controller.Dispose();
            }
        }

        protected override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            return UniTask.FromResult(
                Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddTransient(typeof(CookingMakiRecepieFacade), typeof(UniqueVcFacade<CookingMakiRecepieController, IngridientButtonView>));
                descriptor.AddTransient(typeof(CookingNigiriRecepieFacade), typeof(UniqueVcFacade<CookingNigiriRecepieController, IngridientButtonView>));
            })
                );
        }

        private UniTask InitializeInternal(CancellationToken token)
        {
            var types = _levelDishesTypeProvider.GetLevelDishTypes();

            if (types.Contains(DishType.Maki))
            {
                _dynamicControllers.Add(_makiRecepieFacade);
            }

            if (types.Contains(DishType.Nigiri))
            {
                _dynamicControllers.Add(_nigiriRecepieFacade);
            }

            var loadingTasks = new List<UniTask>();

            foreach (var controller in _dynamicControllers)
            {
                loadingTasks.Add(controller.Initialize(token));
            }

            return UniTask.WhenAll(loadingTasks);
        }
    }
}