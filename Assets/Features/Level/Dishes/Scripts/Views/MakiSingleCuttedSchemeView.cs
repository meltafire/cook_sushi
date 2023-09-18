using UnityEngine;

namespace Sushi.Level.Dishes
{
    public class MakiSingleCuttedSchemeView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        public void NestChild(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }
    }
}
