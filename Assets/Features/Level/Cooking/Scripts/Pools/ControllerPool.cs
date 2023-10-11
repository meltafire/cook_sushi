using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public abstract class ControllerPool<T> where T : IController
    {
        private const int DefaultSize = 15;

        private readonly IFactory<T> _factory;
        private readonly Stack<T> _stack = new Stack<T>(DefaultSize);

        public ControllerPool(IFactory<T> factory)
        {
            _factory = factory;
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

        public async UniTask<T> Get(CancellationToken token)
        {
            if (_stack.Count == 0)
            {
                await GenerateItem(token);
            }

            return _stack.Pop();
        }

        public void Release(T controller)
        {
            _stack.Push(controller);
        }

        private UniTask GenerateItem(CancellationToken token)
        {
            var controller = _factory.Create();
            _stack.Push(controller);

            return controller.Initialzie(token);
        }
    }
}