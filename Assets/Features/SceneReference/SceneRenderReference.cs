using UnityEngine;

namespace Sushi.SceneReference
{
    public class SceneRenderReference : MonoBehaviour, ISceneRenderReference
    {
        [SerializeField]
        private Transform _stageParentTransform;

        public Transform Transform => _stageParentTransform;
    }
}