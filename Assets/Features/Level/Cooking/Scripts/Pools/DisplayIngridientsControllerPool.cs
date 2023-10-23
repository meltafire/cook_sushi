using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Reflex.Core;
using Sushi.Level.Cooking;

namespace Assets.Features.Level.Cooking.Scripts.Pools
{
    public class DisplayIngridientsControllerPool : Pool<BaseCookingDisplayIngridientFacade>
    {
        public DisplayIngridientsControllerPool(Container container) : base(container)
        {
        }
    }
}