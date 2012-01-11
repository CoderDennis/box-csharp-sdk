namespace BoxSync.Core.Statuses
{
	/// <summary>
	/// Specifies statuses of 'get_file_info' web method
	/// </summary>
	public enum GetFileInfoStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 's_get_file_info' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// File information retrieval failed by unknown reason
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

		/// <summary>
		/// The file ID is either invalid, or not accessible by that user.
		/// Represents 'e_access_denied' status string.
		/// </summary>
		AccessDenied = 6,
	}
}
