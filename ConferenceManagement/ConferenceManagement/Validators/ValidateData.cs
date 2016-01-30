namespace ConferenceManagement.Validators
{
    public class ValidateData
    {
        private readonly IValidator _validator;

        /// <summary>
        ///     Constructor for initialization
        /// </summary>
        /// <param name="validator">The validator(Of type RawTitle/RawTimeDuration).</param>
        public ValidateData(IValidator validator)
        {
            _validator = validator;
        }

        /// <summary>
        ///     Function which validates supplied string is Valid according or not Exit from Application
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <returns>Returns true if a string is valid otherwise, returns false</returns>
        public bool IsValid(string data)
        {
            return _validator.IsValid(data);
        }
    }
}