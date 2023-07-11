using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public GameObject PlayerPrefab => playerPrefab;
    public GameObject EnemyPrefab => enemyPrefab;


    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private void Awake()
    {
        Singleton = this;
    }
    //private void Update()
    //{
    //    if(Enemy.list != null)
    //    {
    //        Debug.Log("ASAJCOASJCOASJCOASJCJSA");
    //    }
    //}
    //
    [SerializeField] ushort enemySpawnCount = 0;
    [SerializeField] float TimeSpawnEnemy = 3.5f;
    public void StartSpawnEnemy()
    {
        StartCoroutine(spawnEnemy(TimeSpawnEnemy, GameLogic.Singleton.EnemyPrefab));
    }
    
    ushort count = 0;
    private IEnumerator spawnEnemy(float interval, GameObject enemyPrefab)
    {
        yield return new WaitForSeconds(interval);
        if (count >= enemySpawnCount)
            {
                yield break; // Stop spawning enemies if the count is reached
            }
    Enemy.spawnEnemy(count);
    count++;

    StartCoroutine(spawnEnemy(interval, GameLogic.Singleton.EnemyPrefab));

   }















}
