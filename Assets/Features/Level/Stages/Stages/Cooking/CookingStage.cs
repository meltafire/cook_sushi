using Assets.Features.Level.Cooking.Scripts.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CookingStage : IStage
{
    private readonly ICookingControllerExternalEvents _cookingControllerExternalEvents;
    private readonly ICookingControllerBackButtonExternalEvents _backButtonExternalEvents;

    private CancellationTokenSource _cancellationTokenSource;

    public CookingStage(ICookingControllerExternalEvents cookingControllerExternalEvents, ICookingControllerBackButtonExternalEvents backButtonExternalEvents)
    {
        _cookingControllerExternalEvents = cookingControllerExternalEvents;
        _backButtonExternalEvents = backButtonExternalEvents;
    }

    public async UniTask Run(CancellationToken token)
    {
        _cookingControllerExternalEvents.RequestShow(true);

        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

        _backButtonExternalEvents.RequestToggleBackButton(false);
        _cookingControllerExternalEvents.BackButtonClicked += OnBackButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        _cookingControllerExternalEvents.BackButtonClicked -= OnBackButtonClicked;

        _cancellationTokenSource.Cancel();

        _cookingControllerExternalEvents.RequestShow(false);
    }
}