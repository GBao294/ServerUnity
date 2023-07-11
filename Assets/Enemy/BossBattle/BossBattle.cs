//using Riptide;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine;

//public class BossBattle : MonoBehaviour
//{

//    public static Dictionary<ushort, GameObject> list = new Dictionary<ushort, GameObject>();
//    public ushort enemySpawnCount { get; private set; }

//    [SerializeField] private GameObject enemy;
//    [SerializeField] private float TimeSpawnEnemy = 3.5f;
    
//    [SerializeField] private bool BossSpawnCount = false;
//    private void Start()
//    {
//        StartCoroutine(spawnEnemy(TimeSpawnEnemy, enemy));
//        InvokeRepeating("UpdateNumber", 0.5f, 0.5f);
//    }
   
//    private IEnumerator spawnEnemy(float interval, GameObject enemyPrefab)
//    {
//        yield return new WaitForSeconds(interval);

//        if (enemySpawnCount >= 10)
//        {
//            yield break; // Stop spawning enemies if the count is reached
//        }

//        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6), 0), Quaternion.identity);
//        list.Add(enemySpawnCount, newEnemy);
//        enemySpawnCount++;
//        StartCoroutine(spawnEnemy(interval, enemy));


//    }
//    public static void SendSpawnedEnemy(ushort fromClientId)
//    {
//        if (list.Count > 0)
//        {
//            for (ushort i = 0; i < list.Count; ++i)
//            {
//                SendSpawned(i,fromClientId);
//            }
//        }
       
//    }
//    private static void SendSpawned(ushort id ,ushort fromClientid)
//    {
//        NetworkManager.Singleton.Server.Send(AddSpawnData(id,Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.EnemySpawned)), fromClientid);
//    }
//    private static Message AddSpawnData (ushort id, Message message)
//    {
//        message.AddUShort(id);
//        message.AddVector2(list[id].transform.position);
//        return message;
//    }
//    void UpdateNumber()
//    {
//        Debug.Log("222");
//        if (list.Count > 0)
//        {
//            for (ushort i = 0; i < list.Count; i++)
//            {
//                if (list[i] != null)
//                    Debug.Log("Enemy ID " + i + "tai vi tri " + list[i].transform.position);
//            }

//        }
//    }
   
//    [MessageHandler((ushort)ClientToServerId.GetEnemy)]
//    private static void SpawnE(ushort fromClientId , Message message)
//    {
//        Debug.Log("Nhan duoc thong bao spawn quai phia client");
//        SendSpawnedEnemy(fromClientId);
//    }
//}   





   

  







