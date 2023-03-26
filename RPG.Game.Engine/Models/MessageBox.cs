using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
	public class MessageBox
	{
		public string Title { get; set; } //header
		public IList<string> Messages { get; set; } //list of messages
		public MessageBox(string title, IList<string> messages)
		{
			Title = title;
			Messages = messages;
		}

        public MessageBox(string title, string message)
        {
            Title = title;
            Messages = new List<string> { message };
        }
    }
}
