using System;
using System.Collections.Generic;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Specifies options for folder structure retrieval operation
	/// </summary>
	[Flags]
	public enum RetrieveFolderStructureOptions : byte
	{
		/// <summary>
		/// Indicates that retrieve process should use default
		/// options
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates that only folders must be included in the result tree, 
		/// no files
		/// </summary>
		NoFiles = 1,

		/// <summary>
		/// Indicates that XML folder structure tree should not be compressed
		/// </summary>
		NoZip = 2,

		/// <summary>
		/// Indicates that only one level of folder structure tree 
		/// should be retrieved, so you will get only files and 
		/// folders stored in folder which FolderID you have provided
		/// </summary>
		OneLevel = 4,

		/// <summary>
		/// Indicates that XML folder structure tree shouldn't contain
		/// all the details (thumbnails, shared status, tags, and other attributes are left out).
		/// Recomended to use in mobile applications
		/// </summary>
		Simple = 8
	}

	/// <summary>
	/// Provides helper methods for <see cref="RetrieveFolderStructureOptions"/> enumeration list
	/// </summary>
	public static class FolderStructureRetrieveModeExtensions
	{
		/// <summary>
		/// Checks if <paramref name="folderStructureOptions"/> contains <paramref name="options"/>
		/// </summary>
		/// <param name="folderStructureOptions">Set of <see cref="RetrieveFolderStructureOptions"/> flags</param>
		/// <param name="options">Single folder structure retrieve option</param>
		/// <returns>True if set of flags <paramref name="folderStructureOptions"/> contains <paramref name="options"/></returns>
		public static bool Contains(this RetrieveFolderStructureOptions folderStructureOptions, RetrieveFolderStructureOptions options)
		{
			return (folderStructureOptions & options) == options;
		}

		/// <summary>
		/// Converts <paramref name="folderStructureOptions"/> to string array
		/// </summary>
		/// <param name="folderStructureOptions">Folder structure retrieve options</param>
		/// <returns>String array representation of <paramref name="folderStructureOptions"/></returns>
		public static string[] ToStringArray(this RetrieveFolderStructureOptions folderStructureOptions)
		{
			List<string> result = new List<string>(3);
			
			if(folderStructureOptions.Contains(RetrieveFolderStructureOptions.NoFiles))
			{
				result.Add("nofiles");
			}

			if (folderStructureOptions.Contains(RetrieveFolderStructureOptions.NoZip))
			{
				result.Add("nozip");
			}

			if (folderStructureOptions.Contains(RetrieveFolderStructureOptions.OneLevel))
			{
				result.Add("onelevel");
			}

			if (folderStructureOptions.Contains(RetrieveFolderStructureOptions.Simple))
			{
				result.Add("simple");
			}

			return result.ToArray();
		}
	}
}
