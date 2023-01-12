using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CChat;

namespace CChatClientGUI
{
    internal static class Globals
    {
        private static CChatClient _client = null;

        public static CChatClient Client
        {
            get 
            {
                if (_client != null) return _client;
                else
                {
                    _client = new CChatClient();
                    return _client;
                }
            }
        }

    }
}
