using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Data
{
    public enum CookingStep
    {
        WrapMaki = CookingManipulation.WrapMaki + 100,

        Rice = CookingStapleIngridientType.Rice + 200,
        Nori = CookingStapleIngridientType.Nori + 200,

        Cucumber = CookingIngridientType.Cucumber + 300,
        Salmon = CookingIngridientType.Salmon + 300,
    }
}