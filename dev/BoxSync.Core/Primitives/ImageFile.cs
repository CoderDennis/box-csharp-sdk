namespace BoxSync.Core.Primitives
{

	/// <summary>
	/// Represents the image file entity. This class cannot be inherited
	/// </summary>
	public sealed class ImageFile : File
	{
		/// <summary>
		/// Initiates object
		/// </summary>
		public ImageFile()
		{	
		}

		/// <summary>
		/// Initiates object
		/// </summary>
		/// <param name="file">File object</param>
		public ImageFile(File file)
		{
			Created = file.Created;
			Description = file.Description;
			ID = file.ID;
			IsShared = file.IsShared;
			Name = file.Name;
			OwnerID = file.OwnerID;
			PermissionFlags = file.PermissionFlags;
			PublicName = file.PublicName;
			SHA1Hash = file.SHA1Hash;
			SharedLink = file.SharedLink;
			Size = file.Size;
			Updated = file.Updated;
			

			Tags.AddRange(file.Tags);
		}

		/// <summary>
		/// Gets or sets thumbnail URL
		/// </summary>
		public string ThumbnailUrl
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets small thumbnail URL
		/// </summary>
		public string SmallThumbnailUrl
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets large thumbnail URL
		/// </summary>
		public string LargeThumbnailUrl
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets larger thumbnail URL
		/// </summary>
		public string LargerThumbnailUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets preview thumbnail URL
		/// </summary>
		public string PreviewThumbnailUrl
		{
			get; set;
		}
	}
}
