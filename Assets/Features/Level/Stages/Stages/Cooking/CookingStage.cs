using Assets.Features.Level.Cooking.Scripts.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CookingStage : IStage
{
    private readonly ICookingControllerExternalEvents _cookingControllerExternalEvents;

    private UniTaskCompletionSource _completionSource;

    public CookingStage(
        ICookingControllerExternalEvents cookingControllerExternalEvents
        )
    {
        _cookingControllerExternalEvents = cookingControllerExternalEvents;
    }

    public async UniTask Run(CancellationToken token)
    {
        _completionSource = new UniTaskCompletionSource();
        _cookingControllerExternalEvents.BackButtonClicked += OnBackButtonClicked;

        _cookingControllerExternalEvents.RequestShow(true);

        await _completionSource.Task;

        _cookingControllerExternalEvents.BackButtonClicked -= OnBackButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        _cookingControllerExternalEvents.RequestShow(false);

        _completionSource.TrySetResult();
    }
}
