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

        protected Action<ControllerEvent> BubbleEvent;

        protected void AttachResource(GameObject gameObject)
        {
            _resourseQueue.Enqueue(gameObject);
        }

        public async UniTask RunChild(Controller bubbleEventListener, CancellationToken token)
        {
            if (bubbleEventListener != null)
            {
                BubbleEvent += bubbleEventListener.OnBubbleEventHappen;
            }

            await Run(token);

            if (bubbleEventListener != null)
            {
                BubbleEvent -= bubbleEventListener.OnBubbleEventHappen;
            }

            RemoveResources();
        }

        protected abstract UniTask Run(CancellationToken token);

        protected virtual void HandleBubbleEvent(ControllerEvent controllerEvent)
        {
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