using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Completed
{
	public class Player : MovingObject
	{
        public Text foodText;

        int food;
        int wallDamage = 1; // 벽을 때릴 때 데미지

        int foodScore = 10; // 푸드를 먹었을 때 스코어
        int sodaScore = 20; // 소다를 먹었을 때 스코어
        float nextLevelDelay = 1; // 레벨 넘어갈 때 딜레이시간

        Vector2 touchOrigin = -Vector2.one;

        protected override void Start()
        {
            base.Start();
            food = GameManager.instance.playerFood; // 시작 푸드 스코어로 시작
            foodText.text = "Food : " + food;
        }

        void OnDisable()
        {
            GameManager.instance.playerFood = food; // 현 레벨에서 증감된 푸드스코어 저장
        }

        void Update()
        {
            if (!GameManager.instance.playerTurn) return; // 내 턴이 아니라면 리턴
            
            int h = 0, v = 0;

#if UNITY_STANDALONE || UNITY_EDITOR
            h = (int)Input.GetAxisRaw("Horizontal"); // GetAxis(-1~1), GetAxisRaw(-1, 0, 1)
            v = (int)Input.GetAxisRaw("Vertical");

            if (h != 0)
            {
                v = 0;
            } // 동시에 두 키를 입력받지 않겠다는 뜻 (좌, 우 무엇인가 하나라도 입력이 들어갔다면 상하는 입력이 들어오더라도 무시하겠다)
#else
            if(Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0]; // 첫 터치만 인식하겠다.
                if(myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                else if (myTouch.phase == TouchPhase.Ended)
                {
                    Vector2 touchEnd = myTouch.position;
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y)) //y의 이동량보다 x의 이동량이 크다면
                    {
                        h = x > 0 ? 1 : -1; // x가 양수이면 1, 음수이면 -1
                    }
                    else v = y > 0 ? 1 : -1; // y의 이동량이 더 크다면 y가 양수이면 1, 음수이면 -1

                }
            }
#endif

            if (h != 0 || v != 0)
            {
                AttemptMove<Wall>(h, v);
            } // 둘 중 하나라도 눌렸다면 무브 실행
        }

        protected override void AttemptMove<T>(int xDir, int yDir) // 상속받음 + 추가로 플레이어가 해야하는 내용 추가
        {
            GameManager.instance.playerTurn = false;
            food--; // 움직일때마다 푸드스코어 차감
            foodText.text =  "Food : " + food;
            CheckIfGameOver();
            base.AttemptMove<T>(xDir, yDir);
        }

        void CheckIfGameOver()
        {
            if(food <= 0)
            {
                GameManager.instance.EndGame();
            }
        }

        public void LoseFood(int loss)
        {
            food -= loss;
            foodText.text = "-" + loss + " Food : " + food;
            CheckIfGameOver();
        }

        protected override void OnCantMove<T>(T hitComponet)
        {
            Wall wall = hitComponet as Wall; // T를 Wall로 형변환
            wall.DamageWall(wallDamage);
            anim.SetTrigger("playerChop");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Exit":
                    Invoke("NextLevel", nextLevelDelay); // nextLevelDelay 후에 "NextLevel" 함수 호출
                    this.enabled = false; // 레벨 넘어가는 딜레이 시간동안 움직임 방지
                    break;
                case "Food":
                    food += foodScore;
                    foodText.text = "+" + foodScore + " Food : " + food;
                    collision.gameObject.SetActive(false); // 푸드를 먹으면 화면 상에서 사라지도록 (어차피 다음 레벨로 넘어가면 다 파괴시키므로 잠시 화면에서만 안보이게)
                    break;
                case "Soda":
                    food += sodaScore;
                    foodText.text = "+" + sodaScore + " Food : " + food;
                    collision.gameObject.SetActive(false); // 소다를 먹으면 화면 상에서 사라지도록 (어차피 다음 레벨로 넘어가면 다 파괴시키므로 잠시 화면에서만 안보이게)
                    break;
            }
        }

        void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}