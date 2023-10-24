using Sushi.SceneReference;
using UnityEngine;

namespace Assets.Features.SceneReference
{
    public class SceneOverlayCanvasReference : MonoBehaviour, ISceneOverlayCanvasReference
    {
        [SerializeField]
        private RectTransform _overlayCanvasTransform;

        public Transform Transform => _overlayCanvasTransform;
    }
}