using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieAccounting : IRecepieAccounting, IRecepieAccountingDrawSignals
    {
        private readonly Stack<CookingAction> _drawedElements = new Stack<CookingAction>();

        public event Action<bool> DisplayMakiRecepie;
        public event Action<CookingIngridientType, int> DisplayIngridient;
        public event Action HideIngridient;

        public void ShowIngridient(CookingAction scheme)
        {
            Debug.Log($"push {scheme}");

            _drawedElements.Push(scheme);

            Toggle(scheme, true);
        }

        public void ShowIngridient(CookingIngridientType scheme, int count)
        {
            var element = (CookingAction)scheme;

            Debug.Log($"push {element} base was {scheme}");

            _drawedElements.Push((CookingAction)scheme);

            DisplayIngridient?.Invoke(scheme, count);
        }

        public void RevertIngridient()
        {
            var scheme = _drawedElements.Pop();

            Debug.Log($"pop {scheme}");

            Toggle(scheme, false);
        }

        private void Toggle(CookingAction scheme, bool isOn)
        {
            if(scheme == CookingAction.Maki)
            {
                DisplayMakiRecepie?.Invoke(isOn);
            }
            else if (!isOn)
            {
                HideIngridient?.Invoke();
            }
        }
    }
}