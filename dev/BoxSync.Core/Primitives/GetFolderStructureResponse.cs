using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents response from 'GetFolderStructure' method
	/// </summary>
	public sealed class GetFolderStructureResponse : ResponseBase<GetAccountTreeStatus>
	{
		/// <summary>
		/// Folder information
		/// </summary>
		public Folder Folder
		{
			get;
			internal set;
		}
	}
}
