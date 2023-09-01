using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Utils.Controllers
{
    public abstract class Controller : IBubbleEventListener
    {
        private int _id;

        protected Action<ControllerEvent> BubbleEvent;

        protected Controller()
        {
            _id = UnityEngine.Random.Range(0, 1000);
            Debug.Log($"ctr {_id}");
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

            Debug.Log($"done {_id}");
        }

        protected abstract UniTask Run(CancellationToken token);

        public virtual void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            BubbleEvent?.Invoke(controllerEvent);
        }
    }
}