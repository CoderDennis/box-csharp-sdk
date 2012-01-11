using System;
using System.Collections.Generic;
using System.Diagnostics;

using BoxSync.Core.ServiceReference;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents the Box.NET folder entity
	/// </summary>
	[DebuggerDisplay("ID = {ID}, Name = {Name}, Folders = {Folders.Count}, Files = {Files.Count}, Tags = {Tags.Count}, IsShared = {IsShared}")]
	public sealed class Folder : FolderBase
	{
		/// <summary>
		/// Initializes folder object
		/// </summary>
		public Folder()
		{
		}

		internal Folder(SOAPFolder folder):base(folder)
		{
		}


		private List<Folder> _folders = new List<Folder>();
		
		/// <summary>
		/// Gets or sets list subfolders
		/// </summary>
		public List<Folder> Folders
		{
			get
			{
				return _folders;
			}
			set
			{
				_folders = value;
			}
		}


		private List<File> _files = new List<File>();

		/// <summary>
		/// Gets or sets list of child files
		/// </summary>
		public List<File> Files
		{
			get
			{
				return _files;
			}
			set
			{
				_files = value;
			}
		}


		private List<TagPrimitive> _tags = new List<TagPrimitive>();

		/// <summary>
		/// List of tags associated with folder
		/// </summary>
		public List<TagPrimitive> Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				_tags = value;
			}
		}


		/// <summary>
		/// Description of the folder
		/// </summary>
		public string Description
		{
			get; 
			set;
		}

		/// <summary>
		/// Link to shared folder
		/// </summary>
		public string SharedLink
		{
			get; 
			set;
		}

		/// <summary>
		/// Folder permissions
		/// </summary>
		public UserPermissionFlags? PermissionFlags
		{
			get; 
			set;
		}

		/// <summary>
		/// Role
		/// </summary>
		public string Role
		{
			get; 
			set;
		}

		/// <summary>
		/// Size of the folder
		/// </summary>
		public long? Size
		{
			get; 
			set;
		}

		/// <summary>
		/// File count 
		/// </summary>
		public int FileCount
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the date when folder was created
		/// </summary>
		public DateTime Created
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the date when folder was updated last time
		/// </summary>
		public DateTime Updated
		{
			get; 
			set;
		}
	}
}
