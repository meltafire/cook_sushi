﻿using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Reflex.Core;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public class CookingNigiriRecepieFacade : UniqueVcFacade<CookingNigiriRecepieController, ButtonView>
    {
        private static readonly string ContainerNameInner = "CookingNigiriRecepieFacade";

        protected override string ContainerName => ContainerNameInner;

        public CookingNigiriRecepieFacade(
            AssetInstantiator<ButtonView> assetInstantiator,
            Container container) : base(assetInstantiator, container)
        {
        }
    }
}