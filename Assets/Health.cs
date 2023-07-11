using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Player player;
    public bool IsAlive => health > 0f;
    [SerializeField] private float respawnSeconds;
    //[SerializeField] private GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
        SendHealthChanged(health);
    }

    // Update is called once per frame
    public void UpdateHealth(float mod)
    {
        // Cập nhật giá trị health
        health += mod;

        // Kiểm tra nếu health vượt quá giá trị maxHealth, thì gán lại giá trị maxHealth
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0f)
        {
            health = 0f;

            // Lấy component Player
            Player player = GetComponent<Player>();

            // Vô hiệu hóa movement của Player
            player.movement.Enabled(false);

            // Thay đổi tag của Player thành "Die"
            player.model.tag = "Die";
            player.transform.position = Vector3.zero;
            // Gửi thông điệp đã chết
            SendDied();

            // Chờ một khoảng thời gian trước khi respawn
            StartCoroutine(DelayedRespawn());
        }

        // Gửi thông điệp thay đổi health
        SendHealthChanged(mod);
    }



    private IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(respawnSeconds);

        InstantRespawn();
    }

    public void InstantRespawn()
    {

        GetComponent<Player>().movement.Enabled(true);
        player.model.tag = "Player";
        health = 75;
        SendHealthChanged(75);
        GetComponent<Player>().SendRespawned();
    }

    private void SendDied()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.playerDied);
        message.AddUShort(GetComponent<Player>().Id);
        //message.AddVector3(transform.position);
        NetworkManager.Singleton.Server.SendToAll(message);
    }

    private void OnGUI()
    {
       float t = Time.deltaTime / 1f;
       healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);
    }
    private void SendHealthChanged(float mod)
    {
        Debug.Log("Cap nhat lai mau phia client " +mod+ " " + player.Id);
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.playerHealthChanged);
        message.AddUShort(player.Id);
        message.AddFloat(mod);
        NetworkManager.Singleton.Server.SendToAll(message);
    }
   
}
