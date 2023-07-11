using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    //[SerializeField] public GameObject model;
    public ushort Id { get; private set; }
    public string Username { get; private set; }

    public PlayerControllerOnl Movement => movement;
    [SerializeField] public PlayerControllerOnl movement;

    [SerializeField] public GameObject model;
    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

       
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0, 0), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        list.Add(id, player);
    }

    #region Messages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector2(transform.position);
        return message;
    }
    public void SendRespawned()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.playerRespawned);
        message.AddUShort(Id);
        message.AddVector3(Vector3.zero);
        NetworkManager.Singleton.Server.SendToAll(message);
    }


    private void Move(Vector2 newPosition)
    {
        transform.position = newPosition;

    }
    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {

        if (list.TryGetValue(fromClientId, out Player player))
            player.Movement.SetInput(message.GetBools(5), message.GetQuaternion()); // gui di chuyen va huong nhin cua chuot client
    }

    [MessageHandler((ushort)ClientToServerId.shoot)]
    private static void Shoot(ushort fromClientId, Message message)
    {

        if (list.TryGetValue(fromClientId, out Player player))
            player.Movement.shoot(); // ban dan
    }




    [MessageHandler((ushort)ClientToServerId.chat)]
    private static void Chat(ushort fromClientId, Message message)
    {

        Message message1 = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.message);
        string get = list[fromClientId].Username + ": " + message.GetString();
        Debug.Log(get);
        message1.AddString(get);
        if (list.TryGetValue(fromClientId, out Player player))
            NetworkManager.Singleton.Server.SendToAll(message1);
    }

    #endregion

}
