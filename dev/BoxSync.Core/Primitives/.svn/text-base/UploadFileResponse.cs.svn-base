using System.Collections.Generic;

using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents the response which returns upload file method
	/// </summary>
	public sealed class UploadFileResponse : ResponseBase<UploadFileStatus>
	{
		/// <summary>
		/// Gets or sets the ID of the folder to which file(s) was (were) uploaded
		/// </summary>
		public long FolderID
		{
			get;
			internal set;
		}

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
