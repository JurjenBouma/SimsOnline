using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMessageCodes
{
    public enum ServerMessages
    {
        GameMessage,
        Ping,
        ServerConnected,
        PlayerData,
        WorldInfo,
        ModInfo,
        FileHeader,
        FileData,
        LaunchGame,
    }
    public enum ClientMessages
    {
        PlayerInfo,
        GameMessage,
        ExeInfo,
    }
    public enum FileTypes
    {
        ModFile,
        ExeFile,
        WorldFile,
    }
}
