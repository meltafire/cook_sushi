using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public abstract class BaseCookingDisplayMakiWrapController : IController
{
    public abstract void Show(bool isOn);
    public abstract void Dispose();
    public abstract UniTask Initialize(CancellationToken token);
}

public class CookingDisplayMakiWrapController : BaseCookingDisplayMakiWrapController
{
    private readonly ICookingDisplayMakiWrapView _view;

    public CookingDisplayMakiWrapController(
        ICookingDisplayMakiWrapView view)
    {
        _view = view;
    }

    public override void Dispose()
    {
    }

    public override UniTask Initialize(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    public override void Show(bool isOn)
    {
        _view.Toggle(isOn);

        _view.SetAsLastSibling();
    }
}
