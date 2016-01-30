using System;
using System.Linq;
using System.Text.RegularExpressions;
using ConferenceManagement.Log;
using ConferenceManagement.Utilities;

namespace ConferenceManagement.Validators
{
    /// <summary>
    ///     Class which extends IValidator for Validation of Topic
    /// </summary>
    public class RawTitle : IValidator
    {
        /// <summary>
        ///     Function to validate topic title.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>returns true if given string is valid. Otherwise, false</returns>
        public bool IsValid(string candidate)
        {
            try
            {
                var candidateTrim = candidate.Trim();
                var isValidWithLightning = IsValidWithLightning(candidateTrim);


                if (isValidWithLightning.HasValue) return isValidWithLightning.Value;

                if (!HasOnlyOneNumberSet(candidateTrim)) return false;

                return IsValidPosition(candidateTrim);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }
            return false;
        }

        /// <summary>
        ///     IF topic has lightning, then there should exist no other digits
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>null if inconclusive</returns>
        private bool? IsValidWithLightning(string candidate)
        {
            if (candidate.ToUpper().EndsWith(Constants.DefaultTopicDuration))
            {
                return !candidate.Any(char.IsDigit);
            }

            return null;
        }

        private bool HasOnlyOneNumberSet(string candidate)
        {
            var ints = Regex.Matches(candidate, @"\d").Cast<Match>().Select(m => m.Index).ToArray();
            return ints.Length - 1 == ints.Last() - ints.First();
        }

        /// <summary>
        ///     Function to check whether TopicTitle has numbers(min) only at the end or not.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        ///     returns true if is valid position the specified topic; otherwise, false.
        /// </returns>
        private bool IsValidPosition(string candidate)
        {
            var regex = new Regex(Constants.TimePattern);
            return regex.IsMatch(candidate);
        }
    }
}