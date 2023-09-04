using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Utils.Controllers
{
    public abstract class Controller
    {
        private readonly Queue<GameObject> _resourseQueue = new Queue<GameObject>();

        private Action<ControllerEvent> BubbleEvent;

        public UniTask RunChild(CancellationToken token)
        {
            return RunInternal(token);
        }

        public async UniTask RunChildFromFactory<T>(IFactory<T> controllerFactory, CancellationToken token) where T : Controller
        {
            var controllerToRun = controllerFactory.Create();

            controllerToRun.BubbleEvent += OnBubbleEventHappen;

            await controllerToRun.RunInternal(token);

            controllerToRun.BubbleEvent -= OnBubbleEventHappen;
        }

        public async UniTask RunChildFromFactory<T, U>(IFactoryWithData<T, U> controllerFactory, U data, CancellationToken token)
            where T : Controller
            where U : FactoryData
        {
            var controllerToRun = controllerFactory.Create(data);

            controllerToRun.BubbleEvent += OnBubbleEventHappen;

            await controllerToRun.RunInternal(token);

            controllerToRun.BubbleEvent -= OnBubbleEventHappen;
        }

        protected void InvokeBubbleEvent(ControllerEvent controllerEvent)
        {
            BubbleEvent?.Invoke(controllerEvent);
        }

        protected void AttachResource(GameObject gameObject)
        {
            _resourseQueue.Enqueue(gameObject);
        }

        protected abstract UniTask Run(CancellationToken token);

        protected virtual void HandleBubbleEvent(ControllerEvent controllerEvent)
        {
        }

        private async UniTask RunInternal(CancellationToken token)
        {
            await Run(token);

            RemoveResources();
        }

        private void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            HandleBubbleEvent(controllerEvent);
            BubbleEvent?.Invoke(controllerEvent);
        }

        private void RemoveResources()
        {
            if (_resourseQueue.Count > 0)
            {
                GameObject.Destroy(_resourseQueue.Dequeue());
            }
        }
    }
}