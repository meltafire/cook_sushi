using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public class CookingIngridientControllerData
    {
        public readonly CookingIngridientType CookingIngridientType;

        public CookingIngridientControllerData(CookingIngridientType cookingIngridientType)
        {
            CookingIngridientType = cookingIngridientType;
        }
    }
}