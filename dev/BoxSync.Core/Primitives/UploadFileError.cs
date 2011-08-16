namespace BoxSync.Core.Primitives
{

	/// <summary>
	/// Provides information about type of error which happend during file upload process
	/// </summary>
	public enum UploadFileError : byte
	{
		/// <summary>
		/// Unknown type of error
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// There was not enough free space to store object
		/// </summary>
		NotEnoughFreeSpace = 1,

		/// <summary>
		/// File size was larger than allowed
		/// </summary>
		FileSizeLimitExceeded = 2,

		/// <summary>
		/// Access to account is denied
		/// </summary>
		AccessDenied = 3,

		/// <summary>
		/// No errors
		/// </summary>
		None = 4
	}
}
