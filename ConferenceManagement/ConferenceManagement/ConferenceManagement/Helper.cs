using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceManagement
{
    public static class Helper
    {
        public static void AllocateTopicsForSession(List<Topics> topics)
        {
            var morningSessionRoom1 = new List<Topics>();
            var eveningSessionRoom1 = new List<Topics>();
            var morningSessionRoom2 = new List<Topics>();
            var eveningSessionRoom2 = new List<Topics>();
            int totalDurationRoom1Am = 0, totalDurationRoom1Pm = 0, totalDurationRoom2Am = 0, totalDurationRoom2Pm = 0;
            var next = 0;

            for (var i = 0; i < topics.Count; i++)
            {
                var duration = topics[i].TopicDuration;

                if (next == 0 && totalDurationRoom1Am + duration <= 180)
                {
                    morningSessionRoom1.Add(topics[i]);
                    totalDurationRoom1Am += duration;
                    next++;
                }
                else if (next == 1 && totalDurationRoom2Am + duration <= 180)
                {
                    morningSessionRoom2.Add(topics[i]);
                    totalDurationRoom2Am += duration;
                    next++;
                }
                else if (next == 2 && totalDurationRoom1Pm + duration <= 240)
                {
                    eveningSessionRoom1.Add(topics[i]);
                    totalDurationRoom1Pm += duration;
                    next++;
                }
                else if (next == 3 && totalDurationRoom2Pm + duration <= 240)
                {
                    eveningSessionRoom2.Add(topics[i]);
                    totalDurationRoom2Pm += duration;
                    next = 0;
                }
                else
                {
                    i--;
                    next++;
                    if (next == 4) next = 0;
                }
            }
            // work out time of networking event
            var mins = Math.Max(totalDurationRoom1Pm, totalDurationRoom2Pm);
            if (mins < 180) mins = 180; // can't be before 4.00 pm
            var networkingEventTime = DateTime.Today.AddHours(13).AddMinutes(mins);

            // print results
            Console.Clear();
            Console.WriteLine(Constants.EventTrackOne);
            GetEvents(morningSessionRoom1, eveningSessionRoom1, networkingEventTime);

            Console.WriteLine("\n" + Constants.EventTrackTwo);
            GetEvents(morningSessionRoom2, eveningSessionRoom2, networkingEventTime);

            Console.ReadKey();
        }

        public static void GetEvents(List<Topics> morningSessionRoom1, List<Topics> eveningSessionRoom1,
            DateTime eventTime)
        {
            var dayStartTime = DateTime.Today.AddHours(9);

            foreach (var t in morningSessionRoom1)
            {
                Console.WriteLine("{0} {1}", dayStartTime.ToString(Constants.EventBookingDisplayFormat), t.TopicTitle);
                dayStartTime = dayStartTime.AddMinutes(t.TopicDuration);
            }

            Console.WriteLine(Constants.LunchScheduleTime);
            dayStartTime = DateTime.Today.AddHours(13);

            foreach (var t in eveningSessionRoom1)
            {
                Console.WriteLine("{0} {1}", dayStartTime.ToString(Constants.EventBookingDisplayFormat), t.TopicTitle);
                dayStartTime = dayStartTime.AddMinutes(t.TopicDuration);
            }

            Console.WriteLine("{0} {1}", eventTime.ToString(Constants.EventBookingDisplayFormat),
                Constants.NeteworkingEvent);
        }

        public static List<Topics> GetTrack(List<string> meetings)
        {
            return (from meeting in meetings
                let items = meeting.Trim().Split(' ')
                let len2 = items.Length - 1
                let last = items[len2].ToLower()
                select new Topics
                {
                    TopicTitle = meeting,
                    TopicDuration =
                        Constants.DefaultTopicDuration.Equals(last) ? 5 : int.Parse(last.Replace("min", string.Empty))
                }).ToList();

            //var topics = new List<Topics>();
            //foreach (string meeting in meetings)
            //{
            //    var items = meeting.Trim().Split(' ');
            //    var len2 = items.Length - 1;
            //    var last = items[len2].ToLower();
            //    topics.Add(new Topics
            //    {
            //        TopicTitle = meeting,
            //        TopicDuration =
            //            Constants.DefaultTopicDuration.Equals(last) ? 5 : int.Parse(last.Replace("min", string.Empty))
            //    });
            //}
            //return topics;
        }
    }
}