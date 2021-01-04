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
        string simsWorldFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Electronic Arts\\De Sims 3\\Saves";
        public World world = new World();
        public ModFile modFile;
        public List<FileInterface> ReceivedFiles = new List<FileInterface>();
        public string AppStartupPath;
        public string AppName;

        public OnlineMod(string appStartupPath,string appName)
        {
            AppStartupPath = appStartupPath;
            AppName = appName;
            modFile = new ModFile(appStartupPath);
        }
        public void Start(string nickName, string ip, EventHandler onPlayersChanged)
        {
            tcpManager = new TcpCommunication(nickName);
            tcpManager.OnPlayersChanged += onPlayersChanged;
            tcpManager.OnServerConnected += new ServerConnectedEvent(OnServerConnected);
            tcpManager.OnFileHeaderReceived += new FileHeaderEvent(OnFileHeaderReceived);
            tcpManager.OnFileDataReceived += new FileDataEvent(OnFileDataReceived);
            tcpManager.OnWorldInfoReceived += new WorldInfoEvent(OnWorldInfoReceived);
            tcpManager.OnModInfoReceived += new ModInfoEvent(OnModInfoReceived);
            tcpManager.ConnectToServer(ip, 34019);
        }
        public void Stop()
        {
            if (tcpManager != null)
                tcpManager.Stop();
        }
        
    }
}
