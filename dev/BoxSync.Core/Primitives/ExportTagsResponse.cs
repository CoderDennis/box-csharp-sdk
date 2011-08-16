using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents response from 'ExportTags' method
	/// </summary>
	public sealed class ExportTagsResponse : ResponseBase<ExportTagsStatus>
	{
		/// <summary>
		/// List of tags associated with user's account
		/// </summary>
		public TagPrimitiveCollection TagsList
		{
			get;
			internal set;
		}
	}
}
