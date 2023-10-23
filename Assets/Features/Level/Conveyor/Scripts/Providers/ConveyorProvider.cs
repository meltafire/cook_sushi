﻿using Assets.Features.Level.Conveyor.Scripts.Providers;
using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using Utils.AddressablesLoader;

namespace Sushi.Level.Conveyor
{
    public class ConveyorProvider : AssetInstantiator<ConveyorView>
    {
        private readonly IStageRootParentTransformProvider _transformProvider;

        public ConveyorProvider(IStageRootParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<ConveyorView> Load()
        {
            return Instantiate(ConveyorConstants.ConveyorPrefabName, _transformProvider.ParentTransform);
        }
    }
}