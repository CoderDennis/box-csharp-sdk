using System.Collections.Generic;

using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents response from 'OverwriteFile' method
	/// </summary>
	public sealed class OverwriteFileResponse : ResponseBase<OverwriteFileStatus>
	{
		private Dictionary<File, UploadFileError> _uploadedFileStatus = new Dictionary<File, UploadFileError>();

		/// <summary>
		/// Gets or sets the list of errors which happed during upload process for every single file
		/// </summary>
		public Dictionary<File, UploadFileError> UploadedFileStatus
		{
			get
			{
				return _uploadedFileStatus;
			}
			internal set
			{
				if (value == null)
				{
					_uploadedFileStatus.Clear();
				}
				else
				{
					_uploadedFileStatus = value;
				}
			}
		}
	}
}
