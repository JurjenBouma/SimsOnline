using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsTcpClient
{
    public class Player
    {
        public string Name;
        public bool IsReady;
        public int ID;
        public Player(string name, bool isReady,int id)
        {
            Name = name;
            IsReady = isReady;
            ID = id;
        }
        public override string ToString()
        {
            if (IsReady)
                return Name + " - Ready";
            else
                return Name + " - Not Ready";
        }
    }
}
