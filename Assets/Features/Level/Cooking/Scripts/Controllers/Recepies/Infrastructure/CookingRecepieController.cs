using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public abstract class CookingRecepieController : IController
    {
        private readonly IRecipeSelectionEvents _events;
        private readonly ButtonView _view;

        public CookingRecepieController(
            ButtonView view,
            IRecipeSelectionEvents events)
        {
            _view = view;
            _events = events;
        }

        public UniTask Initialize(CancellationToken token)
        {
            _view.gameObject.SetActive(true);

            _view.ButtonPressed += OnButtonPressed;

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _view.ButtonPressed -= OnButtonPressed;
        }

        protected abstract DishType ReportData();

        private void OnButtonPressed()
        {
            _events.ReportSchemeChosen(ReportData());
        }
    }
}