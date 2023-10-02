using Assets.Features.Level.Cooking.Scripts.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CookingStage : IStage
{
    private readonly ICookingControllerExternalEvents _cookingControllerExternalEvents;
    private readonly UniTaskCompletionSource _uniTaskCompletionSource;

    public CookingStage(ICookingControllerExternalEvents cookingControllerExternalEvents)
    {
        _cookingControllerExternalEvents = cookingControllerExternalEvents;

        _uniTaskCompletionSource = new UniTaskCompletionSource();
    }

    public UniTask Run(CancellationToken token)
    {
        _cookingControllerExternalEvents.RequestShow();

        _cookingControllerExternalEvents.PopupClosed += OnPopupClosed;

        return _uniTaskCompletionSource.Task;
    }

    private void OnPopupClosed()
    {
        _cookingControllerExternalEvents.PopupClosed -= OnPopupClosed;

        _uniTaskCompletionSource.TrySetResult();
    }
}