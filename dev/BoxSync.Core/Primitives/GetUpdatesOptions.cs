using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Specifies options for updates retrieval operation
	/// </summary>
	[Flags]
	public enum GetUpdatesOptions : byte
	{
		/// <summary>
		/// Indicates that retrieve process should use default
		/// options
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates that the information ubout the updates should not be compressed
		/// while transfering from server to client application
		/// </summary>
		NoZip = 1
	}

	/// <summary>
	/// Provides helper methods for RetrieveFolderStructureOptions enumeration list
	/// </summary>
	public static class GetUpdatesOptionsExtensions
	{
		public static bool Contains(this GetUpdatesOptions getUpdatesOptions, GetUpdatesOptions option)
		{
			return (getUpdatesOptions & option) == option;
		}

		public static string[] ToStringArray(this GetUpdatesOptions options)
		{
			List<string> result = new List<string>(3);

			if ((options & GetUpdatesOptions.NoZip) == GetUpdatesOptions.NoZip)
			{
				result.Add("nozip");
			}

			return result.ToArray();
		}
	}
}
