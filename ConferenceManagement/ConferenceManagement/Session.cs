using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement
{
    public abstract class Session
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string TopicTitle { get; set; }
    }


    public class TalkSession : Slot
    {
        public List<Topics> Topics { get; set; }

        public int _duration;
        public int Duration
        {
            get { return _duration; }
            private set { _duration = (int)EndTime.Subtract(StartTime).TotalMinutes; }
        }

        public TimeSpan TimeRemaining { get; set; }

        public string Title { get; set; }
    }

    public class NetworkingEvent : Slot
    {
        public TimeSpan StartTimeFrom { get; set; }

        public TimeSpan StartTimeTo { get; set; }
    }

    public class Break : Slot
    {

    }

}
