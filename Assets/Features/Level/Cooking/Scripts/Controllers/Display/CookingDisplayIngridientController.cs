using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public class CookingDisplayIngridientController : ResourcefulController
{
    private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
    private readonly CookingDisplayIngridientProvider _displayIngridientProvider;
    private readonly IIngridientDisplayEvents _events;

    private CookingDisplayIngridientView _view;

    public CookingDisplayIngridientController(
        IIngridientsDispalyParentTransformProvider parentTransformProvider,
        CookingDisplayIngridientProvider displayIngridientProvider,
        IIngridientDisplayEvents events)
    {
        _parentTransformProvider = parentTransformProvider;
        _displayIngridientProvider = displayIngridientProvider;
        _events = events;
    }

    public override async UniTask Initialzie(CancellationToken token)
    {
        await LoadPrefab();

        _events.ShowRequest += OnShowRequest;
        _events.HideRequest += OnHideRequest;
    }

    public override void Dispose()
    {
        _events.ShowRequest -= OnShowRequest;
        _events.HideRequest -= OnHideRequest;

        base.Dispose();
    }

    private void OnHideRequest()
    {
        _view.Toggle(false);
    }

    private void OnShowRequest(CookingStep step, int count)
    {
        _view.SetData(GetTextForStep(step), count);
    }

    private string GetTextForStep(CookingStep step)
    {
        return step.ToString();
    }

    private async UniTask LoadPrefab()
    {
        AttachResource(_displayIngridientProvider);

        _view = await _displayIngridientProvider.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

        _view.Toggle(false);
    }
}
