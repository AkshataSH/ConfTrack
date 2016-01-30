using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement
{
    
    public class ConferenceRoom
    {
        public string Title { get; private set; }

        public TalkSession MorningSession { get; set; }

        public Break LunchBreak { get; set; }

        public TalkSession EveningSession { get; set; }

       // public NetworkingEvent Networking { get; set; }
    }

    
}
