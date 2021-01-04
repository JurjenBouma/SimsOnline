using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsTcpClient
{
    public delegate void ModMessageEvent(string message);
    public delegate void UpDateReadyEvent();
    public partial class OnlineMod
    {
        public ModMessageEvent MessageEvent = null;
        public UpDateReadyEvent OnUpdateReady  = null;

        public TcpCommunication tcpManager;
        public BufferSocket bufferManager = new BufferSocket();
        public World world = new World();
        public ModFile modFile;
        public List<FileInterface> ReceivedFiles = new List<FileInterface>();
        public bool canLaunch = false;
        public bool isLoaded = false;

        public OnlineMod()
        {
            modFile = new ModFile();
        }
        public void Start(string nickName, string ip, EventHandler onPlayersChanged)
        {
            tcpManager = new TcpCommunication();
            tcpManager.OnPlayersChanged += onPlayersChanged;
            tcpManager.OnServerConnected += new ServerConnectedEvent(OnServerConnected);
            tcpManager.OnFileHeaderReceived += new FileHeaderEvent(OnFileHeaderReceived);
            tcpManager.OnFileDataReceived += new FileDataEvent(OnFileDataReceived);
            tcpManager.OnWorldInfoReceived += new WorldInfoEvent(OnWorldInfoReceived);
            tcpManager.OnModInfoReceived += new ModInfoEvent(OnModInfoReceived);
            tcpManager.OnLaunchGame += new LaunchGameEvent(OnLaunchGame);
            tcpManager.ConnectToServer(ip, 34019);
        }
        public void Stop()
        {
            if (tcpManager != null)
                tcpManager.Stop();
            unloadMod();
        }
        public void UpdatePlayerData()
        {
            tcpManager.SendPlayerInfo();
        }
        public void LoadMod()
        {
            if (!isLoaded)
            {
                world.SaveWorld();
                modFile.LoadMod();
                isLoaded = true;
                MessageEvent("Start the game manualy.");
                bufferManager.Connect();
            }
        }
        public void unloadMod()
        {
            if (isLoaded)
            {
                modFile.UnLoadMod();
                isLoaded = false;
            }
        }
    }
}
