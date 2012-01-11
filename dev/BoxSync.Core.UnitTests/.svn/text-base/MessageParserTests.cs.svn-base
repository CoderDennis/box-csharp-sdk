using System;
using System.Linq;
using System.Linq.Expressions;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;

using NUnit.Framework;

using File=BoxSync.Core.Primitives.File;


namespace BoxSync.Core.UnitTests
{
	[TestFixture]
	public class MessageParserTests
	{
		[Test]
		public void TestParseFolderStructureMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\folder_structure.xml");
			
			Expression<Func<long, TagPrimitive>> materializeTag = tagID => GetTag(tagID);

			Folder folder = MessageParser.Instance.ParseFolderStructureMessage(data, materializeTag);

			Assert.IsNotNull(folder);
			Assert.AreEqual(0, folder.ID);
			Assert.AreEqual(1, folder.Files.Count);
			Assert.AreEqual(1, folder.Folders.Count);
			Assert.AreEqual(8193, folder.Size);
			Assert.AreEqual(4497081, folder.OwnerID);
			Assert.AreEqual(0, folder.Tags.Count);

			Assert.AreEqual("somename", folder.Folders[0].Name);
			Assert.AreEqual(23001888, folder.Folders[0].ID);
			Assert.AreEqual(4497082, folder.Folders[0].OwnerID);
			Assert.IsTrue(folder.Folders[0].IsShared.HasValue);
			Assert.IsTrue(folder.Folders[0].IsShared.Value);
			Assert.AreEqual(0, folder.Folders[0].Size);
			Assert.AreEqual(0, folder.Folders[0].FileCount);
			StringAssert.IsMatch("some stupid desription", folder.Folders[0].Description);
			
			Assert.AreEqual(2, folder.Folders[0].Tags.Count);
			Assert.AreEqual(301888, folder.Folders[0].Tags[0].ID);
			Assert.AreEqual("301888", folder.Folders[0].Tags[0].Text);
			Assert.AreEqual(301892, folder.Folders[0].Tags[1].ID);
			Assert.AreEqual("301892", folder.Folders[0].Tags[1].Text);

			Assert.AreEqual("somename2", folder.Folders[0].Folders[0].Name);
			Assert.AreEqual(23001890, folder.Folders[0].Folders[0].ID);
			Assert.AreEqual(4497083, folder.Folders[0].Folders[0].OwnerID);
			Assert.IsTrue(folder.Folders[0].Folders[0].IsShared.HasValue);
			Assert.IsFalse(folder.Folders[0].Folders[0].IsShared.Value);
			Assert.AreEqual(0, folder.Folders[0].Folders[0].Size);
			Assert.AreEqual(0, folder.Folders[0].Folders[0].FileCount);
			Assert.AreEqual(0, folder.Folders[0].Folders[0].Tags.Count);

			Assert.AreEqual("avatar.jpg", folder.Files[0].Name);
			Assert.AreEqual(238458103, folder.Files[0].ID);
			Assert.AreEqual(4497083, folder.Files[0].OwnerID);
			Assert.IsTrue(folder.Files[0].IsShared.HasValue);
			Assert.IsTrue(folder.Files[0].IsShared.Value);
			Assert.AreEqual(8193, folder.Files[0].Size);
			Assert.AreEqual(0, folder.Files[0].Tags.Count);
		}

		[Test]
		public void TestParseFolderSimpleStructureMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\folder_structure_simple.xml");

			Expression<Func<long, TagPrimitive>> materializeTag = tagID => GetTag(tagID);

			Folder folder = MessageParser.Instance.ParseFolderStructureMessage(data, materializeTag);

			Assert.IsNotNull(folder);
			Assert.AreEqual(0, folder.ID);
			Assert.AreEqual(2, folder.Files.Count);
			Assert.AreEqual(1, folder.Folders.Count);
			Assert.IsFalse(folder.Size.HasValue);
			Assert.IsFalse(folder.OwnerID.HasValue);
			Assert.AreEqual(0, folder.Tags.Count);

