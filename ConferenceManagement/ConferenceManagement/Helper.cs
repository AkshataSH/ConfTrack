using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ConferenceManagement.Log;
using ConferenceManagement.Utilities;
using ConferenceManagement.Validators;

namespace ConferenceManagement
{
    public static class Helper
    {
        #region ListOfTopic

        /// <summary>
        ///     Converts given input to List of Tracks
        /// </summary>
        /// <param name="topics">The List of Topics.</param>
        /// <returns>
        ///     returns list of Topic
        /// </returns>
        public static List<Topics> GetTrack(List<string> topics)
        {
            return (from topic in topics
                where IsValidData(topic)
                // Validate given Input
                let items = topic.Trim().Split(' ')
                //Split the string
                let len2 = items.Length - 1
                let last = items[len2].ToLower()
                select new Topics
                {
                    TopicTitle = topic, //Title
                    TopicDuration =
                        Constants.DefaultTopicDuration.Equals(last.ToUpper())
                            ? 5
                            : int.Parse(last.ToUpper().Replace(Constants.TopicDurationSpecifier, string.Empty)) //Duration
                }).ToList();
        }

        #endregion

        #region AllocateTopic

        /// <summary>
        ///     Allocates given topics across session.
        ///     Allocates largest duration topic at the beginning of the session and continue allocating session with smaller
        ///     duration until
        ///     there is a space in session.
        /// </summary>
        /// <param name="topics">The List of Topics.</param>
        public static void AllocateTopicsForSession(List<Topics> topics)
        {
            var morningSessionRoom1 = new List<Topics>();
            var eveningSessionRoom1 = new List<Topics>();
            var morningSessionRoom2 = new List<Topics>();
            var eveningSessionRoom2 = new List<Topics>();
            int totalDurationRoom1Am = 0, totalDurationRoom1Pm = 0, totalDurationRoom2Am = 0, totalDurationRoom2Pm = 0;
            var nextSession = 0;
            try
            {
                for (var i = 0; i < topics.Count; i++)
                {
                    var duration = topics[i].TopicDuration;

                    if (nextSession == 0 && totalDurationRoom1Am + duration <= 180)
                    {
                        morningSessionRoom1.Add(topics[i]);
                        totalDurationRoom1Am += duration;
                        nextSession++;
                    }
                    else if (nextSession == 1 && totalDurationRoom2Am + duration <= 180)
                    {
                        morningSessionRoom2.Add(topics[i]);
                        totalDurationRoom2Am += duration;
                        nextSession++;
                    }
                    else if (nextSession == 2 && totalDurationRoom1Pm + duration <= 240)
                    {
                        eveningSessionRoom1.Add(topics[i]);
                        totalDurationRoom1Pm += duration;
                        nextSession++;
                    }
                    else if (nextSession == 3 && totalDurationRoom2Pm + duration <= 240)
                    {
                        eveningSessionRoom2.Add(topics[i]);
                        totalDurationRoom2Pm += duration;
                        nextSession = 0;
                    }
                    else
                    {
                        i--;
                        nextSession++;
                        if (nextSession == 4) nextSession = 0;
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
            catch (Exception ex)
            {
                Logger.LogMessage(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }
        }

        /// <summary>
        ///     Function to print records
        /// </summary>
        /// <param name="morningSessionRoom1">The Session.</param>
        /// <param name="eveningSessionRoom1">The Session.</param>
        /// <param name="eventTime">The Time.</param>
        public static void GetEvents(List<Topics> morningSessionRoom1, List<Topics> eveningSessionRoom1,
            DateTime eventTime)
        {
            var dayStartTime = DateTime.Today.AddHours(9); // to get 9 AM

            foreach (var room in morningSessionRoom1)
            {
                Console.WriteLine("{0} {1}", dayStartTime.ToString(Constants.EventBookingDisplayFormat), room.TopicTitle);
                dayStartTime = dayStartTime.AddMinutes(room.TopicDuration);
            }

            Console.WriteLine(Constants.LunchScheduleTime);
            dayStartTime = DateTime.Today.AddHours(13); //to get 1 PM

            foreach (var room in eveningSessionRoom1)
            {
                Console.WriteLine("{0} {1}", dayStartTime.ToString(Constants.EventBookingDisplayFormat), room.TopicTitle);
                dayStartTime = dayStartTime.AddMinutes(room.TopicDuration);
            }

            Console.WriteLine("{0} {1}", eventTime.ToString(Constants.EventBookingDisplayFormat),
                Constants.NeteworkingEvent);
        }

        #endregion

        #region Validation

        /// <summary>
        ///     Determines whether [is valid data] [the specified raw data].
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <returns>
        ///     <c>true</c> if [is valid data] [the specified raw data]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidData(string rawData)
        {
            try
            {
                var validateTime = new ValidateData(new RawTimeDuration());
                var validateTitle = new ValidateData(new RawTitle());

                if (validateTime.IsValid(rawData) && validateTitle.IsValid(rawData))
                    return true;
                var strMessage = "Invalid Input provided - Topic " + rawData;
                Logger.LogMessage(strMessage);
                ShowConfirmMessage(strMessage);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }

            return false;
        }

        /// <summary>
        ///     Displays Message Box for alert.
        /// </summary>
        /// <param name="strMessage">The Message.</param>
        /// <returns>
        ///     On click of OK, Conference Track is filled except invalid topics. On click of No, Exit from application.
        /// </returns>
        private static void ShowConfirmMessage(string strMessage)
        {
            if (
                MessageBox.Show(strMessage + " Do you want to proceed?",
                    "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return;
            Environment.Exit(0);
        }

        #endregion
    }
}