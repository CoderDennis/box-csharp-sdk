namespace BoxSync.Core.Statuses
{
	/// <summary>
	/// Specifies statuses of 'get_comments' web method
	/// </summary>
	public enum GetCommentsStatus : byte
	{
		/// <summary>
		/// Unknown status string
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Represents 'listing_ok' status string
		/// </summary>
		Successful = 1,

		/// <summary>
		/// An error ocured during operation execution
		/// </summary>
		Failed = 2,
	}
}
