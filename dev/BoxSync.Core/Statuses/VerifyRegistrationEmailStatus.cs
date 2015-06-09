namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies statuses of 'verify_registration_email' web method
	/// </summary>
	public enum VerifyRegistrationEmailStatus : byte
	{
		/// <summary>
		/// Used if status string doen't match to any of enum members
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'email_ok' status string
		/// </summary>
		EmailOK = 1,

		/// <summary>
		/// An error occurred during operation execution
		/// </summary>
		Failed = 2,

		/// <summary>
		/// An invalid API key was provided, or the API key is restricted from calling this function.
		/// Represents 'application_restricted' status string
		/// </summary>
		ApplicationRestricted = 4,

		/// <summary>
		/// The login provided is not a valid email address.
		/// Represents 'email_invalid' status string
		/// </summary>
		EmailInvalid = 5,

		/// <summary>
		/// The login provided is already registered by another user.
		/// Represents 'email_already_registered' status string
		/// </summary>
		EmailAlreadyRegistered = 6
	}
}
