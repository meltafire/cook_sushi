using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public interface IConveyorGameObjectData
    {
        public void SetData(
            Vector3 topStart,
            Vector3 topEnd,
            Vector3 bottomStart,
            Vector3 bottomEnd);
    }
}