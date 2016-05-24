using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Managers;

namespace GoblinKart.Init
{
    public class InitHost
    {
        public InitHost()
        {
            NetworkManager.Instance.InitNetworkServer();
            Debug.WriteLine("You have created a network server!");
        }
    }
}
