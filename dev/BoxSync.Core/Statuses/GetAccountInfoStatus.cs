namespace BoxSync.Core.Statuses
{
	/// <summary>
	/// Specifies statuses of 'get_account_info' web method
	/// </summary>
	public enum GetAccountInfoStatus
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'get_account_info_ok' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// Failed to retrieve user account information
		/// </summary>
		Failed = 2,

		/// <summary>
		/// The user did not successfully authenticate.
		/// Represents 'not_logged_in' status string.
		/// </summary>
		NotLoggedIn = 3,

		/// <summary>
		/// An invalid API key was provided, or the API key is restricted from calling this function.
		/// Represents 'application_restricted' status string.
		/// </summary>
		ApplicationRestricted = 4,
	}
}
