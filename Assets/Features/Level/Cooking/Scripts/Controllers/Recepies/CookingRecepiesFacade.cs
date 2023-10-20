using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public abstract class BaseCookingRecepiesFacade : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }

    public class CookingRecepiesFacade : BaseCookingRecepiesFacade
    {
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly MvcFacade<CookingMakiRecepieController, IngridientButtonView> _makiRecepieFacade;
        private readonly MvcFacade<CookingNigiriRecepieController, IngridientButtonView> _nigiriRecepieFacade;
        private readonly List<IController> _dynamicControllers = new List<IController>();

        public CookingRecepiesFacade(
            ILevelDishesTypeProvider levelDishesTypeProvider,
            MvcFacade<CookingMakiRecepieController,IngridientButtonView> makiRecepieFacade,
            MvcFacade<CookingNigiriRecepieController, IngridientButtonView> nigiriRecepieFacade)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _makiRecepieFacade = makiRecepieFacade;
            _nigiriRecepieFacade = nigiriRecepieFacade;
        }

        public override void Dispose()
        {
            foreach(var controller in _dynamicControllers)
            {
                controller.Dispose();
            }
        }

        public override UniTask Initialize(CancellationToken token)
        {
            var types = _levelDishesTypeProvider.GetLevelDishTypes();

            if(types.Contains(DishType.Maki))
            {
                _dynamicControllers.Add(_makiRecepieFacade);
            }

            if(types.Contains(DishType.Nigiri))
            {
                _dynamicControllers.Add(_nigiriRecepieFacade);
            }

            var loadingTasks = new List<UniTask>();

            foreach(var controller in _dynamicControllers)
            {
                loadingTasks.Add(controller.Initialize(token));
            }

            return UniTask.WhenAll(loadingTasks);
        }
    }
}