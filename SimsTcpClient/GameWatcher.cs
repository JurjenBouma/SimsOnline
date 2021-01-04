using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace SimsTcpClient
{
    public static class GameWatcher
    {
        public delegate void OnGamStateChangesHandler(bool isGameRunning);
        public static OnGamStateChangesHandler OnGameStateChanged = null;
        public static Thread GameWatchThread = new Thread(WatchGame);

        private static void WatchGame()
        {
            WatchForStart();
        }

        private static void WatchForStart()
        {
            while (!IsProcessRunning("TS3W"))
            {

            }
            OnGameStateChanged(true);
            WatchForClose();
        }
        private static void WatchForClose()
        {
            while (IsProcessRunning("TS3W"))
            {

            }
            OnGameStateChanged(false);
            WatchForStart();
        }

        private static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == processName)
                    return true;
            }
            return false;
        }
    }
}
