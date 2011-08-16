using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using BoxSync.Core.Primitives;
using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Base class for all integration tests
	/// </summary>
	[TestFixture]
	public abstract class IntegrationTestBase
	{
		protected TestContext Context
		{
			get; 
			set;
		}

		[SetUp]
		public void SetUp()
		{
			InitializeContext();
		}

		[TearDown]
		public void CleanUp()
		{
			if (Context != null)
			{
				if (Context.Manager != null)
				{
					Context.Manager.Logout();
				}

				Context = null;
			}
		}

		/// <summary>
		/// Service login
		/// </summary>
		protected string Login
		{
			get
			{
				return ConfigurationManager.AppSettings["login"];
			}
		}

		/// <summary>
		/// Service password
		/// </summary>
		protected string Password
		{
			get
			{
				return ConfigurationManager.AppSettings["password"];
			}
		}

		/// <summary>
		/// Application key
		/// </summary>
		protected string ApplicationKey
		{
			get
			{
				return ConfigurationManager.AppSettings["applicationKey"];
			}
		}

		/// <summary>
		/// Service Url
		/// </summary>
		protected string ServiceUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["serviceUrl"];
			}
		}

		protected void InitializeContext()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			string token;
			User user;

			manager.GetTicket(out ticket);

			SubmitAuthenticationInformation(ticket);

			manager.GetAuthenticationToken(ticket, out token, out user);

			Context = new TestContext
			          	{
			          		AuthenticatedUser = user,
			          		Manager = manager,
			          		Ticket = ticket,
			          		Token = token
			          	};
		}

		protected string SubmitAuthenticationInformation(string ticket)
		{
			string uploadResult = null;

			using (WebClient client = new WebClient ())
			{

				client.Headers.Add("Content-Type:application/x-www-form-urlencoded");

				Uri destinationAddress = new Uri("http://www.box.net/api/1.0/auth/" + ticket);

				ManualResetEvent submitFinishedEvent = new ManualResetEvent(false);

				Action submitLoginPassword = () =>
				{
					uploadResult = client.UploadString(destinationAddress, "POST",
													   "login=" + Login +
													   "&password=" +
													   Password +
													   "&dologin=1&__login=1");
				};

				AsyncCallback callback = asyncResult =>
				{
					ManualResetEvent submitFinished = (ManualResetEvent)asyncResult.AsyncState;

					submitFinished.Set();
				};

				IAsyncResult asyncResult2 = submitLoginPassword.BeginInvoke(callback, submitFinishedEvent);

				submitFinishedEvent.WaitOne();

				submitLoginPassword.EndInvoke(asyncResult2);
			}

			return uploadResult;
		}

		protected static CreateFolderResponse CreateFolder(BoxManager manager, string folderName)
		{
			return manager.CreateFolder(folderName, 0, false);
		}

		protected static UploadFileResponse UploadTemporaryFile(BoxManager manager)
		{
			byte[] fileContent = Encoding.UTF8.GetBytes(Guid.Empty.ToString());

			return UploadTemporaryFile(manager, fileContent, 0);
		}

		protected static UploadFileResponse UploadTemporaryFile(BoxManager manager, byte[] fileContent, long folderID)
		{
			string tempFileName = Path.GetTempFileName();

			System.IO.File.WriteAllBytes(tempFileName, fileContent);

			return manager.AddFile(tempFileName, 0);
		}

		protected static void DeleteTemporaryFile(BoxManager manager, long objectID)
		{
			manager.DeleteObject(objectID, ObjectType.File);
		}

		protected static void DeleteFolder(BoxManager manager, long folderID)
		{
			manager.DeleteObject(folderID, ObjectType.Folder);
		}
	}
}
