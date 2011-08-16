namespace BoxSync.Core.Statuses
{

	/// <summary>
	/// Specifies statuses of 'move' web method
	/// </summary>
	public enum MoveObjectStatus : byte
	{
		/// <summary>
		/// Used if status string doen't match to any of the enum members
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 's_move_node' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// Represents 'e_move_node' status string
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
		ApplicationRestricted = 4
	}
}
