using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        UpdateInfo,
        FileHeader,
        FileData,
        LaunchGame,
        StartGamePlay,
    }
    public enum ClientMessages
    {
        PlayerInfo,
        GameMessage,
        ExeInfo,
        GameLoaded,
    }
    public enum FileTypes
    {
        ModFile,
        ExeFile,
        WorldFile,
        Dll,
    }
}
