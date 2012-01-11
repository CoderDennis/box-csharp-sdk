namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies statuses of 'delete' web method
	/// </summary>
	public enum DeleteObjectStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 's_delete_node' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// Target type is not valid or file/folder ID is not valid for the user's account.
		/// Represents 'e_delete_node' status string
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
