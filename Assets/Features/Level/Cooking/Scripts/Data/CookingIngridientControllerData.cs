using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public class CookingIngridientControllerData
    {
        public readonly CookingIngridientType CookingIngridientType;
        public readonly int Count;

        public CookingIngridientControllerData(CookingIngridientType cookingIngridientType, int count)
        {
            CookingIngridientType = cookingIngridientType;
            Count = count;
        }
    }
}