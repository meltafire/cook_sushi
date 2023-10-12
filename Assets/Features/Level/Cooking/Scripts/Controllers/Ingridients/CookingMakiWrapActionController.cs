using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

public class CookingMakiWrapActionController : ResourcefulController
{
    private readonly CookingMakiWrapActionInstantiator _instantiator;
    private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
    private readonly IRecepieAccounting _recepieAccounting;

    private ButtonView _view;

    public CookingMakiWrapActionController(
        CookingMakiWrapActionInstantiator instantiator,
        IIngridientsDispalyParentTransformProvider parentTransformProvider,
        IRecepieAccounting recepieAccounting)
    {
        _instantiator = instantiator;
        _parentTransformProvider = parentTransformProvider;
        _recepieAccounting = recepieAccounting;
    }

    public override async UniTask Initialzie(CancellationToken token)
    {
        await LoadPrefab();

        _view.ButtonPressed += OnButtonPressed;
    }

    public override void Dispose()
    {
        _view.ButtonPressed -= OnButtonPressed;

        base.Dispose();
    }

    public void Show(bool isOn)
    {
        if(isOn)
        {
            MoveLast();
        }

        _view.Toggle(isOn);
    }

    public void MoveLast()
    {
        _view.transform.SetAsLastSibling();
    }

    private async UniTask LoadPrefab()
    {
        AttachResource(_instantiator);

        _view = await _instantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

        Show(false);
    }

    private void OnButtonPressed()
    {
        _recepieAccounting.ShowWrapMaki();
    }
}
