using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public abstract class BaseCookingIngridientController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }

    public class CookingIngridientController : BaseCookingIngridientController
    {
        private readonly CookingIngridientControllerData _data;
        private readonly IRecepieAccounting _drawer;
        private readonly IngridientButtonView _view;

        public CookingIngridientController(
            CookingIngridientControllerData data,
            IngridientButtonView view,
            IRecepieAccounting drawer)
        {
            _data = data;
            _drawer = drawer;
            _view = view;
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _view.SetText(_data.CookingIngridientType.ToString());

            _view.ButtonPressed += OnButtonPressed;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _view.ButtonPressed -= OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            _drawer.ShowIngridient(_data.CookingIngridientType, _data.Count);
        }
    }
}