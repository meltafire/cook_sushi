using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public abstract class Pool<T> where T : IController
    {
        private const int DefaultSize = 15;

        private readonly Container _container;
        private readonly Stack<T> _stack = new Stack<T>(DefaultSize);

        public Pool(Container container)
        {
            _container = container;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            var taskList = new List<UniTask>(DefaultSize);

            for (var i = 0; i < DefaultSize; i++)
            {
                taskList.Add(GenerateItem(token));
            }

            await UniTask.WhenAll(taskList);
        }

        public void Dispose()
        {
            while (_stack.Count > 0)
            {
                var controller = _stack.Pop();

                controller.Dispose();
            }
        }

        public T Get()
        {
            var controller = _stack.Pop();

            return controller;
        }

        public void Release(T controller)
        {
            _stack.Push(controller);
        }

        private UniTask GenerateItem(CancellationToken token)
        {
            var controller = _container.Resolve<T>();
            _stack.Push(controller);

            return controller.Initialize(token);
        }
    }
}