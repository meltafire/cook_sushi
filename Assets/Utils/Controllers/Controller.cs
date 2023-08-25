using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Utils.Controllers
{
    public abstract class Controller
    {
        protected Action<ControllerEvent> BubbleEvent;

        protected UniTask RunChild<T>(IFactory<T> controllerFactory, CancellationToken token) where T : Controller
        {
            var controller = controllerFactory.Create();

            return RunChild(controller, token);
        }

        protected async UniTask RunChild<T>(T controller, CancellationToken token) where T : Controller
        {
            controller.BubbleEvent += OnBubbleEventHappen;

            await controller.Run(token);

            controller.BubbleEvent -= OnBubbleEventHappen;
        }

        protected abstract UniTask Run(CancellationToken token);

        protected virtual void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            BubbleEvent?.Invoke(controllerEvent);
        }
    }
}