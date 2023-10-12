using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public enum CookingAction
    {
        Nigiri = DishType.Nigiri,
        Maki = DishType.Maki,
        UraMaki = DishType.UraMaki,

        WrapMaki = CookingManipulation.WrapMaki,

        Rice = CookingStapleIngridientType.Rice,
        Nori = CookingStapleIngridientType.Nori,

        Cucumber = CookingIngridientType.Cucumber,
        Salmon = CookingIngridientType.Salmon,
        Tuna = CookingIngridientType.Tuna,

    }
}