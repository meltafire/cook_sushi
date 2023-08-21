using UnityEngine;

namespace Sushi.SceneReference
{
    public class SceneHandler : MonoBehaviour, ISceneReference
    {
        [SerializeField]
        private RectTransform _overlayCanvasTransform;

        public RectTransform OverlayCanvasTransform => _overlayCanvasTransform;
    }
}