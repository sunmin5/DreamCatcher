using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Completed
{
	public class GameManager : MonoBehaviour
	{
        #region ----------------------------- 싱글턴 -----------------------------

        public static GameManager instance; // 싱글턴
        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // 씬을 다시 불러오더라도 나 자신을 파괴시키지 않겠다
            }
            else
            {
                Destroy(gameObject); // 첫번째 씬 이후로는 파괴시킨다 (최초 실행 시 파괴시키지않고 남겨뒀으므로 이후로는 없애야 1개만 남음)
            }    
        }
        #endregion

        [HideInInspector] public bool playerTurn;
        public int playerFood = 100; // 시작 시 푸드스코어 // public이지만 인스펙터창에서는 숨기겠다
        [SerializeField] float nextLevelDelay = 2; // 레벨 넘어갈 때 딜레이 시간
        [SerializeField] float turnDelay = 0.2f; // 턴 당 딜레이

        GameObject levelImage; // DontDestroyOnLoad에서는 씬 전환 시 다 파괴시키므로 public 사용하면 안됨. 직접 채워줘야함
        Text levelText; // DontDestroyOnLoad에서는 씬 전환 시 다 파괴시키므로 public 사용하면 안됨. 직접 채워줘야함

        int level = 1;
        BoardManager boardManager;

        bool doingSetup;
        bool enemyMoving;

        List<Enemy> enemies = new List<Enemy>(); // enemies 라는 리스트 생성(빈 공간)

        void Start()
        {
            boardManager = GetComponent<BoardManager>();
            InitGame();
            SceneManager.sceneLoaded += OnScendLoaded; // sceneLoaded => 델리게이트 (메소드 변수화)
        }

        /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInit()
        {
            SceneManager.sceneLoaded += OnScendLoaded; // sceneLoaded => 델리게이트 (메소드 변수화)
        }*/    //SceneManager.sceneLoaded += OnScendLoaded;를 스타트에 쓰거나 스타트에서 쓰지않고 이렇게 쓰거나 동일한 결과

        private static void OnScendLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.level++;
            instance.InitGame();
        }

        void InitGame()
        {
            doingSetup = true;
            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelImage.SetActive(true);
            levelText.text = "Day " + level;
            Invoke("HideLevelImage", nextLevelDelay); // HideLevelImage 메소드를 nextLevelDelay 후에 실행
            enemies.Clear(); // 레벨이 올라갈때마다 즉 씬이 새로 불러질 때마다 enemy 리스트 클리어
            boardManager.Setup(level);
        }

        void HideLevelImage()
        {
            levelImage.SetActive(false);
            playerTurn = true;
            doingSetup = false;
        }

        public void EndGame()
        {
            levelText.text = "After " + level + " day, \n    you starved!";
            levelImage.SetActive(true);
            enabled = false;
        }

        public void AddEnemyToList(Enemy enemy)
        {
            enemies.Add(enemy); // 비어있는 enemies 리스트에 enemy 등록
        }

        void Update()
        {
            if (playerTurn || doingSetup || enemyMoving) return;
            StartCoroutine(MoveEnemies());
        }
        IEnumerator MoveEnemies()
        {
            enemyMoving = true;
            yield return new WaitForSeconds(turnDelay);

            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            foreach (var enemy in enemies)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(turnDelay);
            }
            playerTurn = true;
            enemyMoving = false;
        }
    }
}