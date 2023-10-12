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

        public int IngridientsCount => _drawedElements.Count;

        public event Action<bool> DisplayMakiRecepie;
        public event Action<bool> DisplayNigiriRecepie;
        public event Action<CookingIngridientType, int> DisplayIngridient;
        public event Action HideIngridient;
        public event Action<bool> DisplayWrapMaki;

        public void ShowIngridient(CookingAction scheme)
        {
            _drawedElements.Push(scheme);

            if (scheme == CookingAction.Maki)
            {
                DisplayMakiRecepie?.Invoke(true);
            }
            else if (scheme == CookingAction.Nigiri)
            {
                DisplayNigiriRecepie?.Invoke(true);
            }
        }

        public void ShowIngridient(CookingIngridientType scheme, int count)
        {
            var element = (CookingAction)scheme;

            _drawedElements.Push((CookingAction)scheme);

            DisplayIngridient?.Invoke(scheme, count);
        }

        public void ShowWrapMaki()
        {
            var element = CookingAction.WrapMaki;

            _drawedElements.Push(element);

            DisplayWrapMaki?.Invoke(true);
        }

        public void RevertIngridient()
        {
            var scheme = _drawedElements.Pop();

            if(scheme == CookingAction.Maki)
            {
                DisplayMakiRecepie?.Invoke(false);
            }
            else if(scheme == CookingAction.Nigiri)
            {
                DisplayNigiriRecepie?.Invoke(false);
            }
            else if(scheme == CookingAction.WrapMaki)
            {
                DisplayWrapMaki?.Invoke(false);
            }
            else
            {
                HideIngridient?.Invoke();
            }
        }
    }
}