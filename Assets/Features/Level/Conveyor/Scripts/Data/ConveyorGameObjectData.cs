using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public class ConveyorGameObjectData : IConveyorPointsProvider, IConveyorGameObjectData
    {
        private Vector3 _topStart;
        private Vector3 _topEnd;
        private Vector3 _bottomStart;
        private Vector3 _bottomEnd;

        public Vector3 TopStart => _topStart;
        public Vector3 TopEnd => _topEnd;
        public Vector3 BottomStart => _bottomStart;
        public Vector3 BottomEnd => _bottomEnd;

        public void SetData(
            Vector3 topStart,
            Vector3 topEnd,
            Vector3 bottomStart,
            Vector3 bottomEnd
            )
        {
            _topStart = topStart;
            _topEnd = topEnd;
            _bottomStart = bottomStart;
            _bottomEnd = bottomEnd;
        }
    }
}