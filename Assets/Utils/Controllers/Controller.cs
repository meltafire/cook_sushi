using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Utils.Controllers
{
    public abstract class Controller : IBubbleEventListener
    {
        private readonly Queue<GameObject> _resourseQueue = new Queue<GameObject>();

        protected Action<ControllerEvent> BubbleEvent;

        public void AttachResource(GameObject gameObject)
        {
            _resourseQueue.Enqueue(gameObject);
        }

        public async UniTask RunChild(IBubbleEventListener bubbleEventListener, CancellationToken token)
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

        public virtual void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            BubbleEvent?.Invoke(controllerEvent);
        }

        protected abstract UniTask Run(CancellationToken token);

        private void RemoveResources()
        {
            if (_resourseQueue.Count > 0)
            {
                GameObject.Destroy(_resourseQueue.Dequeue());
            }
        }
    }
}