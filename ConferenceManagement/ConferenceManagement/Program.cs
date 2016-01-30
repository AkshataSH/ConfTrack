using System;
using System.Configuration;
using System.IO;
using System.Linq;
using ConferenceManagement.Log;
using ConferenceManagement.Utilities;

namespace ConferenceManagement
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                //Read all lines from Text File input.
                var meetings = File.ReadAllLines(ConfigurationSettings.AppSettings["InputFileName"]);
                //Arrange given inputs into List of Topics
                var tracks = Helper.GetTrack(meetings.ToList());
                // sort _tracks into decreasing order by duration. 
                tracks = tracks.OrderByDescending(t => t.TopicDuration).ToList();
                //Allocate according to Large to small.
                Helper.AllocateTopicsForSession(tracks);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }
        }
    }
}