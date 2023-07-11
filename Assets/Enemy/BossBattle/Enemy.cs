using Riptide;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    

    public static Dictionary<ushort, Enemy> list = new Dictionary<ushort, Enemy>();

    //[SerializeField] private float TimeSpawnEnemy = 3.5f;

    [SerializeField] private bool BossSpawnCount = false;
    public ushort IdE { get; private set; }

    [SerializeField] public float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthSlider;
    public bool IsAlive => health > 0f;
    [SerializeField] private float respawnSeconds;

    public EnemyFollowPlayer Movement => movement;
    [SerializeField] public EnemyFollowPlayer movement;

    public static void spawnEnemy(ushort id)
    {
        Enemy newEnemy = Instantiate(GameLogic.Singleton.EnemyPrefab, new Vector2(Random.Range(-3f, 3), Random.Range(-4f, 4)), Quaternion.identity).GetComponent<Enemy>();
        list.Add(id, newEnemy);
        newEnemy.IdE = id;
        newEnemy.SendSpawned(id);
    }

    public static void SendSpawnedEnemy(ushort fromClientId)
    {
        
        //foreach (Enemy otherEnemy in list.Values)
        //    otherPlayer.SendSpawned(id);


        if (list.Count > 0)
        {
            for (ushort i = 0; i < list.Count; ++i)
            {
                SendSpawned(i, fromClientId);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    public void UpdateHealth(float mod)
    {

        {
            health += mod;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0f)
            {
                health = 0f;
                //healthSlider.value = health;
                movement.Enabled(false);
                //GetComponent<Enemy>().model.SetActive(false);

                //SendDied();
                //StartCoroutine(DelayedRespawn());
            }
            SendHealthChanged(mod);
        }

    }


    private void OnGUI()
    {
        float t = Time.deltaTime / 1f;
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);
    }
    private void SendHealthChanged(float mod)
    {
        Debug.Log("Cap nhat lai mau phia client " + mod + " " + IdE);
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.EnemyHealthChanged);
        message.AddUShort(IdE);
        message.AddFloat(mod);
        NetworkManager.Singleton.Server.SendToAll(message);
    }


    //public void SendRespawned()
    //{
    //    Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.EnemyRespawned);
    //    message.AddUShort(IdE);
    //    message.AddVector3(transform.position);
    //    NetworkManager.Singleton.Server.SendToAll(message);
    //}


    private static void SendSpawned(ushort id, ushort fromClientid)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(id, Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.EnemySpawned)), fromClientid);
    }

    private void SendSpawned(ushort id )
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(id, Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.EnemySpawned)));
    }

    private static Message AddSpawnData(ushort id, Message message)
    {
        message.AddUShort(id);
        message.AddVector2(list[id].transform.position);
        return message;
    }
  
    [MessageHandler((ushort)ClientToServerId.GetEnemy)]
    private static void SpawnE(ushort fromClientId, Message message)
    {
        Debug.Log("Nhan duoc thong bao spawn quai phia client");
        SendSpawnedEnemy(fromClientId);
    }

}
