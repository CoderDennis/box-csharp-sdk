namespace BoxSync.Core.Statuses
{
	/// <summary>
	/// Specifies the statuses of 'get_updates' web method
	/// </summary>
	public enum GetUpdatesStatus : byte
	{
		/// <summary>
		/// Used if status string doen't match to any of the enum members
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 's_get_updates' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// The user is already no longer logged-in on Box.NET.
		/// Represents 'not_logged_in' status string
		/// </summary>
		NotLoggedIn = 3,

		/// <summary>
		/// An invalid API key was provided, or the API key is restricted from calling this function.
		/// Represents 'application_restricted' status string
		/// </summary>
		ApplicationRestricted = 4,

		/// <summary>
		/// Represents 'e_invalid_timestamp' status string
		/// </summary>
		InvalidTimestamp = 5
	}
}