			Assert.AreEqual("test_1", folder.Folders[0].Name);
			Assert.AreEqual(26935530, folder.Folders[0].ID);
			Assert.IsFalse(folder.Folders[0].OwnerID.HasValue);
			Assert.IsFalse(folder.Folders[0].IsShared.HasValue);
			Assert.IsFalse(folder.Folders[0].Size.HasValue);
			Assert.AreEqual(0, folder.Folders[0].FileCount);
			Assert.IsNull(folder.Folders[0].Description);

			Assert.AreEqual(0, folder.Folders[0].Tags.Count);

			Assert.AreEqual("3.1237931098.jpg", folder.Files[0].Name);
			Assert.AreEqual(276668870, folder.Files[0].ID);
			Assert.IsFalse(folder.Files[0].OwnerID.HasValue);
			Assert.IsFalse(folder.Files[0].IsShared.HasValue);
			Assert.AreEqual(50854, folder.Files[0].Size);
			Assert.AreEqual(0, folder.Files[0].Tags.Count);

			Assert.AreEqual("comedy.png", folder.Files[1].Name);
			Assert.AreEqual(276668886, folder.Files[1].ID);
			Assert.IsFalse(folder.Files[1].OwnerID.HasValue);
			Assert.IsFalse(folder.Files[1].IsShared.HasValue);
			Assert.AreEqual(896, folder.Files[1].Size);
			Assert.AreEqual(0, folder.Files[1].Tags.Count);
		}


		[Test]
		public void TestSuccessParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\upload_ok.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Successful, response.Status);
			Assert.AreEqual(2, response.UploadedFileStatus.Count);

			File fp1 = response.UploadedFileStatus.Keys.ElementAt(0);

			Assert.AreEqual(5996, fp1.ID);
			Assert.AreEqual("read_me.txt", fp1.Name);
			Assert.IsTrue(fp1.IsShared.HasValue);
			Assert.IsFalse(fp1.IsShared.Value);

			Assert.AreEqual(UploadFileError.None, response.UploadedFileStatus[fp1]);

			File fp2 = response.UploadedFileStatus.Keys.ElementAt(1);

			Assert.AreEqual(UploadFileError.FileSizeLimitExceeded, response.UploadedFileStatus[fp2]);

			Assert.AreEqual("Rally.avi", fp2.Name);
			Assert.AreEqual(0, fp2.ID);
			Assert.IsFalse(fp2.OwnerID.HasValue);
			Assert.IsNull(fp2.SharedLink);
			Assert.IsFalse(fp2.PermissionFlags.HasValue);
		}

		[Test]
		public void TestApplicationRestrictedParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\application_restricted.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.ApplicationRestricted, response.Status);
			Assert.AreEqual(0, response.UploadedFileStatus.Count);
		}

		[Test]
		public void TestNotLoggedIDParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\not_logged_id.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.NotLoggedID, response.Status);
			Assert.AreEqual(0, response.UploadedFileStatus.Count);
		}

		[Test]
		public void TestUploadFailedFileSizeLimitExceededParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\upload_some_files_failed-filesize_limit_exceeded.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Failed, response.Status);
			Assert.AreEqual(1, response.UploadedFileStatus.Count);

			File fp1 = response.UploadedFileStatus.Keys.ElementAt(0);

			Assert.AreEqual(0, fp1.ID);
			Assert.AreEqual("Rally.avi", fp1.Name);
			Assert.IsFalse(fp1.IsShared.HasValue);
			Assert.AreEqual(UploadFileError.FileSizeLimitExceeded, response.UploadedFileStatus[fp1]);
		}

		[Test]
		public void TestUploadFailedNotEnoughFreeSpaceParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\upload_some_files_failed-not_enough_free_space.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Failed, response.Status);
			Assert.AreEqual(1, response.UploadedFileStatus.Count);

			File fp1 = response.UploadedFileStatus.Keys.ElementAt(0);

			Assert.AreEqual(0, fp1.ID);
			Assert.AreEqual("Rally2.avi", fp1.Name);
			Assert.IsFalse(fp1.IsShared.HasValue);
			Assert.AreEqual(UploadFileError.NotEnoughFreeSpace, response.UploadedFileStatus[fp1]);
		}

		[Test]
		public void TestUploadFailedAccessDeniedParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\upload_some_files_failed-access_denied.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Failed, response.Status);
			Assert.AreEqual(1, response.UploadedFileStatus.Count);

			File fp1 = response.UploadedFileStatus.Keys.ElementAt(0);

			Assert.AreEqual(0, fp1.ID);
			Assert.AreEqual("Rally4.avi", fp1.Name);
			Assert.IsFalse(fp1.IsShared.HasValue);
			Assert.AreEqual(UploadFileError.AccessDenied, response.UploadedFileStatus[fp1]);
		}

		[Test]
		public void TestUploadFailedUnknownErrorParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\upload_some_files_failed-unknown_error.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Failed, response.Status);
			Assert.AreEqual(1, response.UploadedFileStatus.Count);

			File fp1 = response.UploadedFileStatus.Keys.ElementAt(0);

			Assert.AreEqual(0, fp1.ID);
			Assert.AreEqual("Rally5.avi", fp1.Name);
			Assert.IsFalse(fp1.IsShared.HasValue);
			Assert.AreEqual(UploadFileError.Unknown, response.UploadedFileStatus[fp1]);
		}


		[Test]
		public void TestNoFilesElementParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\no_files_element.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Successful, response.Status);
			Assert.AreEqual(0, response.UploadedFileStatus.Count);
		}

		[Test]
		public void TestUnknownStatusParseUploadResponseMessage()
		{
			string data = System.IO.File.ReadAllText(@"..\..\Data\upload_file_response\unknown_status.xml");

			UploadFileResponse response = MessageParser.Instance.ParseUploadResponseMessage(data);

			Assert.AreEqual(UploadFileStatus.Unknown, response.Status);
			Assert.AreEqual(0, response.UploadedFileStatus.Count);
		}

		[Test]
		public void TestParseExportTagsMessage()
		{
			int firstTagID = 37;
			string firstTagText = "tag1";

			int secondTagID = 38;
			string secondTagText = "tag2";

			int thirdTagID = 1234567890;
			string thirdTagText = "tag_name_3";

			string data = System.IO.File.ReadAllText(@"..\..\Data\export_tags.xml");

			TagPrimitiveCollection tagList = MessageParser.Instance.ParseExportTagsMessage(data);

			Assert.AreEqual(3, tagList.Count);

			TagPrimitive firstTag = tagList.GetTag(firstTagID);
			Assert.AreEqual(firstTagID, firstTag.ID);
			StringAssert.IsMatch(firstTagText, firstTag.Text);
			StringAssert.IsMatch(firstTagText, tagList.GetTagText(firstTagID));

			TagPrimitive secondTag = tagList.GetTag(secondTagID);
			Assert.AreEqual(secondTagID, secondTag.ID);
			StringAssert.IsMatch(secondTagText, secondTag.Text);
			StringAssert.IsMatch(secondTagText, tagList.GetTagText(secondTagID));

			TagPrimitive thirdTag = tagList.GetTag(1234567890);
			Assert.AreEqual(thirdTagID, thirdTag.ID);
			StringAssert.IsMatch(thirdTagText, thirdTag.Text);
			StringAssert.IsMatch(thirdTagText, tagList.GetTagText(thirdTagID));
		}


		private TagPrimitive GetTag(long id)
		{
			return new TagPrimitive(id, id.ToString());
		}

		private User GetUser(int id)
		{
			SOAPUser user = new SOAPUser();

			user.user_id = id;
			user.access_id = 12345;
			user.email = "some@email.com";
			user.login = "some@email.com";
			user.max_upload_size = 12345;
			user.space_amount = 1234567890;
			user.space_used = 1234;

			return new User(user);
		}
	}
}
