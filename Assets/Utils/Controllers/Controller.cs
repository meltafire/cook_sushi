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
        private Action<ControllerEvent> DivingEvent;

        public UniTask RunChild(CancellationToken token)
        {
            return RunInternalLogic(token);
        }

        public UniTask RunChildFromFactory<T>(IFactory<T> controllerFactory, CancellationToken token) where T : Controller
        {
            var controllerToRun = controllerFactory.Create();

            return RunChildInternal(controllerToRun, token);
        }

        public UniTask RunChildFromFactory<T, U>(IFactoryWithData<T, U> controllerFactory, U data, CancellationToken token)
            where T : Controller
            where U : FactoryData
        {
            var controllerToRun = controllerFactory.Create(data);

            return RunChildInternal(controllerToRun, token);
        }

        protected void InvokeBubbleEvent(ControllerEvent controllerEvent)
        {
            BubbleEvent?.Invoke(controllerEvent);
        }

        protected void InvokeDivingEvent(ControllerEvent controllerEvent)
        {
            DivingEvent?.Invoke(controllerEvent);
        }

        protected void AttachResource(GameObject gameObject)
        {
            _resourseQueue.Enqueue(gameObject);
        }

        protected abstract UniTask Run(CancellationToken token);

        protected virtual void HandleBubbleEvent(ControllerEvent controllerEvent)
        {
        }

        protected virtual void HandleDivingEvent(ControllerEvent controllerEvent)
        {
        }

        private async UniTask RunChildInternal(Controller controllerToRun, CancellationToken token)
        {
            controllerToRun.BubbleEvent += OnBubbleEventHappen;

            await controllerToRun.RunInternal(this, token);

            controllerToRun.BubbleEvent -= OnBubbleEventHappen;
        }

        private async UniTask RunInternal(Controller parentController, CancellationToken token)
        {
            if(parentController != null)
            {
                parentController.DivingEvent += OnDivingEventHappen;
            }

            await RunInternalLogic(token);

            if(parentController != null)
            {
                parentController.DivingEvent -= OnDivingEventHappen;
            }
        }

        private async UniTask RunInternalLogic(CancellationToken token)
        {
            await Run(token);

            RemoveResources();
        }

        private void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            HandleBubbleEvent(controllerEvent);
            BubbleEvent?.Invoke(controllerEvent);
        }

        private void OnDivingEventHappen(ControllerEvent controllerEvent)
        {
            HandleDivingEvent(controllerEvent);
            DivingEvent?.Invoke(controllerEvent);
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