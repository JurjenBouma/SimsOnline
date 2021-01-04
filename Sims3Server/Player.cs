﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TcpConnection;

namespace Sims3Server
{
    public class Player
    {
        public string Name;
        public bool IsReady;
        public bool IsGameLoaded;
        public TcpSocket clientSocket;

        public Player(TcpSocket tcpSocket)
        {
            Name = "";
            IsReady = false;
            IsGameLoaded = false;
            clientSocket = tcpSocket;
        }
    }
}
