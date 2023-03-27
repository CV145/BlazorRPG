using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.ViewModels
{
    public class KeyProcessingEventArgs : EventArgs
    {
        public string Key { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public float Location { get; set; }

        public bool Repeat { get; set; }

        public bool CtrlKey { get; set; }

        public bool ShiftKey { get; set; }

        public bool AltKey { get; set; }

        public bool MetaKey { get; set; }
    }
}
