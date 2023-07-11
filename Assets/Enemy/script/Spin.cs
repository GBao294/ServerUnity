using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private float currentRotation = 0f;
    [SerializeField] private Enemy enemy;
    void FixedUpdate()
    {
        // Tăng giá trị rotation hiện tại theo tốc độ quay
        currentRotation += rotationSpeed;

        // Kiểm tra nếu giá trị rotation vượt quá 180 độ, ta sẽ trừ đi 180 để quay trở lại từ 0
        if (currentRotation > 180f)
        {
            currentRotation -= 180f;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        Debug.Log(transform.rotation);
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ServerToClientId.EnemySpin);
        
        message.AddUShort(enemy.IdE);
        message.AddQuaternion(transform.rotation);
        // Gửi thông điệp phản hồi cho client
        NetworkManager.Singleton.Server.SendToAll(message);
    }
}
