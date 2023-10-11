using Sushi.Level.Cooking;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Pools
{
    public class DisplayIngridientsControllerPool : ControllerPool<CookingDisplayIngridientController>
    {
        public DisplayIngridientsControllerPool(IFactory<CookingDisplayIngridientController> factory) : base(factory)
        {
        }
    }
}