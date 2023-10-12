using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

public class CookingDisplayMakiWrapController : ResourcefulController
{
    private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
    private readonly CookingDisplayMakiWrapInstantiator _displayMakiWrapInstantiator;

    private CookingDisplayRecepieView _view;

    public CookingDisplayMakiWrapController(
        IIngridientsDispalyParentTransformProvider parentTransformProvider,
        CookingDisplayMakiWrapInstantiator displayMakiWrapInstantiator)
    {
        _parentTransformProvider = parentTransformProvider;
        _displayMakiWrapInstantiator = displayMakiWrapInstantiator;
    }

    public override async UniTask Initialzie(CancellationToken token)
    {
        await LoadPrefab();
    }

    public void Show(bool isOn)
    {
        _view.Toggle(isOn);
        _view.transform.SetAsLastSibling();
    }

    private async UniTask LoadPrefab()
    {
        AttachResource(_displayMakiWrapInstantiator);

        _view = await _displayMakiWrapInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

        Show(false);
    }
}
