using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public enum CookingAction
    {
        Nigiri = DishType.Nigiri,
        Maki = DishType.Maki,
        UraMaki = DishType.UraMaki,

        WrapMaki = CookingManipulation.WrapMaki + 100,

        Rice = CookingIngridientType.Rice + 200,
        Nori = CookingIngridientType.Nori + 200,
        Cucumber = CookingIngridientType.Cucumber + 200,
        Salmon = CookingIngridientType.Salmon + 200,

    }
}