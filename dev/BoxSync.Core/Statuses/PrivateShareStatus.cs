namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies statuses of 'private_share' web method
	/// </summary>
	public enum PrivateShareStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'private_share_ok' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// The target is not 'file'/'folder'. Or the new name contains invalid characters.
		/// Represents 'private_share_error' status string
		/// </summary>
		Failed = 2,

		/// <summary>
		/// The user is already no longer logged into Box for your application.
		/// Represents 'not_logged_in' status string
		/// </summary>
		NotLoggedIn = 3,

		/// <summary>
		/// An invalid API key was provided, or the API key is restricted from calling this function.
		/// Represents 'application_restricted' status string
		/// </summary>
		ApplicationRestricted = 4,

		/// <summary>
		/// The object ID is invalid.
		/// Represents 'wrong_node' status string
		/// </summary>
		WrongNode = 5
	}
}
