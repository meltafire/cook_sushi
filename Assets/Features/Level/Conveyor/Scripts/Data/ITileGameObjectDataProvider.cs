using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public interface ITileGameObjectDataProvider
    {
        public GameObject GameObject { get; }
        public Transform TilesParentTransform { get; }
    }
}