using UnityEngine;
using System.Collections;

namespace Completed
{
    public class Enemy : MovingObject
    {
        public int playerDamage;

        Transform target;

        bool skip = false; // 진퇴양난 걸렸을 때 탈출할 기회

        protected override void Start()
        {
            base.Start();
            GameManager.instance.AddEnemyToList(this);
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected override void OnCantMove<T>(T hitComponet)
        {
            Player player = hitComponet as Player;
            player.LoseFood(playerDamage);
            anim.SetTrigger("enemyAttack");
        }

        protected override void AttemptMove<T>(int xDir, int yDir) // 2턴에 한번씩 움직임
        {
            if (skip)
            {
                skip = false;
                return; // skip이 true라면 움직이지 않음
            }
            base.AttemptMove<T>(xDir, yDir);
            skip = true;
        }

        public void MoveEnemy()
        {
            int xDir = 0, yDir = 0;

            // Mathf.Abs : 절대값
            if (Mathf.Abs(target.position.x - transform.position.x) <= float.Epsilon) // 같은 x축 선상에 있다면
            {
                yDir = (target.position.y > transform.position.y) ? 1 : -1; // 타겟의 y위치가 나의 y위치보다 크다면 y방향으로 +1만큼, 반대라면 -1만큼 이동
            }
            else // 같은 x축 선상에 있지 않다면
            {
                xDir = (target.position.x > transform.position.x) ? 1 : -1; // 타겟의 x위치가 나의 x위치보다 크다면 x방향으로 +1만큼, 반대라면 -1만큼 이동
            }

            AttemptMove<Player>(xDir, yDir);
        }
    }
}