using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Actions;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public abstract class BaseCookingMakiWrapActionController : IController
{
    public abstract void Dispose();
    public abstract UniTask Initialize(CancellationToken token);
    public abstract void Show(bool isOn);
    public abstract void MoveLast();
}

public class CookingMakiWrapActionController : BaseCookingMakiWrapActionController
{
    private readonly IRecepieAccounting _recepieAccounting;

    private readonly ICookingMakiWrapActionView _view;

    public CookingMakiWrapActionController(
        ICookingMakiWrapActionView view,
        IRecepieAccounting recepieAccounting)
    {
        _view = view;
        _recepieAccounting = recepieAccounting;
    }

    public override UniTask Initialize(CancellationToken token)
    {
        Show(false);

        _view.ButtonPressed += OnButtonPressed;

        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        _view.ButtonPressed -= OnButtonPressed;
    }

    public override void Show(bool isOn)
    {
        if(isOn)
        {
            MoveLast();
        }

        _view.Toggle(isOn);
    }

    public override void MoveLast()
    {
        _view.SetAsLastSibling();
    }

    private void OnButtonPressed()
    {
        _recepieAccounting.ShowWrapMaki();
    }
}
