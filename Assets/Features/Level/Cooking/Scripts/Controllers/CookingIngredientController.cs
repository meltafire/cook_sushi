using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;
using Utils.Controllers;

public class CookingIngredientController : Controller
{
    private readonly CookingIngredientViewProvider _viewProvider;
    private readonly ILoadingStageControllerEvents _loadingStageControllerEvents;
    private readonly CookingIngridientType _cookingIngridientType;
    private readonly SpriteAtlas _ingridientsAtlas;
    private readonly Transform _parentTransform;

    private CookingIngredientView _view;
    private UniTaskCompletionSource _completionSource;

    protected override async UniTask Run(CancellationToken token)
    {
        _completionSource = new UniTaskCompletionSource();

        _loadingStageControllerEvents.LoadRequest += OnLoadRequested;

        await _completionSource.Task;
    }

    private void OnLoadRequested()
    {
        _loadingStageControllerEvents.LoadRequest -= OnLoadRequested;
        _loadingStageControllerEvents.ReportStartedLoading();

        LoadPrefab().Forget();
    }

    private async UniTask LoadPrefab()
    {
        AttachResource(_viewProvider);

        _view = await _viewProvider.Instantiate(_parentTransform);

        var sprite = _ingridientsAtlas.GetSprite(GetIngridientSpriteName());

        _view.SetSprite(sprite);
    }

    private string GetIngridientSpriteName()
    {
        switch (_cookingIngridientType)
        {
            case CookingIngridientType.Rice:
                return CookingConstantData.RiceSpriteName;

            case CookingIngridientType.Nori:
                return CookingConstantData.NoriSpriteName;

            case CookingIngridientType.Salmon:
                return CookingConstantData.SalmonSpriteName;

            case CookingIngridientType.Cucumber:
                return CookingConstantData.CucumberSpriteName;

            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
