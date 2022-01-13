using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Wall : MonoBehaviour
	{
		public Sprite damageSprite;
		int hp = 3;

		SpriteRenderer _renderer;

        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void DamageWall(int loss)
        {
            _renderer.sprite = damageSprite;
            hp = -loss;
            if(hp <= 0)
            {
                gameObject.SetActive(false); // Destroy를 안쓰는 이유 : 써도 되긴하지만 생성/파괴 명령이 메모리를 조금 더 많이 차지함
            }
        }
    }
}
