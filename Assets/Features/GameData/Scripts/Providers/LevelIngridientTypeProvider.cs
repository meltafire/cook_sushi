using Assets.Features.GameData.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Features.GameData.Scripts.Providers
{
    public class LevelIngridientTypeProvider: ILevelIngridientTypeProvider
    {
        private readonly HashSet<CookingIngridientType> _ingridientsTypes =
            new HashSet<CookingIngridientType>() { CookingIngridientType.Cucumber, CookingIngridientType.Tuna, CookingIngridientType.Salmon };

        public HashSet<CookingIngridientType> GetLevelIngridients()
        {
            return _ingridientsTypes;
        }
    }
}