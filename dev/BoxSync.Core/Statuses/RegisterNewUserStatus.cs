namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies execution status of 'register_new_user' web method
	/// </summary>
	public enum RegisterNewUserStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'successful_register' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// Represents 'e_register' status string
		/// </summary>
		Failed = 2,

		/// <summary>
		/// The login provided is not a valid email address.
		/// Represents 'email_invalid' status string.
		/// </summary>
		EmailInvalid = 3,

		/// <summary>
		/// An invalid API key was provided, or the API key is restricted from calling this function.
		/// Represents 'application_restricted' status string.
		/// </summary>
		ApplicationRestricted = 4,

		/// <summary>
		/// The login provided is already registered by another user.
		/// Represents 'email_already_registered' status string.
		/// </summary>
		EmailAlreadyRegistered = 5
	}
}
