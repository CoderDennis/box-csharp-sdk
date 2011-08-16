using System.Diagnostics;

using BoxSync.Core.ServiceReference;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Defines base properties of Box.NET folder
	/// </summary>
	[DebuggerDisplay("ID = {ID}, Name = {Name}, FolderType = {FolderTypeID}, IsShared = {IsShared}")]
	public class FolderBase
	{
		/// <summary>
		/// Initializes folder object
		/// </summary>
		public FolderBase()
		{
			
		}

		internal FolderBase(SOAPFolder folder)
		{
			ID = folder.folder_id;
			Name = folder.folder_name;
			FolderTypeID = folder.folder_type_id;
			ParentFolderID = folder.parent_folder_id;
			Password = folder.password;
			Path = folder.path;
			PublicName = folder.public_name;
			IsShared = folder.shared == 1;
			OwnerID = folder.user_id;
		}

		/// <summary>
		/// ID of the folder
		/// </summary>
		public long ID
		{
			get;
			set;
		}

		/// <summary>
		/// Name of the folder
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// The ID of the user who owns the folder
		/// </summary>
		public long? OwnerID
		{
			get;
			set;
		}

		/// <summary>
		/// Type of the folder. Could be null
		/// </summary>
		public long? FolderTypeID
		{
			get;
			set;
		}

		/// <summary>
		/// ID of the parent folder. Could be null
		/// </summary>
		public long? ParentFolderID
		{
			get; 
			set;
		}

		/// <summary>
		/// If the file is shared and password protected, this is the password associated with that file.
		/// Could be null
		/// </summary>
		public string Password
		{
			get; 
			set;
		}

		/// <summary>
		/// The path of the folder from the root. 
		/// Could be null
		/// </summary>
		public string Path
		{
			get; 
			set;
		}

		/// <summary>
		/// If the file is shared, this URL can be used to display a shared page.
		/// Could be null
		/// </summary>
		public string PublicName
		{
			get; 
			set;
		}

		/// <summary>
		/// Indicates if folder is shared
		/// </summary>
		public bool? IsShared
		{
			get; 
			set;
		}
	}
}
