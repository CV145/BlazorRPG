using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Services
{
    //All messages sent from this singleton object
    public class MessageBoxBroker
    {
        private static readonly MessageBoxBroker _messageBroker = new MessageBoxBroker();

        private MessageBoxBroker() { }

        public event EventHandler<MessageBox>? OnMessageRaised;
        public static MessageBoxBroker Instance => _messageBroker;

        public void RaiseMessage (MessageBox message)
        {
            if (OnMessageRaised != null)
            {
                OnMessageRaised.Invoke(this, message);
            }    
        }
    }
}
