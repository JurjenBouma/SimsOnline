using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimsTcpClient
{
    public partial class OnlineMod
    {
        private void OnGameLoaded()
        {
            tcpManager.SendGameLoaded();
        }
        private void OnGameMessageSims(byte[] messages)
        {
            tcpManager.SendGameMessages(messages);
        }
    }
}
