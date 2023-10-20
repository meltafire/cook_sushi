using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public abstract class BaseCookingDisplayNigiriRecepieController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
        public abstract void Show(bool isOn);
    }

    public class CookingDisplayNigiriRecepieController : BaseCookingDisplayNigiriRecepieController
    {
        private readonly ICookingDisplayNigiriRecepieView _view;

        public CookingDisplayNigiriRecepieController(
            ICookingDisplayNigiriRecepieView view)
        {
            _view = view;
        }

        public override void Dispose()
        {
        }

        public override UniTask Initialize(CancellationToken token)
        {
            Show(false);

            return UniTask.CompletedTask;
        }

        public override void Show(bool isOn)
        {
            _view.Toggle(isOn);
        }
    }
}