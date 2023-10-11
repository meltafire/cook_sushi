using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public class CookingDisplayIngridientController : ResourcefulController
{
    private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
    private readonly CookingDisplayIngridientInstantiator _displayIngridientProvider;

    private CookingDisplayIngridientView _view;

    public CookingDisplayIngridientController(
        IIngridientsDispalyParentTransformProvider parentTransformProvider,
        CookingDisplayIngridientInstantiator displayIngridientProvider)
    {
        _parentTransformProvider = parentTransformProvider;
        _displayIngridientProvider = displayIngridientProvider;
    }

    public override async UniTask Initialzie(CancellationToken token)
    {
        await LoadPrefab();
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

    private async UniTask LoadPrefab()
    {
        AttachResource(_displayIngridientProvider);

        _view = await _displayIngridientProvider.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

        Hide();
    }
}
