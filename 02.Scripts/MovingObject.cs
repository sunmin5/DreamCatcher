using UnityEngine;
using System.Collections;

namespace Completed
{
	public abstract class MovingObject : MonoBehaviour // 추상 클래스 => 추상 메소드를 하나 이상 보유하고 있는 클래스, 상속자로 사용할 클래스
	{
        public LayerMask blockingLayer; // blockingLayer라고 지정된 레이어만 감지하겠다
        float moveTime = 30;

		Rigidbody2D rb2D;
        BoxCollider2D box2D;
        protected Animator anim;

		protected virtual void Start()
        {
			rb2D = GetComponent<Rigidbody2D>();
            box2D = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }
        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            Vector2 start = transform.position; // 시작점
            Vector2 end = start + new Vector2(xDir, yDir); //도착점

            //레이를 쏴도 본인 콜라이더에 걸려서 본인이 선택됨. 따라서 순간 콜라이더를 껐다가 레이를 쏘고 콜라이더 재활성화
            box2D.enabled = false;
            hit = Physics2D.Linecast(start, end, blockingLayer);
            box2D.enabled = true;

            if (hit.transform == null)
            {
                StartCoroutine(SmoothMove(end));
                return true;
            }
            return false;
        }

        protected IEnumerator SmoothMove(Vector2 end) // 한 칸 이동
        {
            float distance = (rb2D.position - end).sqrMagnitude; // 도착점까지 남은 거리의 크기(magnitude), sqrMagnitude(제곱 수) 
            //큰지 작은지 단순 비교만 할때는 정확한 수치는 중요하지 않으므로 sqrMagnitude가 메모리를 덜 먹는다(루트 계산을 안하므로). 하지만 정확한 거리가 필요할때는 magnitude로 확실하게 써줘야함

            while(distance > float.Epsilon) // float.Epsilon = 1.4 * 10^(-45) => 거의 0과 같은 아주 작은 수
            {
                Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, moveTime*Time.deltaTime); // MoveTowards => rb2D.position에서 end까지 moveTime*Time.deltaTime 만큼 이동해주는 함수 
                rb2D.MovePosition(newPos); // MovePosition : 해당 위치값으로 움직이는 모습이 연출됨
                distance = (rb2D.position - end).sqrMagnitude;

                yield return null; // 한 프레임 휴식
            }
        }

        // 플레이어는 벽을, 적은 플레이어를 공격하도록 해야함
        protected virtual void AttemptMove<T>(int xDir, int yDir) // <> 제네릭 사용 => 스크립트를 지정함으로써 같은 메소드로 여러가지를 구분지을 수 있음 (상속을 하기때문에 의미가 있음)
        {
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);

            if (hit.transform == null) return;

            T hitComponet = hit.transform.GetComponent<T>();

            if (!canMove && hitComponet != null)
            {
                OnCantMove(hitComponet);
            }
        }
        protected abstract void OnCantMove<T>(T hitComponet); // 추상 메소드 : 내용은 없고 이름만 존재함. 상속자가 내용을 채워감 (중괄호가 없고 세미콜론으로 마무리)
    }
}