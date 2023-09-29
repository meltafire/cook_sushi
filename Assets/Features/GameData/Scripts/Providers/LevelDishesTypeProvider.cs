using System.Collections.Generic;

namespace Assets.Features.GameData.Scripts.Providers
{
    public class LevelDishesTypeProvider : ILevelDishesTypeProvider
    {
        private readonly HashSet<DishType> _dishTypes = new HashSet<DishType>() { DishType.Nigiri, DishType.Maki };

        public HashSet<DishType> GetLevelDishTypes()
        {
            return _dishTypes;
        }
    }
}