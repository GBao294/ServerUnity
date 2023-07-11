//using Riptide;
////using Riptide.Attributes;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ShootingServer : MonoBehaviour
//{
//    private float lastShootTime = 0f;
//    public float shootCooldown = 0.2f;
//    [SerializeField] private Player player;

//    [MessageHandler((ushort)ClientToServerId.shoot)]
//    private  void HandleShoot(Message message)
//    {
//        float currentTime = GetServerTime();

//        // Kiểm tra khoảng thời gian giữa các lần bắn
//        if (currentTime - lastShootTime >= shootCooldown)
//        {
//            // Bắn đạn
//            Shoot();

//            // Gửi phản hồi về cho client
//            // SendShootConfirmation(message.GetSenderClientId());

//            // Cập nhật thời điểm cuối cùng bắn
//            lastShootTime = currentTime;
//        }
//    }

//    private void Shoot()
//    {
//        // Thực hiện logic bắn đạn ở phía server
//    }

//    private void SendShootNotification(ushort shootingClientId)
//    {
//        // Tạo thông điệp phản hồi bắn
//        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.shootNotification);
//        message.AddUShort(player.Id);
//        // Gửi thông điệp phản hồi cho client
//        NetworkManager.Singleton.Server.SendToAll(message);
//    }

//    private float GetServerTime()
//    {
//        // Trả về thời gian hiện tại của server (có thể sử dụng các thư viện hỗ trợ hoặc cơ chế đồng bộ thời gian khác)
//        return Time.time;
//    }
//}