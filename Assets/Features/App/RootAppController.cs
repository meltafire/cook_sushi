using Reflex.Core;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App
{
    public class RootAppController : IStartable
    {
        private readonly IFactory<AppController> _appControllerFactory;
        private readonly CancellationToken _cancellationToken;

        public RootAppController(CancellationToken cancellationToken, IFactory<AppController> appControllerFactory)
        {
            _appControllerFactory = appControllerFactory;
            _cancellationToken = cancellationToken;
        }

        public async void Start()
        {
            await _appControllerFactory.Create().RunChild(null, _cancellationToken);
        }
    }
}
