using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxSync.Core.Primitives
{
	public class UpdateItem
	{
		public long ID
		{
			get; 
			set;
		}

		public long UserID
		{
			get; 
			set;
		}

		public string UserName
		{
			get; 
			set;
		}

		public string UserEmail
		{
			get; 
			set;
		}

		public DateTime Updated
		{
			get; 
			set;
		}

		public UpdateType UpdateType
		{
			get; 
			set;
		}

		public int FolderID
		{
			get; 
			set;
		}

		public string FolderName
		{
			get; 
			set;
		}

		public bool IsShared
		{
			get; 
			set;
		}

		public string SharedName
		{
			get; 
			set;
		}

		public long OwnerID
		{
			get; 
			set;
		}

		public string FolderPath
		{
			get; 
			set;
		}

		public bool IsCollaborationAccessAllowed
		{
			get; 
			set;
		}

		private List<UpdatedFile> _files = new List<UpdatedFile>();
		public List<UpdatedFile> Files
		{
			get
			{
				return _files;
			}
		}
	}
}
