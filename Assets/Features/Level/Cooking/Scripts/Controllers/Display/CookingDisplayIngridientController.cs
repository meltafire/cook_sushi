using Assets.Features.GameData.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public abstract class BaseCookingDisplayIngridientController : IController
{
    public abstract void Hide();
    public abstract void Show(CookingIngridientType step, int count);
    public abstract void Dispose();
    public abstract UniTask Initialize(CancellationToken token);
}


public class CookingDisplayIngridientController : BaseCookingDisplayIngridientController
{
    private readonly CookingDisplayIngridientView _view;

    public CookingDisplayIngridientController(
        CookingDisplayIngridientView view)
    {
        _view = view;
    }

    public override UniTask Initialize(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {

    }

    public override void Hide()
    {
        _view.Toggle(false);
    }

    public override void Show(CookingIngridientType step, int count)
    {
        _view.SetData(GetTextForStep(step), count);

        _view.Toggle(true);
    }

    private string GetTextForStep(CookingIngridientType step)
    {
        return step.ToString();
    }
}
