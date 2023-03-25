using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class QuestStatus
    {
        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
        }

        public Quest PlayerQuest { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
