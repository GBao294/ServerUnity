using Riptide;
using Riptide.Utils;
using UnityEngine;

public enum ServerToClientId : ushort
{
    sync = 1,
    activeScene,
    playerSpawned,
    EnemySpawned,
    playerMovement,
    EnemyMovement,
    EnemySpin,
    message,
    shootNotification,
    playerHealthChanged,
    EnemyHealthChanged,
    playerDied,
    EnemyDied,
    playerRespawned,
    EnemyRespawned,


}

public enum ClientToServerId : ushort
{
    name = 1,
    GetEnemy,
    input,
    shoot,
    chat,

}



public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }
   

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    public ushort CurrentTick { get; private set; } = 0;
    private void FixedUpdate()  
    {
        Server.Update();
        if (CurrentTick % 200 == 0)
                SendSync();
        CurrentTick++;
        
        if(CurrentTick == 60000)
            CurrentTick = 0;
    }
    private void SendSync()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ServerToClientId.sync);
        message.AddUShort(CurrentTick);
        Server.SendToAll(message);
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    
    private void PlayerLeft(object sender, ServerDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Client.Id, out Player player))
            Destroy(player.gameObject);
    }
   
}
