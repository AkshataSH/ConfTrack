using System;
using System.Text.RegularExpressions;
using ConferenceManagement.Log;
using ConferenceManagement.Utilities;

namespace ConferenceManagement.Validators
{
    /// <summary>
    ///     Class which extends IValidator for Validation of Time
    /// </summary>
    public class RawTimeDuration : IValidator
    {
        /// <summary>
        ///     Function which validates supplied string is Valid according to Time or not
        /// </summary>
        /// <param name="candidate">The Candidate.</param>
        /// <returns>Returns true if a string is valid Time else returns false</returns>
        public bool IsValid(string candidate)
        {
            try
            {
                var regex = new Regex(Constants.TimePattern);
                return (regex.IsMatch(candidate.Trim()) && (candidate.Trim().ToUpper().EndsWith(Constants.TopicDurationSpecifier)) || candidate.Trim().ToUpper().EndsWith(Constants.DefaultTopicDuration));
            }
            catch (Exception ex)
            {
                Logger.LogMessage(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }
            return false;
        }
    }
}