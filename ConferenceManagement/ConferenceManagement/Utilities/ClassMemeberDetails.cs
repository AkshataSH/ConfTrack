using System.Runtime.CompilerServices;

namespace ConferenceManagement.Utilities
{
    public class ClassMemeberDetails //Extension Method to get calling method name
    {
        #region CallerFunction

        /// <summary>
        ///     Function to return name of calling method
        /// </summary>
        /// <param name="memberName">The memberName.</param>
        /// <returns>return calling function name</returns>
        public static string GetCallerFunctionName([CallerMemberName] string memberName = "")
        {
            return memberName; //output will me name of calling method
        }

        #endregion
    }
}