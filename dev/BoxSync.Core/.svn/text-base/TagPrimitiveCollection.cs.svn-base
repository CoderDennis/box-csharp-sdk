using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BoxSync.Core.Primitives;


namespace BoxSync.Core
{
	/// <summary>
	/// Represents a list of tags
	/// </summary>
	public sealed class TagPrimitiveCollection : IEnumerable<TagPrimitive>
	{
		private readonly List<TagPrimitive> _tagList = new List<TagPrimitive>();

		/// <summary>
		/// Gets list of tags
		/// </summary>
		/// <returns>List of tags</returns>
		public List<string> GetTagTextList()
		{
			return _tagList.Select(tag => tag.Text).ToList();
		}

		/// <summary>
		/// Get the text of tag by ID
		/// </summary>
		/// <param name="tagID">Tag ID</param>
		/// <returns>Tag text</returns>
		public string GetTagText(long tagID)
		{
			TagPrimitive resultTag = _tagList.First(tag => tag.ID.Equals(tagID));

			return resultTag.ID != 0 ? resultTag.Text : null;
		}

		/// <summary>
		/// Gets tag object by ID
		/// </summary>
		/// <param name="tagID">Tag ID</param>
		/// <returns>Tag object</returns>
		public TagPrimitive GetTag(long tagID)
		{
			TagPrimitive resultTag = _tagList.First(tag => tag.ID.Equals(tagID));

			return resultTag;
		}

		/// <summary>
		/// Adds tag to collection
		/// </summary>
		/// <param name="tag">Tag object</param>
		public void AddTag(TagPrimitive tag)
		{
			TagPrimitive resultTag = _tagList.FirstOrDefault(tagItem => tagItem.ID.Equals(tag.ID));

			if (resultTag.ID == 0)
			{
				_tagList.Add(tag);
			}
			else
			{
				resultTag.Text = tag.Text;
			}
		}

		/// <summary>
		/// Adds tag objects into collection
		/// </summary>
		/// <param name="tagList">List of the tags</param>
		public void AddTagRange(IEnumerable<TagPrimitive> tagList)
		{
			foreach (TagPrimitive tag in tagList)
			{
				AddTag(tag);
			}
		}

		/// <summary>
		/// Clears collection
		/// </summary>
		public void Clear()
		{
			_tagList.Clear();
		}

		/// <summary>
		/// Indicates if the collection is empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return _tagList.Count == 0;
			}
		}

		/// <summary>
		/// Gets the number of elements in the collection
		/// </summary>
		public int Count
		{
			get
			{
				return _tagList.Count;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection
		/// </summary>
		/// <returns>Collection enumerator</returns>
		public IEnumerator<TagPrimitive> GetEnumerator()
		{
			return _tagList.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection
		/// </summary>
		/// <returns>Collection enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets or sets the tag at the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public TagPrimitive this[int index]
		{
			get
			{
				return _tagList[index];
			}
			set
			{
				_tagList[index] = value;
			}
		}
	}
}