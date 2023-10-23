using UnityEngine;

namespace Sushi.SceneReference
{
    public class SceneHandler : MonoBehaviour, ISceneReference, IStageRootParentTransformProvider
    {
        [SerializeField]
        private Transform _stageParentTransform;
        [SerializeField]
        private RectTransform _overlayCanvasTransform;

        public RectTransform OverlayCanvasTransform => _overlayCanvasTransform;

        public Transform ParentTransform => _stageParentTransform;
    }
}