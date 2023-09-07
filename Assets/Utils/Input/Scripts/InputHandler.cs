using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils.Input
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _camera;

        private RaycastHit2D[] _results;
        private int _layerIgnoreRaycast;

        private void Awake()
        {
            _layerIgnoreRaycast = LayerMask.NameToLayer("RaycastLock");
            _camera = Camera.main;
            _results = new RaycastHit2D[1];
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            var hits = Physics2D.GetRayIntersectionNonAlloc(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), _results);

            if (hits == 0)
            {
                return;
            }

            var rayHitCollider = _results[0].collider;

            if (!rayHitCollider)
            {
                return;
            }

            var gameObject = rayHitCollider.gameObject;

            if (gameObject.layer == _layerIgnoreRaycast)
            {
                return;
            }

            rayHitCollider.gameObject.GetComponent<IClickable>().HandleClick();
        }
    }
}