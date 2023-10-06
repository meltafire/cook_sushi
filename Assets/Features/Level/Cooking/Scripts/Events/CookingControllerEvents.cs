using Assets.Features.GameData.Scripts.Data;
using System;
using System.Collections.Generic;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public class CookingControllerEvents :
        ICookingControllerEvents,
        ICookingControllerExternalEvents,
        ICookingControllerBackButtonExternalEvents
    {
        public event Action ShowRequest;
        public event Action<HashSet<DishType>> ShowDishSelectionRequest;
        public event Action HideDishSelectionRequest;
        public event Action<DishType> DishSelected;
        public event Action PopupClosed;
        public event Action<bool> ToggleBackButton;

        public void RequestShow()
        {
            ShowRequest?.Invoke();
        }

        public void ShowDishSelection(HashSet<DishType> set)
        {
            ShowDishSelectionRequest?.Invoke(set);
        }

        public void HideDishSelection()
        {
            HideDishSelectionRequest?.Invoke();
        }

        public void ReportDishSelected(DishType dishType)
        {
            DishSelected?.Invoke(dishType);
        }

        public void ReportPopupClosed()
        {
            PopupClosed?.Invoke();
        }

        public void RequestToggleBackButton(bool isOn)
        {
            ToggleBackButton?.Invoke(isOn);
        }
    }
}