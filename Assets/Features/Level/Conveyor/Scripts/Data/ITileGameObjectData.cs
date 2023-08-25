using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public interface ITileGameObjectData
    {
        public Transform TilesParentTransform { set; }
        public void SetIileGameObject(GameObject gameObject, float artLength);
    }
}