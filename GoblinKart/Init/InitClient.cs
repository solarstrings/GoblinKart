using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Managers;

namespace GoblinKart.Init
{
    public class InitClient
    {
        public InitClient()
        {
            if (NetworkManager.Instance.InitClientConnection())
                Debug.WriteLine("Client connected to the host!");
            else
                Debug.WriteLine("Client connection to the host failed...");

        }
      


        // Where do we assign the network components to needed entities?
    }
}
