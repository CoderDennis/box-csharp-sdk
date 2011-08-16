namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies execution status of 'rename' web method
	/// </summary>
	public enum RenameObjectStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 's_rename_node' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// The target is not 'file'/'folder'. Or the new name contains invalid characters. Or target ID is invalid object ID in the user's account.
		/// Represents 'e_rename_node' status string
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
		ApplicationRestricted = 4
	}
}
