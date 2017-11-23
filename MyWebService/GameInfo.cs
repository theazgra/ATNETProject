using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebService
{
    class GameInfo
    {
        public int PlayerCount { get; private set; }

        public GameInfo(int playerCount)
        {
            PlayerCount = playerCount;
        }
    }
}
