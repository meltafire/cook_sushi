using Assets.Features.Level.Conveyor.Scripts.Views;
using System;
using UnityEngine;

namespace Sushi.Level.Conveyor.Views
{
    public class ConveyorTileView : MonoBehaviour, IConveyorTileView
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private Transform _transform;

        public float SpriteLength => _spriteRenderer.bounds.size.x;
        public Vector3 Position => _transform.position;

        public event Action OnUpdate;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }
    }
}