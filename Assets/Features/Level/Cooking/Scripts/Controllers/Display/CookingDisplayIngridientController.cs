using Assets.Features.GameData.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public class CookingDisplayIngridientController : IController
{
    private readonly CookingDisplayIngridientView _view;

    public CookingDisplayIngridientController(
        CookingDisplayIngridientView view)
    {
        _view = view;
    }

    public UniTask Initialize(CancellationToken token)
    {
        Hide();

        return UniTask.CompletedTask;
    }

    public void Dispose()
    {
    }

    public void Hide()
    {
        _view.Toggle(false);
    }

    public void Show(CookingIngridientType step, int count)
    {
        _view.SetData(GetTextForStep(step), count);

        _view.Toggle(true);
    }

    private string GetTextForStep(CookingIngridientType step)
    {
        return step.ToString();
    }
}
