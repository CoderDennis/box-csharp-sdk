using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;

using NUnit.Framework;


namespace BoxSync.Core.UnitTests.Primitives
{
	[TestFixture]
	public class CommentTests
	{
		[Test]
		public void TestConstructor()
		{
			DateTime parentCommentCreatedOn = new DateTime(2010, 1, 1, 2, 3, 4);
			const string parentCommentAvatarUrl = "http://jdhfgbsdjh.jdh";
			const long parentCommentID = 36756327;
			const string parentCommentMessage = "ksjdvbsdjkbgvksdjbvgsldk usgis gygfawekl gwieaugw29375293#$%^&";
			const long parentCommentUserID = 723648;
			const string parentCommentUserName = "skjdgbs";

			DateTime childCommentCreatedOn = new DateTime(2010, 3, 5, 5, 3, 4);
			const string childCommentAvatarUrl = "http://kjdfgblsdlsfkn.jdh";
			const long childCommentID = 5234216543;
			const string childCommentMessage = "ieurgt834q7t34nv 34t 4ty 34p8t 34qt";
			const long childCommentUserID = 2389529873;
			const string childCommentUserName = "kll;kn;oih";

			DateTime grandChildCommentCreatedOn = new DateTime(2010, 9, 5, 9, 3, 9);
			const string grandChildCommentAvatarUrl = "http://skdgbdskb.jdh";
			const long grandChildCommentID = 12541;
			const string grandChildCommentMessage = "ылдкпоиыл";
			const long grandChildCommentUserID = 346346;
			const string grandChildCommentUserName = "єйжцщойзцк й";

			SOAPComment parentSoapComment = new SOAPComment
			                          	{
			                          		avatar_url = parentCommentAvatarUrl,
			                          		comment_id = parentCommentID,
			                          		created = (long) UnixTimeConverter.Instance.ToUnixTime(parentCommentCreatedOn),
			                          		message = parentCommentMessage,
			                          		user_id = parentCommentUserID,
			                          		user_name = parentCommentUserName
			                          	};

			SOAPComment childSoapComment = new SOAPComment
			                               	{
												avatar_url = childCommentAvatarUrl,
												comment_id = childCommentID,
												created = (long)UnixTimeConverter.Instance.ToUnixTime(childCommentCreatedOn),
												message = childCommentMessage,
												user_id = childCommentUserID,
												user_name = childCommentUserName
			                               	};

			SOAPComment grandChildSoapComment = new SOAPComment
			                                    	{
														avatar_url = grandChildCommentAvatarUrl,
														comment_id = grandChildCommentID,
														created = (long)UnixTimeConverter.Instance.ToUnixTime(grandChildCommentCreatedOn),
														message = grandChildCommentMessage,
														user_id = grandChildCommentUserID,
														user_name = grandChildCommentUserName
			                                    	};

			parentSoapComment.reply_comments = new[] {childSoapComment};
			childSoapComment.reply_comments = new[] { grandChildSoapComment };

			Comment comment = new Comment(parentSoapComment);

			Assert.AreEqual(parentCommentID, comment.ID);
			Assert.AreEqual(parentCommentUserID, comment.UserID);
			Assert.AreEqual(parentCommentCreatedOn, comment.CreatedOn);

			StringAssert.AreEqualIgnoringCase(parentCommentAvatarUrl, comment.AvatarUrl);
			StringAssert.AreEqualIgnoringCase(parentCommentMessage, comment.Text);
			StringAssert.AreEqualIgnoringCase(parentCommentUserName, comment.UserName);
			
			Assert.AreEqual(1, comment.ReplyComments.Count);


			Assert.AreEqual(childCommentID, comment.ReplyComments[0].ID);
			Assert.AreEqual(childCommentUserID, comment.ReplyComments[0].UserID);
			Assert.AreEqual(childCommentCreatedOn, comment.ReplyComments[0].CreatedOn);

			StringAssert.AreEqualIgnoringCase(childCommentAvatarUrl, comment.ReplyComments[0].AvatarUrl);
			StringAssert.AreEqualIgnoringCase(childCommentMessage, comment.ReplyComments[0].Text);
			StringAssert.AreEqualIgnoringCase(childCommentUserName, comment.ReplyComments[0].UserName);

			Assert.AreEqual(1, comment.ReplyComments[0].ReplyComments.Count);


			Assert.AreEqual(grandChildCommentID, comment.ReplyComments[0].ReplyComments[0].ID);
			Assert.AreEqual(grandChildCommentUserID, comment.ReplyComments[0].ReplyComments[0].UserID);
			Assert.AreEqual(grandChildCommentCreatedOn, comment.ReplyComments[0].ReplyComments[0].CreatedOn);

			StringAssert.AreEqualIgnoringCase(grandChildCommentAvatarUrl, comment.ReplyComments[0].ReplyComments[0].AvatarUrl);
			StringAssert.AreEqualIgnoringCase(grandChildCommentMessage, comment.ReplyComments[0].ReplyComments[0].Text);
			StringAssert.AreEqualIgnoringCase(grandChildCommentUserName, comment.ReplyComments[0].ReplyComments[0].UserName);

			Assert.AreEqual(0, comment.ReplyComments[0].ReplyComments[0].ReplyComments.Count);
		}
	}
}
