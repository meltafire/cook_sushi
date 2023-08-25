using Sushi.Level.Conveyor.Views;
using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public class TileGameObjectData : ITileGameObjectData, ITileGameObjectDataProvider, ITileGameObjectDimensionProvider
    {
        private float _artLength;

        public GameObject GameObject { get; private set; }
        public Transform TilesParentTransform { get; set; }

        public float TileLength => _artLength;

        public void SetIileGameObject(GameObject gameObject, float artLength)
        {
            GameObject = gameObject;

            _artLength = artLength;
        }
    }
}