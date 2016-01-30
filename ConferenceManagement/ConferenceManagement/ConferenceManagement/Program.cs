using System.IO;
using System.Linq;

namespace ConferenceManagement
{
    public class Program
    {
        private static void Main()
        {
            var meetings = File.ReadAllLines(Constants.InputFileName);
            var tracks = Helper.GetTrack(meetings.ToList());

            // sort _tracks into decreasing order by duration 
            tracks = tracks.OrderByDescending(t => t.TopicDuration).ToList();
            Helper.AllocateTopicsForSession(tracks);
        }
    }
}