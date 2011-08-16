using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;
using NUnit.Framework;
using File=BoxSync.Core.Primitives.File;

namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Set of tests for "GetUpdates" web method
	/// </summary>
	[TestFixture]
	public class GetUpdatesTests : IntegrationTestBase
	{
		/// <summary>
		/// Tests successful scenario of "GetUpdates" method call
		/// </summary>
		[Test]
		public void TestGetUpdates()
		{
			UploadFileResponse uploadResponse = UploadTemporaryFile(Context.Manager);
		    UploadFileError status = uploadResponse.UploadedFileStatus[uploadResponse.UploadedFileStatus.Keys.ElementAt(0)];

            Assert.AreEqual(UploadFileError.None, status);

		    GetFileInfoResponse getFileInfoResponse =
		        Context.Manager.GetFileInfo(uploadResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

            Assert.AreEqual(GetFileInfoStatus.Successful, getFileInfoResponse.Status);

            DateTime createdDate = getFileInfoResponse.File.Created;

            Assert.AreNotEqual(DateTime.MinValue, createdDate);
			
            Assert.AreEqual(UploadFileStatus.Successful, uploadResponse.Status);

			GetUpdatesResponse getUpdatesResponse = Context.Manager.GetUpdates(createdDate.AddSeconds(-1), createdDate.AddSeconds(1), GetUpdatesOptions.NoZip);

			DeleteTemporaryFile(Context.Manager, uploadResponse.UploadedFileStatus.Keys.ToArray()[0].ID);

			Assert.IsNull(getUpdatesResponse.Error);
			Assert.IsNull(getUpdatesResponse.UserState);
			Assert.AreEqual(GetUpdatesStatus.Successful, getUpdatesResponse.Status);
		}

		
	}
}
