using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public enum CookingAction
    {
        Nigiri = DishType.Nigiri,
        Maki = DishType.Maki,
        UraMaki = DishType.UraMaki,

        WrapMaki = CookingManipulation.WrapMaki + 100,

        Rice = CookingStapleIngridientType.Rice + 200,
        Nori = CookingStapleIngridientType.Nori + 200,

        Cucumber = CookingIngridientType.Cucumber,
        Salmon = CookingIngridientType.Salmon,
        Tuna = CookingIngridientType.Tuna,

    }
}