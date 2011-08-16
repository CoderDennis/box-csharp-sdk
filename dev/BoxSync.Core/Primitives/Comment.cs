using System;
using System.Collections.Generic;

using BoxSync.Core.ServiceReference;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents user's comment
	/// </summary>
	public class Comment
	{
		/// <summary>
		/// Initializes type instance
		/// </summary>
		public Comment()
		{
		}

		/// <summary>
		/// Initializes type instance
		/// </summary>
		/// <param name="comment">Comment object</param>
		internal Comment(SOAPComment comment)
		{
			AvatarUrl = comment.avatar_url;
			ID = comment.comment_id;
			CreatedOn = UnixTimeConverter.Instance.FromUnixTime(comment.created);
			Text = comment.message;
			UserID = comment.user_id;
			UserName = comment.user_name;

			_replyComments.AddRange(ConvertReplyComments(comment.reply_comments));
		}

		private static IList<Comment> ConvertReplyComments(SOAPComment[] comments)
		{
			List<Comment> result = new List<Comment>();

			if (comments != null)
			{
				for (int i = 0; i < comments.Length; i++)
				{
					result.Add(new Comment(comments[i]));
				}
			}

			return result;
		}

		/// <summary>
		/// Gets or sets ID of the comment
		/// </summary>
		public long ID
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the URL of user's avatar image 
		/// </summary>
		public string AvatarUrl
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets comment's creation date
		/// </summary>
		public DateTime CreatedOn
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets comment's text
		/// </summary>
		public string Text
		{
			get; 
			set;
		}

		private readonly List<Comment> _replyComments = new List<Comment>();

		/// <summary>
		/// Gets reply comments
		/// </summary>
		public IList<Comment> ReplyComments
		{ 
			get
			{
				return _replyComments;
			}
		}

		/// <summary>
		/// Gets or sets ID of the user who posted comment
		/// </summary>
		public long UserID
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets name of the user who posted comment
		/// </summary>
		public string UserName
		{
			get; 
			set;
		}
	}
}
