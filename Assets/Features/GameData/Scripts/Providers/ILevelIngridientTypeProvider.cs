using Assets.Features.GameData.Scripts.Data;
using System.Collections.Generic;

namespace Assets.Features.GameData.Scripts.Providers
{
    public interface ILevelIngridientTypeProvider
    {
        public HashSet<CookingIngridientType> GetLevelIngridients();
    }
}