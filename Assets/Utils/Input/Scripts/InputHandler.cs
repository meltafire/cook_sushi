using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils.Input
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            var rayHitCollider = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue())).collider;

            if (!rayHitCollider)
            {
                return;
            }

            rayHitCollider.gameObject.GetComponent<IClickable>().HandleClick();
        }
    }
}