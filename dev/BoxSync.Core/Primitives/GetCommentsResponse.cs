using System.Collections.Generic;
using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	public class GetCommentsResponse : ResponseBase<GetCommentsStatus>
	{
		private IList<Comment> _comments = new List<Comment>();
		
		/// <summary>
		/// Gets list of comments
		/// </summary>
		public IList<Comment> Comments
		{ 
			get
			{
				return _comments;
			}
		}
	}
}
