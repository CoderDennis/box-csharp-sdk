namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies execution status of 'create_folder' web method
	/// </summary>
	public enum CreateFolderStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'create_ok' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// An error ocured during folder creation process
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
		/// The folder ID provided is not valid for the user's account.
		/// Represents 'e_no_parent_folder' status string.
		/// </summary>
		NoParentFolder = 5
	}
}
