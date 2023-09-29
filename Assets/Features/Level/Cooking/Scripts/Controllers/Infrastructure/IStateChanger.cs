using Assets.Features.Level.Cooking.Scripts.States;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Infrastructure
{
    public interface IStateChanger
    {
        public void SetState(ICookingControllerState state);
    }
}