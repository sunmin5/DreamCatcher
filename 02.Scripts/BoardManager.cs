using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // UnityEngine과 System 모두 Random 메소드가 있기 때문에 둘 다 선언된 경우 인식을 못하므로 UnityEngine의 Random을 쓸 것이기에 선언해주는 것

namespace Completed
{
	public class Count // 최소, 최대값 기억공간 한번에 묶어서 만들어주기 위해
	{
		public int min;
		public int max;

		public Count(int min , int max) // 생성자 => 초기값 
		{
			this.min = min;
			this.max = max;
		}
		/*public Count(int _min, int _max) // 생성자 => 초기값 
		{
			min = _min;
			max = _max;
		}*/ //위와 동일
	}

	public class BoardManager : MonoBehaviour
	{
		public int mapSize; // 정사각 사이즈로 만들 것이기에 가로 세로 따로 안주고 그냥 사이즈로 통일

		//int foodMin = 1, foodMax = 5; 기본적인 초기화 방법
		//int wallMin = 5, wallMax = 9; 기본적인 초기화 방법
		public Count foodCount = new Count(1, 5); // 클래스를 이용하여 초기화 방법
		public Count wallCount = new Count(5, 9); // 클래스를 이용하여 초기화 방법

		public GameObject exit; // 출구 게임 오브젝트
		public GameObject[] floorTiles; // 바닥 게임 오브젝트
		public GameObject[] wallTiles; // 벽 게임 오브젝트
		public GameObject[] foodTiles; // 음식 아이템 게임 오브젝트
		public GameObject[] enemyTiles; // 적 게임 오브젝트
		public GameObject[] outWallTiles; // 테두리 벽 게임 오브젝트

		Transform boardHolder;

		List<Vector3> gridPositions = new List<Vector3>(); // 리스트는 차지하고 있는 공간을 없앨 수 있음

		public void Setup(int level)
		{
			BoardSetup();
			InitializeList();

			LayoutObjectAtRandom(wallTiles, wallCount);
			LayoutObjectAtRandom(foodTiles, foodCount);

			int num = (int)Mathf.Log(level, 2); ; // Mathf.Log(x, y) => 밑이 y이고 진수가 x인 로그함수
			Count enemyCount = new Count(num, num);
			LayoutObjectAtRandom(enemyTiles, enemyCount);

			Instantiate(exit, new Vector3(mapSize - 1, mapSize - 1, 0), Quaternion.identity); // 출구를 해당 공간에 시작 시 배치
		}

		void LayoutObjectAtRandom(GameObject[] tiles, Count count) // 랜덤으로 오브젝트 생성(레이아웃으로) wall, food, enemy
        {
			int objectCount = Random.Range(count.min, count.max + 1);

            for (int i = 0; i < objectCount; i++)
            {
				int index = Random.Range(0, tiles.Length);
				GameObject tile = tiles[index];
				Vector3 pos = RandomPosition();
				Instantiate(tile, pos, Quaternion.identity);
            }
        }

		Vector3 RandomPosition()
        {
			int index = Random.Range(0, gridPositions.Count); // gridPosition은 리스트이므로 .Count
			Vector3 randomPos = gridPositions[index];
			gridPositions.RemoveAt(index); // (RemoveAt)인덱스가 차지하고 있는 해당 공간 삭제
			return randomPos;
        }

		void InitializeList() // 리스트 초기화
        {
			gridPositions.Clear();
			for (int x = 1; x < mapSize - 1; x++) // 테두리 벽이 있으므로 인필드는 (1,1) ~ (6, 6)까지
            {
                for (int y = 1; y < mapSize - 1; y++)
                {
					gridPositions.Add(new Vector3(x, y, 0)); // 해당 범위 내에서 리스트를 채운다.
                }
            }
        }

		void BoardSetup() // 바닥과 테두리 벽 게임 오브젝트 생성
        {
			boardHolder = new GameObject("boardHolder").transform; // boardHolder라는 빈 Empty 생성

			for (int x = -1; x < mapSize + 1; x++) // 등호가 없으므로 +1
            {
                for (int y = -1; y <= mapSize; y++) // 등호가 있으므로 그냥 mapSize
                {
					int index = Random.Range(0, floorTiles.Length);
					GameObject tile = floorTiles[index];

					if(x == -1 || y == -1 || x == 8 || y == 8) // 테두리 벽 생성 조건
                    {
						index = Random.Range(0, outWallTiles.Length);
						tile = outWallTiles[index];
                    }
					GameObject go = Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity); // 타일을 new Vector3(x, y, 0) 공간에 회전없이 생성하여 go라는 오브젝트에 담기
					go.transform.SetParent(boardHolder); // go 오브젝트는 boardHolder의 자식으로 생성
				}
            }
        }
	}
}