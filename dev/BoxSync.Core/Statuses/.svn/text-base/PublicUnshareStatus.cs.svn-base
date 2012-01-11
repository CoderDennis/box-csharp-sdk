namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies execution status of 'public_unshare' web method
	/// </summary>
	public enum PublicUnshareStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'unshare_ok' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// The target is not 'file'/'folder'. Or the new name contains invalid characters.
		/// Represents 'unshare_error' status string
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
		/// The file or folder ID is invalid.
		/// Represents 'wrong_node' status string.
		/// </summary>
		WrongNode = 5
	}
}
