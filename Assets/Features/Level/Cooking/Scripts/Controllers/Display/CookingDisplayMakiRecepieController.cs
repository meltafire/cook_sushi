using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public abstract class BaseCookingDisplayMakiRecepieController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
        public abstract void Show(bool isOn);
    }

    public class CookingDisplayMakiRecepieController : BaseCookingDisplayMakiRecepieController
    {
        private readonly ICookingDisplayMakiStartRecepieView _startView;
        private readonly ICookingDisplayMakiEndRecepieView _endView;

        public CookingDisplayMakiRecepieController(
            ICookingDisplayMakiStartRecepieView startView,
            ICookingDisplayMakiEndRecepieView endView)
        {
            _startView = startView;
            _endView = endView;
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
            _startView.Toggle(isOn);
            _endView.Toggle(isOn);
        }
    }
}