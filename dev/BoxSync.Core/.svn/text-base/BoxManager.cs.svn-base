using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;

using ICSharpCode.SharpZipLib.Zip;

using File=BoxSync.Core.Primitives.File;


namespace BoxSync.Core
{
	/// <summary>
	/// Provides methods for using Box.NET SOAP web service
	/// </summary>
	public sealed partial class BoxManager
	{
		private const string UPLOAD_FILE_URI_TEMPLATE = "http://upload.box.net/api/1.0/upload/{0}/{1}";
		private const string OVERWRITE_FILE_URI_TEMPLATE = "http://upload.box.net/api/1.0/overwrite/{0}/{1}";
		private const string FILE_NEW_COPY_URI_TEMPLATE = "http://upload.box.net/api/1.0/new_copy/{0}/{1}";
		
		private readonly boxnetService _service;
		private readonly string _apiKey;
		private string _token;
		private readonly IWebProxy _proxy;
		private TagPrimitiveCollection _tagCollection;
		

		/// <summary>
		/// Instantiates BoxManager
		/// </summary>
		/// <param name="applicationApiKey">The unique API key which is assigned to application</param>
		/// <param name="serviceUrl">Box.NET SOAP service Url</param>
		/// <param name="proxy">Proxy information</param>
		public BoxManager(string applicationApiKey, string serviceUrl, IWebProxy proxy) :
			this(applicationApiKey, serviceUrl, proxy, null)
		{
		}

		/// <summary>
		/// Instantiates BoxManager
		/// </summary>
		/// <param name="applicationApiKey">The unique API key which is assigned to application</param>
		/// <param name="serviceUrl">Box.NET SOAP service Url</param>
		/// <param name="proxy">Proxy information</param>
		/// <param name="authorizationToken">Valid authorization token</param>
		public BoxManager(
			string applicationApiKey, 
			string serviceUrl, 
			IWebProxy proxy, 
			string authorizationToken)
		{
			_apiKey = applicationApiKey;
			
			_service = new boxnetService();
			_proxy = proxy;
			
			_service.Url = serviceUrl;
			_service.Proxy = proxy;

			_token = authorizationToken;
		}


		/// <summary>
		/// Gets or sets authentication token required for communication 
		/// between Box.NET service and user's application
		/// </summary>
		public string AuthenticationToken
		{
			get
			{
				return _token;
			}
			set
			{
				_token = value;
			}
		}

		/// <summary>
		/// Proxy used to access Box.NET service
		/// </summary>
		public IWebProxy Proxy
		{
			get
			{
				return _proxy;
			}
		}


		#region AuthenticateUser

		/// <summary>
		/// Authenticates user
		/// </summary>
		/// <param name="login">Account login</param>
		/// <param name="password">Account password</param>
		/// <param name="method"></param>
		/// <param name="authenticationToken">Authentication token</param>
		/// <param name="authenticatedUser">Authenticated user information</param>
		/// <returns>Operation result</returns>
		/// <exception cref="NotSupportedException">The exception is thrown every time you call the method</exception>
		[Obsolete("This method is no longer supported by Box.NET API")]
		public AuthenticationStatus AuthenticateUser(
			string login, 
			string password, 
			string method, 
			out string authenticationToken, 
			out User authenticatedUser)
		{
			throw new NotSupportedException("This method is no longer supported by Box.NET API");
		}

		/// <summary>
		/// Authenticates user
		/// </summary>
		/// <param name="login">Account login</param>
		/// <param name="password">Account password</param>
		/// <param name="method"></param>
		/// <returns>Operation result</returns>
		/// <exception cref="NotSupportedException">The exception is thrown every time you call the method</exception>
		[Obsolete("This method is no longer supported by Box.NET API")]
		public AuthenticateUserResponse AuthenticateUser(
			string login,
			string password,
			string method)
		{
			throw new NotSupportedException("This method is no longer supported by Box.NET API");
		}

		/// <summary>
		/// Authenticates user
		/// </summary>
		/// <param name="login">Account login</param>
		/// <param name="password">Account password</param>
		/// <param name="method"></param>
		/// <param name="authenticateUserCompleted">Callback method which will be invoked when operation completes</param>
		/// <exception cref="NotSupportedException">The exception is thrown every time you call the method</exception>
		[Obsolete("This method is no longer supported by Box.NET API")]
		public void AuthenticateUser(
			string login,
			string password,
			string method,
			OperationFinished<AuthenticateUserResponse> authenticateUserCompleted)
		{
			AuthenticateUser(login, password, method, authenticateUserCompleted, null);
		}

		/// <summary>
		/// Authenticates user
		/// </summary>
		/// <param name="login">Account login</param>
		/// <param name="password">Account password</param>
		/// <param name="method"></param>
		/// <param name="authenticateUserCompleted">Callback method which will be invoked when operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="authenticateUserCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="NotSupportedException">The exception is thrown every time you call the method</exception>
		[Obsolete("This method is no longer supported by Box.NET API")]
		public void AuthenticateUser(
			string login,
			string password,
			string method,
			OperationFinished<AuthenticateUserResponse> authenticateUserCompleted,
			object userState)
		{
			throw new NotSupportedException("This method is no longer supported by Box.NET API");
		}

		#endregion

		#region GetAuthenticationToken

		/// <summary>
		/// Gets authentication token required for communication between Box.NET service and user's application.
		/// Method has to be called after the user has authorized themself on Box.NET site
		/// </summary>
		/// <param name="authenticationTicket">Athentication ticket</param>
		/// <param name="authenticationToken">Authentication token</param>
		/// <param name="authenticatedUser">Authenticated user account information</param>
		/// <returns>Operation result</returns>
		public GetAuthenticationTokenStatus GetAuthenticationToken(
			string authenticationTicket, 
			out string authenticationToken, 
			out User authenticatedUser)
		{
			SOAPUser user;

			string result = _service.get_auth_token(_apiKey, authenticationTicket, out authenticationToken, out user);

			authenticatedUser = user == null ? null : new User(user);
			_token = authenticationToken;

			return StatusMessageParser.ParseGetAuthenticationTokenStatus(result);
		}

		/// <summary>
		/// Gets authentication token required for communication between Box.NET service and user's application.
		/// Method has to be called after the user has authorized themself on Box.NET site
		/// </summary>
		/// <param name="authenticationTicket">Athentication ticket</param>
		/// <param name="getAuthenticationTokenCompleted">Call back method which will be invoked when operation completes</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="getAuthenticationTokenCompleted"/> is null</exception>
		public void GetAuthenticationToken(
			string authenticationTicket, 
			OperationFinished<GetAuthenticationTokenResponse> getAuthenticationTokenCompleted)
		{
			GetAuthenticationToken(authenticationTicket, getAuthenticationTokenCompleted, null);
		}

		/// <summary>
		/// Gets authentication token required for communication between Box.NET service and user's application.
		/// Method has to be called after the user has authorized themself on Box.NET site
		/// </summary>
		/// <param name="authenticationTicket">Athentication ticket</param>
		/// <param name="getAuthenticationTokenCompleted">Callback method which will be invoked when operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getAuthenticationTokenCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="getAuthenticationTokenCompleted"/> is null</exception>
		public void GetAuthenticationToken(
			string authenticationTicket, 
			OperationFinished<GetAuthenticationTokenResponse> getAuthenticationTokenCompleted, 
			object userState)
		{
			ThrowIfParameterIsNull(getAuthenticationTokenCompleted, "getAuthenticationTokenCompleted");

			_service.get_auth_tokenCompleted += GetAuthenticationTokenFinished;

			object[] state = {getAuthenticationTokenCompleted, userState};

			_service.get_auth_tokenAsync(_apiKey, authenticationTicket, state);
		}

		private void GetAuthenticationTokenFinished(object sender, get_auth_tokenCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			Exception error = null;

			OperationFinished<GetAuthenticationTokenResponse> getAuthenticationTokenCompleted =
				(OperationFinished<GetAuthenticationTokenResponse>) state[0];

			GetAuthenticationTokenStatus status;

			if (e.Error == null)
			{
				status = StatusMessageParser.ParseGetAuthenticationTokenStatus(e.Result);

				if (status == GetAuthenticationTokenStatus.Unknown)
				{
					error = new UnknownOperationStatusException(e.Result);
				}
			}
			else
			{
				status = GetAuthenticationTokenStatus.Failed;
				error = e.Error;
			}

			GetAuthenticationTokenResponse response = new GetAuthenticationTokenResponse
			                                          	{
			                                          		Status = status,
			                                          		UserState = state[1],
			                                          		Error = error,
															AuthenticationToken = string.Empty
			                                          	};

			if (response.Status == GetAuthenticationTokenStatus.Successful)
			{
				User authenticatedUser = new User(e.user);
				response.AuthenticatedUser = authenticatedUser;
				response.AuthenticationToken = e.auth_token;
				_token = e.auth_token;
			}

			getAuthenticationTokenCompleted(response);
		}

		#endregion

		#region GetTicket

		/// <summary>
		/// Gets ticket which is used to generate an authentication page 
		/// for the user to login
		/// </summary>
		/// <param name="authenticationTicket">Authentication ticket</param>
		/// <returns>Operation status</returns>
		public GetTicketStatus GetTicket(out string authenticationTicket)
		{
			string result = _service.get_ticket(_apiKey, out authenticationTicket);

			return StatusMessageParser.ParseGetTicketStatus(result);
		}

		/// <summary>
		/// Gets ticket which is used to generate an authentication page 
		/// for the user to login
		/// </summary>
		/// <param name="getAuthenticationTicketCompleted">Call back method which will be invoked when operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getAuthenticationTicketCompleted"/> is <c>null</c></exception>
		public void GetTicket(OperationFinished<GetTicketResponse> getAuthenticationTicketCompleted)
		{
			GetTicket(getAuthenticationTicketCompleted, null);
		}

		/// <summary>
		/// Gets ticket which is used to generate an authentication page 
		/// for the user to login
		/// </summary>
		/// <param name="getAuthenticationTicketCompleted">Call back method which will be invoked when operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getAuthenticationTicketCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getAuthenticationTicketCompleted"/> is <c>null</c></exception>
		public void GetTicket(
			OperationFinished<GetTicketResponse> getAuthenticationTicketCompleted, 
			object userState)
		{
			ThrowIfParameterIsNull(getAuthenticationTicketCompleted, "getAuthenticationTicketCompleted");

			object[] data = {getAuthenticationTicketCompleted, userState};

			_service.get_ticketCompleted += GetTicketFinished;

			_service.get_ticketAsync(_apiKey, data);
		}


		private void GetTicketFinished(object sender, get_ticketCompletedEventArgs e)
		{
			object[] data = (object[]) e.UserState;
			OperationFinished<GetTicketResponse> getAuthenticationTicketCompleted = (OperationFinished<GetTicketResponse>)data[0];
			GetTicketResponse response;
			
			if (e.Error != null)
			{
				response = new GetTicketResponse
				           	{
				           		Status = GetTicketStatus.Failed,
				           		UserState = data[1],
								Error = e.Error
				           	};
			}
			else
			{
				GetTicketStatus status = StatusMessageParser.ParseGetTicketStatus(e.Result);

				Exception error = status == GetTicketStatus.Unknown
				                  	?
				                  		new UnknownOperationStatusException(e.Result)
				                  	:
				                  		null;

				response = new GetTicketResponse
				           	{
				           		Status = status,
				           		Ticket = e.ticket,
				           		UserState = data[1],
								Error = error
				           	};
			}

			getAuthenticationTicketCompleted(response);
		}

		#endregion

		#region Upload file

		/// <summary>
		/// Adds the specified local file to the specified folder
		/// </summary>
		/// <param name="filePath">Path to the file which needs to be uploaded</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <returns>Operation status</returns>
		public UploadFileResponse AddFile(
			string filePath, 
			long destinationFolderID)
		{
			return AddFiles(new[] {filePath}, destinationFolderID);
		}

		/// <summary>
		/// Asynchronously adds the specified local file to the specified folder
		/// </summary>
		/// <param name="filePath">Path to the file which needs to be uploaded</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="fileUploadCompleted">Callback method which will be invoked after file-upload operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="fileUploadCompleted"/> is <c>null</c></exception>
		public void AddFile(
			string filePath,
			long destinationFolderID,
			OperationFinished<UploadFileResponse> fileUploadCompleted)
		{
			AddFile(filePath, destinationFolderID, fileUploadCompleted, null);
		}

		/// <summary>
		/// Asynchronously adds the specified local file to the specified folder
		/// </summary>
		/// <param name="filePath">Path to the file which needs to be uploaded</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="fileUploadCompleted">Callback method which will be invoked after file-upload operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="fileUploadCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="fileUploadCompleted"/> is <c>null</c></exception>
		public void AddFile(
			string filePath,
			long destinationFolderID,
			OperationFinished<UploadFileResponse> fileUploadCompleted,
			object userState)
		{
			AddFiles(
				new[] { filePath },
				destinationFolderID,
				false,
				null,
				null,
				fileUploadCompleted,
				userState);
		}


		/// <summary>
		/// Adds files to the specified folder
		/// </summary>
		/// <param name="files">Paths to files to upload</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <returns>Operation status</returns>
		public UploadFileResponse AddFiles(
			string[] files, 
			long destinationFolderID)
		{
			return AddFiles(files, destinationFolderID, false, null, null);
		}

		/// <summary>
		/// Adds files to the specified folder
		/// </summary>
		/// <param name="files">Paths to files to upload</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="isFilesShared">Indicates if uploaded files should be marked as shared</param>
		/// <param name="message">Text of the message to send in a notification email to all addresses in the <paramref name="emailsToNotify"/> list</param>
		/// <param name="emailsToNotify">List of email addresses to notify about newly uploaded files</param>
		/// <returns>Operation status</returns>
		public UploadFileResponse AddFiles(
			string[] files, 
			long destinationFolderID, 
			bool isFilesShared, 
			string message, 
			string[] emailsToNotify)
		{
			MultipartWebRequest request = new MultipartWebRequest(string.Format(UPLOAD_FILE_URI_TEMPLATE, _token, destinationFolderID), Proxy);
			UploadFileResponse response;
			
			try
			{
				string serverResponse = request.SubmitFiles(files, isFilesShared, message, emailsToNotify);
				response = MessageParser.Instance.ParseUploadResponseMessage(serverResponse);
			}
			catch (Exception ex)
			{
				response = new UploadFileResponse
				           	{
				           		Error = ex,
				           		Status = UploadFileStatus.Failed,
				           		UploadedFileStatus = new Dictionary<File, UploadFileError>()
				           	};
			}

			response.FolderID = destinationFolderID;

			return response;
		}

		/// <summary>
		/// Asynchronously adds files to the specified folder
		/// </summary>
		/// <param name="files">Paths to files to upload</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="isFilesShared">Indicates if uploaded files should be marked as shared</param>
		/// <param name="message">Text of the message to send in a notification email to all addresses in the <paramref name="emailsToNotify"/> list</param>
		/// <param name="emailsToNotify">List of email addresses to notify about newly uploaded files</param>
		/// <param name="filesUploadCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="filesUploadCompleted"/> is <c>null</c></exception>
		public void AddFiles(
			string[] files, 
			long destinationFolderID, 
			bool isFilesShared, 
			string message, 
			string[] emailsToNotify,
			OperationFinished<UploadFileResponse> filesUploadCompleted)
		{
			AddFiles(files, destinationFolderID, isFilesShared, message, emailsToNotify, filesUploadCompleted, null);
		}

		/// <summary>
		/// Asynchronously adds files to the specified folder
		/// </summary>
		/// <param name="files">Paths to files to upload</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="isFilesShared">Indicates if uploaded files should be marked as shared</param>
		/// <param name="message">Text of the message to send in a notification email to all addresses in the <paramref name="emailsToNotify"/> list</param>
		/// <param name="emailsToNotify">List of email addresses to notify about newly uploaded files</param>
		/// <param name="filesUploadCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="filesUploadCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="filesUploadCompleted"/> is <c>null</c></exception>
		public void AddFiles(
			string[] files,
			long destinationFolderID,
			bool isFilesShared,
			string message,
			string[] emailsToNotify,
			OperationFinished<UploadFileResponse> filesUploadCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(filesUploadCompleted, "filesUploadCompleted");

			MultipartWebRequest request =
				new MultipartWebRequest(string.Format(UPLOAD_FILE_URI_TEMPLATE, _token, destinationFolderID), Proxy);

			object[] state = new[] {filesUploadCompleted, userState, destinationFolderID};

			request.SubmitFiles(files, isFilesShared, message, emailsToNotify, UploadFilesFinished, state);
		}


		private void UploadFilesFinished(MultipartRequestUploadResponse uploadFilesResponse)
		{
			object[] state = (object[]) uploadFilesResponse.UserState;
			OperationFinished<UploadFileResponse> filesUploadCompleted = (OperationFinished<UploadFileResponse>) state[0];

			UploadFileResponse response = uploadFilesResponse.Error == null
			                              	?
			                              		MessageParser.Instance.ParseUploadResponseMessage(uploadFilesResponse.Status)
			                              	:
			                              		new UploadFileResponse();

			response.FolderID = (long) state[2];
			response.UserState = state[1];
			response.Error = uploadFilesResponse.Error;

			filesUploadCompleted(response);
		}

		#endregion

		#region Overwrite file

		/// <summary>
		/// Overwrites existing file with the new one
		/// </summary>
		/// <param name="filePath">Path to new file</param>
		/// <param name="fileID">ID of the old file to overwrite</param>
		/// <returns>Operation status</returns>
		public OverwriteFileResponse OverwriteFile(
			string filePath, 
			long fileID)
		{
			return OverwriteFile(filePath, fileID, false, null, null);
		}

		/// <summary>
		/// Overwrites existing file with the new one
		/// </summary>
		/// <param name="filePath">Path to new file</param>
		/// <param name="fileID">ID of the old file to overwrite</param>
		/// <param name="isFileShared">Indicates if uploaded file should be marked as shared</param>
		/// <param name="message">Text of the message to send in a notification email to all addresses in the <paramref name="emailsToNotify"/> list</param>
		/// <param name="emailsToNotify">List of email addresses to notify about newly uploaded files</param>
		/// <returns>Operation status</returns>
		public OverwriteFileResponse OverwriteFile(
			string filePath, 
			long fileID, 
			bool isFileShared, 
			string message, 
			string[] emailsToNotify)
		{
			MultipartWebRequest request = new MultipartWebRequest(string.Format(OVERWRITE_FILE_URI_TEMPLATE, _token, fileID), Proxy);
			OverwriteFileResponse response;

			try
			{
				string serverResponse = request.SubmitFiles(new[] { filePath }, isFileShared, message, emailsToNotify);
				response = MessageParser.Instance.ParseOverwriteFileResponseMessage(serverResponse);
			}
			catch (Exception ex)
			{
				response = new OverwriteFileResponse
				           	{
				           		Error = ex,
				           		Status = OverwriteFileStatus.Failed,
				           		UploadedFileStatus = new Dictionary<File, UploadFileError>()
				           	};
			}

			return response;
		}

		/// <summary>
		/// Asynchronously overwrites existing file with the new one
		/// </summary>
		/// <param name="filePath">Path to new file</param>
		/// <param name="fileID">ID of the old file to overwrite</param>
		/// <param name="overwriteFileCompleted">Callback method which will be invoked after file-overwrite operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="overwriteFileCompleted"/> is <c>null</c></exception>
		public void OverwriteFile(
			string filePath, 
			long fileID,
			OperationFinished<OverwriteFileResponse> overwriteFileCompleted)
		{
			OverwriteFile(filePath, fileID, false, null, null, overwriteFileCompleted, null);
		}

		/// <summary>
		/// Asynchronously overwrites existing file with the new one
		/// </summary>
		/// <param name="filePath">Path to new file</param>
		/// <param name="fileID">ID of the old file to overwrite</param>
		/// <param name="isFileShared">Indicates if uploaded file should be marked as shared</param>
		/// <param name="message">Text of the message to send in a notification email to all addresses in the <paramref name="emailsToNotify"/> list</param>
		/// <param name="emailsToNotify">List of email addresses to notify about newly uploaded files</param>
		/// <param name="overwriteFileCompleted">Callback method which will be invoked after file-overwrite operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="overwriteFileCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="overwriteFileCompleted"/> is <c>null</c></exception>
		public void OverwriteFile(
			string filePath,
			long fileID,
			bool isFileShared,
			string message,
			string[] emailsToNotify,
			OperationFinished<OverwriteFileResponse> overwriteFileCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(overwriteFileCompleted, "overwriteFileCompleted");

			MultipartWebRequest request =
				new MultipartWebRequest(string.Format(OVERWRITE_FILE_URI_TEMPLATE, _token, fileID), Proxy);

			object[] state = new[] { overwriteFileCompleted, userState, fileID };

			request.SubmitFiles(new[] { filePath }, isFileShared, message, emailsToNotify, OverwriteFileFinished, state);
		}

		private void OverwriteFileFinished(MultipartRequestUploadResponse uploadFilesResponse)
		{
			object[] state = (object[]) uploadFilesResponse.UserState;
			OperationFinished<OverwriteFileResponse> overwriteFileCompleted = (OperationFinished<OverwriteFileResponse>) state[0];

			OverwriteFileResponse overwriteFileResponse = uploadFilesResponse.Error == null
			                                              	?
			                                              		MessageParser.Instance.ParseOverwriteFileResponseMessage(
			                                              			uploadFilesResponse.Status)
			                                              	:
			                                              		new OverwriteFileResponse();

			overwriteFileResponse.UserState = state[1];
			overwriteFileResponse.Error = uploadFilesResponse.Error;

			overwriteFileCompleted(overwriteFileResponse);
		}

		#endregion

		#region GetFolderStructure

		/// <summary>
		/// Retrieves a user's root folder structure
		/// </summary>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <returns>Operation status</returns>
		public GetFolderStructureResponse GetRootFolderStructure(RetrieveFolderStructureOptions retrieveOptions)
		{
			return GetFolderStructure(0, retrieveOptions);
		}

		/// <summary>
		/// Retrieves a user's root folder structure
		/// </summary>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="folder">Root folder</param>
		/// <returns>Operation status</returns>
		[Obsolete("Use GetRootFolderStructure(RetrieveFolderStructureOptions):GetFolderStructureResponse")]
		public GetAccountTreeStatus GetRootFolderStructure(RetrieveFolderStructureOptions retrieveOptions, out Folder folder)
		{
			return GetFolderStructure(0, retrieveOptions, out folder);
		}

		/// <summary>
		/// Asynchronously retrieves a user's root folder structure
		/// </summary>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="getFolderStructureCompleted">Callback method which will be executed after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFolderStructureCompleted"/> is null</exception>
		public void GetRootFolderStructure(
			RetrieveFolderStructureOptions retrieveOptions,
			OperationFinished<GetFolderStructureResponse> getFolderStructureCompleted)
		{
			GetFolderStructure(0, retrieveOptions, getFolderStructureCompleted, null);
		}

		/// <summary>
		/// Asynchronously retrieves a user's root folder structure
		/// </summary>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="getFolderStructureCompleted">Callback method which will be executed after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getFolderStructureCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFolderStructureCompleted"/> is null</exception>
		public void GetRootFolderStructure(
			RetrieveFolderStructureOptions retrieveOptions,
			OperationFinished<GetFolderStructureResponse> getFolderStructureCompleted,
			object userState)
		{
			GetFolderStructure(0, retrieveOptions, getFolderStructureCompleted, userState);
		}

		/// <summary>
		/// Retrieves a user's folder structure by ID
		/// </summary>
		/// <param name="folderID">ID of the folder to retrieve</param>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <returns>Operation status</returns>
		public GetFolderStructureResponse GetFolderStructure(
			long folderID,
			RetrieveFolderStructureOptions retrieveOptions)
		{
			Folder folder = null;

			byte[] folderInfoXml;

			string result = _service.get_account_tree(_apiKey, _token, folderID, retrieveOptions.ToStringArray(),
			                                          out folderInfoXml);
			GetAccountTreeStatus status = StatusMessageParser.ParseGetAccountTreeStatus(result);

			if (status == GetAccountTreeStatus.Successful)
			{
				string folderInfo = null;

				if (!retrieveOptions.Contains(RetrieveFolderStructureOptions.NoZip))
				{
					folderInfoXml = Unzip(folderInfoXml);
				}

				if (folderInfoXml != null)
				{
					folderInfo = Encoding.ASCII.GetString(folderInfoXml);
				}

				folder = ParseFolderStructureXmlMessage(folderInfo);
			}

			return new GetFolderStructureResponse
			       	{
			       		Folder = folder,
			       		Status = status
			       	};
		}

		/// <summary>
		/// Retrieves a user's folder structure by ID
		/// </summary>
		/// <param name="folderID">ID of the folder to retrieve</param>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="folder">Folder object</param>
		/// <returns>Operation status</returns>
		[Obsolete("Use GetFolderStructure(long, RetrieveFolderStructureOptions):GetFolderStructureResponse")]
		public GetAccountTreeStatus GetFolderStructure(
			long folderID, 
			RetrieveFolderStructureOptions retrieveOptions, 
			out Folder folder)
		{
			folder = null;

			byte[] folderInfoXml;
			
			string result = _service.get_account_tree(_apiKey, _token, folderID, retrieveOptions.ToStringArray(), out folderInfoXml);
			GetAccountTreeStatus status = StatusMessageParser.ParseGetAccountTreeStatus(result);

			switch (status)
			{
				case GetAccountTreeStatus.Successful:
					string folderInfo = null;

					if (!retrieveOptions.Contains(RetrieveFolderStructureOptions.NoZip))
					{
						folderInfoXml = Unzip(folderInfoXml);
					}

					if (folderInfoXml != null)
					{
						folderInfo = Encoding.ASCII.GetString(folderInfoXml);
					}

					folder = ParseFolderStructureXmlMessage(folderInfo);
					break;
			}

			return status;
		}

		/// <summary>
		/// Asynchronously retrieves a user's folder structure by ID
		/// </summary>
		/// <param name="folderID">ID of the folder to retrieve</param>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="getFolderStructureCompleted">Callback method which will be executed after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFolderStructureCompleted"/> is null</exception>
		public void GetFolderStructure(
			long folderID, 
			RetrieveFolderStructureOptions retrieveOptions,
			OperationFinished<GetFolderStructureResponse> getFolderStructureCompleted)
		{
			GetFolderStructure(folderID, retrieveOptions, getFolderStructureCompleted, null);
		}

		/// <summary>
		/// Asynchronously retrieves a user's folder structure by ID
		/// </summary>
		/// <param name="folderID">ID of the folder to retrieve</param>
		/// <param name="retrieveOptions">Retrieve options</param>
		/// <param name="getFolderStructureCompleted">Callback method which will be executed after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getFolderStructureCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFolderStructureCompleted"/> is null</exception>
		public void GetFolderStructure(
			long folderID,
			RetrieveFolderStructureOptions retrieveOptions,
			OperationFinished<GetFolderStructureResponse> getFolderStructureCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(getFolderStructureCompleted, "getFolderStructureCompleted");

			object[] state = new object[3];

			state[0] = getFolderStructureCompleted;
			state[1] = retrieveOptions;
			state[2] = userState;

			_service.get_account_treeCompleted += GetFolderStructureFinished;

			_service.get_account_treeAsync(_apiKey, _token, folderID, retrieveOptions.ToStringArray(), state);
		}


		private void GetFolderStructureFinished(object sender, get_account_treeCompletedEventArgs e)
		{
			object[] state = (object[])e.UserState;
			RetrieveFolderStructureOptions retrieveOptions = (RetrieveFolderStructureOptions)state[1];
			OperationFinished<GetFolderStructureResponse> getFolderStructureCompleted = (OperationFinished<GetFolderStructureResponse>)state[0];
			GetFolderStructureResponse response;

			if (e.Error != null)
			{
				response = new GetFolderStructureResponse
							{
								Status = GetAccountTreeStatus.Failed,
								UserState = state[2],
								Error = e.Error
							};
			}
			else
			{
				response = new GetFolderStructureResponse
				           	{
				           		Status = StatusMessageParser.ParseGetAccountTreeStatus(e.Result),
				           		UserState = state[2]
				           	};

				if (response.Status == GetAccountTreeStatus.Successful)
				{
					byte[] folderInfoXml = null;
					string folderInfo = null;

					if (!retrieveOptions.Contains(RetrieveFolderStructureOptions.NoZip))
					{
						folderInfoXml = Unzip(e.tree);
					}

					if (folderInfoXml != null)
					{
						folderInfo = Encoding.ASCII.GetString(folderInfoXml);
					}

					Folder folder = ParseFolderStructureXmlMessage(folderInfo);

					response.Folder = folder;
				}
				else if (response.Status == GetAccountTreeStatus.Unknown)
				{
					response.Error = new UnknownOperationStatusException(e.Result);
				}

			}

			getFolderStructureCompleted(response);
		}
		
		#endregion

		#region ExportTags
		
		/// <summary>
		/// Retrieves list of user's tags
		/// </summary>
		/// <param name="tagList">List of user's tags</param>
		/// <returns>Operation status</returns>
		public ExportTagsStatus ExportTags(out TagPrimitiveCollection tagList)
		{
			byte[] xmlMessage;

			string result = _service.export_tags(_apiKey, _token, out xmlMessage);
			ExportTagsStatus status = StatusMessageParser.ParseExportTagStatus(result);

			tagList = MessageParser.Instance.ParseExportTagsMessage(Encoding.ASCII.GetString(xmlMessage));

			return status;
		}

		/// <summary>
		/// Asynchronously retrieves list of user's tags
		/// </summary>
		/// <param name="exportTagsCompleted">Callback method which will be invioked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="exportTagsCompleted"/> is null</exception>
		public void ExportTags(OperationFinished<ExportTagsResponse> exportTagsCompleted)
		{
			ExportTags(exportTagsCompleted, null);
		}

		/// <summary>
		/// Asynchronously retrieves list of user's tags
		/// </summary>
		/// <param name="exportTagsCompleted">Callback method which will be invioked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="exportTagsCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="exportTagsCompleted"/> is null</exception>
		public void ExportTags(
			OperationFinished<ExportTagsResponse> exportTagsCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(exportTagsCompleted, "exportTagsCompleted");

			_service.export_tagsCompleted += ExportTagsFinished;

			object[] state = { exportTagsCompleted, userState };

			_service.export_tagsAsync(_apiKey, _token, state);
		}


		private void ExportTagsFinished(object sender, export_tagsCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<ExportTagsResponse> exportTagsFinishedHandler =
				(OperationFinished<ExportTagsResponse>) state[0];
			ExportTagsResponse response = new ExportTagsResponse
			                              	{
			                              		UserState = state[1]
			                              	};

			if (e.Error != null)
			{
				response.Error = e.Error;
			}
			else
			{
				response.Status = StatusMessageParser.ParseExportTagStatus(e.Result);

				switch (response.Status)
				{
					case ExportTagsStatus.Successful:
						response.TagsList = MessageParser.Instance.ParseExportTagsMessage(Encoding.ASCII.GetString(e.tag_xml));
						break;
					case ExportTagsStatus.Unknown:
						response.TagsList = new TagPrimitiveCollection();
						response.Error = new UnknownOperationStatusException(e.Result);
						break;
					default:
						response.TagsList = new TagPrimitiveCollection();
						break;
				}
			}

			exportTagsFinishedHandler(response);
		}

		#endregion

		#region GetTag

		private TagPrimitive GetTag(long id)
		{
			ManualResetEvent wait = new ManualResetEvent(false);
			TagPrimitive result = new TagPrimitive();

			OperationFinished<ExportTagsStatus, TagPrimitive> getTagFinishedHandler = (status, tag) =>
			                                                                          	{
			                                                                          		result = tag;
			                                                                          		wait.Reset();
			                                                                          	};
			GetTag(id, getTagFinishedHandler);
			wait.WaitOne();

			return result;
		}

		private void GetTag(long id, OperationFinished<ExportTagsStatus, TagPrimitive> getTagFinishedHandler)
		{
			if (_tagCollection == null || _tagCollection.IsEmpty)
			{
				OperationFinished<ExportTagsResponse> exportTagsFinishedHandler =
					(response) =>
						{
							_tagCollection = response.TagsList;

							getTagFinishedHandler(response.Status, response.TagsList.GetTag(id));
						};

				ExportTags(exportTagsFinishedHandler);
			}
			else
			{
				getTagFinishedHandler(ExportTagsStatus.Successful, _tagCollection.GetTag(id));
			}
		}

		#endregion

		#region SetDescription

		/// <summary>
		/// Sets description of the object
		/// </summary>
		/// <param name="objectID">ID of the object</param>
		/// <param name="objectType">Object type</param>
		/// <param name="description">Description text</param>
		/// <returns>Operation status</returns>
		public SetDescriptionStatus SetDescription(
			long objectID, 
			ObjectType objectType, 
			string description)
		{
			string type = ObjectType2String(objectType);

			string result = _service.set_description(_apiKey, _token, type, objectID, description);
			
			return StatusMessageParser.ParseSetDescriptionStatus(result);
		}

		/// <summary>
		/// Asynchronously sets description of the object
		/// </summary>
		/// <param name="objectID">ID of the object</param>
		/// <param name="objectType">Object type</param>
		/// <param name="description">Description text</param>
		/// <param name="setDescriptionCompleted">Callback method which will be invoked after delete operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="setDescriptionCompleted"/> is null</exception>
		public void SetDescription(
			long objectID,
			ObjectType objectType,
			string description,
			OperationFinished<SetDescriptionResponse> setDescriptionCompleted)
		{
			SetDescription(objectID, objectType, description, setDescriptionCompleted, null);
		}

		/// <summary>
		/// Asynchronously sets description of the object
		/// </summary>
		/// <param name="objectID">ID of the object</param>
		/// <param name="objectType">Object type</param>
		/// <param name="description">Description text</param>
		/// <param name="setDescriptionCompleted">Callback method which will be invoked after delete operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="setDescriptionCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="setDescriptionCompleted"/> is null</exception>
		public void SetDescription(
			long objectID,
			ObjectType objectType,
			string description,
			OperationFinished<SetDescriptionResponse> setDescriptionCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(setDescriptionCompleted, "setDescriptionCompleted");

			string type = ObjectType2String(objectType);

			_service.set_descriptionCompleted += SetDescriptionFinished;

			object[] state = { setDescriptionCompleted, userState };

			_service.set_descriptionAsync(_apiKey, _token, type, objectID, description, state);
		}


		private void SetDescriptionFinished(object sender, set_descriptionCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<SetDescriptionResponse> setDescriptionFinishedHandler =
				(OperationFinished<SetDescriptionResponse>)state[0];
			SetDescriptionResponse response;
			
			if (e.Error != null)
			{
				response = new SetDescriptionResponse
							{
								Status = SetDescriptionStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new SetDescriptionResponse
				           	{
				           		Status = StatusMessageParser.ParseSetDescriptionStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == SetDescriptionStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			setDescriptionFinishedHandler(response);
		}

		#endregion

		#region Rename

		/// <summary>
		/// Renames specified object
		/// </summary>
		/// <param name="objectID">ID of the object which needs to be renamed</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="newName">New name of the object</param>
		/// <returns>Operation status</returns>
		public RenameObjectStatus RenameObject(
			long objectID, 
			ObjectType objectType, 
			string newName)
		{
			string type = ObjectType2String(objectType);
			string result = _service.rename(_apiKey, _token, type, objectID, newName);

			return StatusMessageParser.ParseRenameObjectStatus(result);
		}
		
		/// <summary>
		/// Asynchronously renames specified object
		/// </summary>
		/// <param name="objectID">ID of the object which needs to be renamed</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="newName">New name of the object</param>
		/// <param name="renameObjectCompleted">Callback method which will be invoked after rename operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="renameObjectCompleted"/> is null</exception>
		public void RenameObject(
			long objectID, 
			ObjectType objectType, 
			string newName,
			OperationFinished<RenameObjectResponse> renameObjectCompleted)
		{
			RenameObject(objectID, objectType, newName, renameObjectCompleted, null);
		}

		/// <summary>
		/// Asynchronously renames specified object
		/// </summary>
		/// <param name="objectID">ID of the object which needs to be renamed</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="newName">New name of the object</param>
		/// <param name="renameObjectCompleted">Callback method which will be invoked after rename operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="renameObjectCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="renameObjectCompleted"/> is null</exception>
		public void RenameObject(
			long objectID,
			ObjectType objectType,
			string newName,
			OperationFinished<RenameObjectResponse> renameObjectCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(renameObjectCompleted, "renameObjectCompleted");

			string type = ObjectType2String(objectType);

			_service.renameCompleted += RenameObjectCompleted;

			object[] state = {renameObjectCompleted, userState};

			_service.renameAsync(_apiKey, _token, type, objectID, newName, state);
		}


		/// <summary>
		/// Handler method which will be executed after rename operation completes
		/// </summary>
		/// <param name="sender">The source of the event</param>
		/// <param name="e">Argument that contains event data</param>
		private void RenameObjectCompleted(object sender, renameCompletedEventArgs e)
		{
			object[] state = (object[])e.UserState;
			OperationFinished<RenameObjectResponse> renameObjectFinishedHandler = (OperationFinished<RenameObjectResponse>)state[0];
			RenameObjectResponse response;
			
			if (e.Error != null)
			{
				response = new RenameObjectResponse
				           	{
								Status = RenameObjectStatus.Failed,
				           		UserState = state[1],
								Error = e.Error
				           	};
			}
			else
			{
				response = new RenameObjectResponse
							{
								Status = StatusMessageParser.ParseRenameObjectStatus(e.Result),
								UserState = state[1]
							};

				response.Error = response.Status == RenameObjectStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			renameObjectFinishedHandler(response);
		}
		
		#endregion

		#region Move
		
		/// <summary>
		/// Moves object from one folder to another one
		/// </summary>
		/// <param name="targetObjectID">ID of the object which needs to be moved</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <returns>Operation status</returns>
		public MoveObjectStatus MoveObject(
			long targetObjectID, 
			ObjectType targetObjectType, 
			long destinationFolderID)
		{
			string type = ObjectType2String(targetObjectType);
			string result = _service.move(_apiKey, _token, type, targetObjectID, destinationFolderID);

			return StatusMessageParser.ParseMoveObjectStatus(result);
		}
		
		/// <summary>
		/// Asynchronously moves object from one folder to another one
		/// </summary>
		/// <param name="targetObjectID">ID of the object which needs to be moved</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="moveObjectCompleted">Callback method which will be invoked after move operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="moveObjectCompleted"/> is null</exception>
		public void MoveObject(
			long targetObjectID, 
			ObjectType targetObjectType, 
			long destinationFolderID,
			OperationFinished<MoveObjectResponse> moveObjectCompleted)
		{
			MoveObject(targetObjectID, targetObjectType, destinationFolderID, moveObjectCompleted, null);
		}

		/// <summary>
		/// Asynchronously moves object from one folder to another one
		/// </summary>
		/// <param name="targetObjectID">ID of the object which needs to be moved</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="moveObjectCompleted">Callback method which will be invoked after move operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="moveObjectCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="moveObjectCompleted"/> is null</exception>
		public void MoveObject(
			long targetObjectID,
			ObjectType targetObjectType,
			long destinationFolderID,
			OperationFinished<MoveObjectResponse> moveObjectCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(moveObjectCompleted, "moveObjectCompleted");

			string type = ObjectType2String(targetObjectType);

			_service.moveCompleted += MoveObjectFinished;

			object[] state = {moveObjectCompleted, userState};

			_service.moveAsync(_apiKey, _token, type, targetObjectID, destinationFolderID, state);
		}


		private void MoveObjectFinished(object sender, moveCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<MoveObjectResponse> moveObjectFinishedHandler = (OperationFinished<MoveObjectResponse>) state[0];
			MoveObjectResponse response;

			if (e.Error != null)
			{
				response = new MoveObjectResponse
				           	{
				           		Status = MoveObjectStatus.Failed,
				           		UserState = state[1],
				           		Error = e.Error
				           	};
			}
			else
			{
				response = new MoveObjectResponse
				           	{
				           		Status = StatusMessageParser.ParseMoveObjectStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == MoveObjectStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			moveObjectFinishedHandler(response);
		}

		#endregion

		#region Logout

		/// <summary>
		/// Logouts current user
		/// </summary>
		/// <returns>Operation status</returns>
		public LogoutStatus Logout()
		{
			string result = _service.logout(_apiKey, _token);

			return StatusMessageParser.ParseLogoutStatus(result);
		}

		/// <summary>
		/// Asynchronously logouts current user
		/// </summary>
		/// <param name="logoutCompleted">Callback method which will be invoked after logout operation completes</param>
		public void Logout(OperationFinished<LogoutResponse> logoutCompleted)
		{
			Logout(logoutCompleted, null);
		}

		/// <summary>
		/// Asynchronously logouts current user
		/// </summary>
		/// <param name="logoutCompleted">Callback method which will be invoked after logout operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="logoutCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="logoutCompleted"/> is null</exception>
		public void Logout(
			OperationFinished<LogoutResponse> logoutCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(logoutCompleted, "logoutCompleted");

			_service.logoutCompleted += LogoutFinished;

			object[] state = {logoutCompleted, userState};

			_service.logoutAsync(_apiKey, _token, state);
		}

		private void LogoutFinished(object sender, logoutCompletedEventArgs e)
		{
			object[] state = (object[])e.UserState;
			OperationFinished<LogoutResponse> logoutFinishedHandler = (OperationFinished<LogoutResponse>)state[0];
			LogoutResponse response;
			
			if (e.Error != null)
			{
				response = new LogoutResponse
							{
								Status = LogoutStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new LogoutResponse
				           	{
				           		Status = StatusMessageParser.ParseLogoutStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == LogoutStatus.Unknown
				        	?
				        		new UnknownOperationStatusException(e.Result)
				        	:
				        		null;
			}

			logoutFinishedHandler(response);
		}
		#endregion

		#region RegisterNewUser

		/// <summary>
		/// Registers new Box.NET user
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <param name="password">Account password</param>
		/// <param name="response">Contains information about user account and valid authorization token</param>
		/// <returns>Operation status</returns>
		public RegisterNewUserStatus RegisterNewUser(string login, string password, out RegisterNewUserResponse response)
		{
			string token;
			SOAPUser user;

			string result = _service.register_new_user(_apiKey, login, password, out token, out user);

			response = new RegisterNewUserResponse {Token = token, User = user};

			return StatusMessageParser.ParseRegisterNewUserStatus(result);
		}
		
		/// <summary>
		/// Asynchronously registers new Box.NET user
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <param name="password">Account password</param>
		/// <param name="registerNewUserCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="registerNewUserCompleted"/> is null</exception>
		public void RegisterNewUser(
			string login, 
			string password, 
			OperationFinished<RegisterNewUserResponse> registerNewUserCompleted)
		{
			RegisterNewUser(login, password, registerNewUserCompleted, null);
		}

		/// <summary>
		/// Asynchronously registers new Box.NET user
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <param name="password">Account password</param>
		/// <param name="registerNewUserCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="registerNewUserCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="registerNewUserCompleted"/> is null</exception>
		public void RegisterNewUser(
			string login,
			string password,
			OperationFinished<RegisterNewUserResponse> registerNewUserCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(registerNewUserCompleted, "registerNewUserCompleted");

			_service.register_new_userCompleted += RegisterNewUserFinished;

			object[] state = {registerNewUserCompleted, userState};

			_service.register_new_userAsync(_apiKey, login, password, state);
		}


		private void RegisterNewUserFinished(object sender, register_new_userCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<RegisterNewUserResponse> registerNewUserFinishedHandler =
				(OperationFinished<RegisterNewUserResponse>) state[0];
			RegisterNewUserResponse response;

			if (e.Error != null)
			{
				response = new RegisterNewUserResponse
							{
								Status = RegisterNewUserStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new RegisterNewUserResponse
				           	{
				           		Token = e.auth_token,
				           		User = e.user,
				           		Status = StatusMessageParser.ParseRegisterNewUserStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == RegisterNewUserStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			registerNewUserFinishedHandler(response);
		}

		#endregion

		#region VerifyRegistrationEmail

		/// <summary>
		/// Verifies registration email address
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <returns>Operation status</returns>
		public VerifyRegistrationEmailStatus VerifyRegistrationEmail(string login)
		{
			string result = _service.verify_registration_email(_apiKey, login);

			return StatusMessageParser.ParseVerifyRegistrationEmailStatus(result);
		}


		/// <summary>
		/// Asynchronously verifies registration email address
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <param name="verifyRegistrationEmailCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="verifyRegistrationEmailCompleted"/> is null</exception>
		public void VerifyRegistrationEmail(
			string login,
			OperationFinished<VerifyRegistrationEmailResponse> verifyRegistrationEmailCompleted)
		{
			VerifyRegistrationEmail(login, verifyRegistrationEmailCompleted, null);
		}

		/// <summary>
		/// Asynchronously verifies registration email address
		/// </summary>
		/// <param name="login">Account login name</param>
		/// <param name="verifyRegistrationEmailCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="verifyRegistrationEmailCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="verifyRegistrationEmailCompleted"/> is null</exception>
		public void VerifyRegistrationEmail(
			string login,
			OperationFinished<VerifyRegistrationEmailResponse> verifyRegistrationEmailCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(verifyRegistrationEmailCompleted, "verifyRegistrationEmailCompleted");

			_service.verify_registration_emailCompleted += VerifyRegistrationEmailFinished;

			object[] state = {verifyRegistrationEmailCompleted, userState};

			_service.verify_registration_emailAsync(_apiKey, login, state);
		}


		private void VerifyRegistrationEmailFinished(object sender, verify_registration_emailCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<VerifyRegistrationEmailResponse> verifyRegistrationEmailFinishedHandler =
				(OperationFinished<VerifyRegistrationEmailResponse>) state[0];
			VerifyRegistrationEmailResponse response;

			if (e.Error != null)
			{
				response = new VerifyRegistrationEmailResponse
							{
								Status = VerifyRegistrationEmailStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new VerifyRegistrationEmailResponse
				           	{
				           		Status =
				           			StatusMessageParser.ParseVerifyRegistrationEmailStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == VerifyRegistrationEmailStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}


			verifyRegistrationEmailFinishedHandler(response);
		}

		#endregion

		#region AddToMyBox

		/// <summary>
		/// Copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileID">ID of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <returns>Operation status</returns>
		public AddToMyBoxStatus AddToMyBox(
			long targetFileID, 
			long destinationFolderID, 
			TagPrimitiveCollection tagList)
		{
			string result = _service.add_to_mybox(_apiKey, _token, targetFileID, null, destinationFolderID,
			                      ConvertTagPrimitiveCollection2String(tagList));

			return StatusMessageParser.ParseAddToMyBoxStatus(result);
		}

		/// <summary>
		/// Copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileName">Name of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <returns>Operation status</returns>
		public AddToMyBoxStatus AddToMyBox(
			string targetFileName, 
			long destinationFolderID, 
			TagPrimitiveCollection tagList)
		{
			string result = _service.add_to_mybox(_apiKey, _token, 0, targetFileName, destinationFolderID,
								  ConvertTagPrimitiveCollection2String(tagList));

			return StatusMessageParser.ParseAddToMyBoxStatus(result);
		}


		/// <summary>
		/// Asuncronously copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileID">ID of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <param name="addToMyBoxCompleted">Delegate which will be executed after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addToMyBoxCompleted"/> is null</exception>
		public void AddToMyBox(
			long targetFileID, 
			long destinationFolderID, 
			TagPrimitiveCollection tagList,
			OperationFinished<AddToMyBoxResponse> addToMyBoxCompleted)
		{
			AddToMyBox(targetFileID, destinationFolderID, tagList, addToMyBoxCompleted, null);
		}

		/// <summary>
		/// Asuncronously copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileID">ID of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <param name="addToMyBoxCompleted">Delegate which will be executed after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="addToMyBoxCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addToMyBoxCompleted"/> is null</exception>
		public void AddToMyBox(
			long targetFileID,
			long destinationFolderID,
			TagPrimitiveCollection tagList,
			OperationFinished<AddToMyBoxResponse> addToMyBoxCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(addToMyBoxCompleted, "addToMyBoxCompleted");

			_service.add_to_myboxCompleted += AddToMyBoxFinished;

			object[] state = { addToMyBoxCompleted , userState};

			_service.add_to_myboxAsync(_apiKey, _token, targetFileID, null, destinationFolderID, ConvertTagPrimitiveCollection2String(tagList), state);
		}


		/// <summary>
		/// Asyncronously copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileName">Name of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <param name="addToMyBoxCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addToMyBoxCompleted"/> is null</exception>
		public void AddToMyBox(
			string targetFileName, 
			long destinationFolderID, 
			TagPrimitiveCollection tagList,
			OperationFinished<AddToMyBoxResponse> addToMyBoxCompleted)
		{
			AddToMyBox(targetFileName, destinationFolderID, tagList, addToMyBoxCompleted, null);
		}

		/// <summary>
		/// Asyncronously copies a file publicly shared by someone to a user's folder
		/// </summary>
		/// <param name="targetFileName">Name of the file which needs to be copied</param>
		/// <param name="destinationFolderID">ID of the destination folder</param>
		/// <param name="tagList">Tags which need to be assigned to the target file</param>
		/// <param name="addToMyBoxCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="addToMyBoxCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addToMyBoxCompleted"/> is null</exception>
		public void AddToMyBox(
			string targetFileName,
			long destinationFolderID,
			TagPrimitiveCollection tagList,
			OperationFinished<AddToMyBoxResponse> addToMyBoxCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(addToMyBoxCompleted, "addToMyBoxCompleted");

			_service.add_to_myboxCompleted += AddToMyBoxFinished;

			object[] state = { addToMyBoxCompleted , userState};

			_service.add_to_myboxAsync(_apiKey, _token, 0, targetFileName, destinationFolderID, ConvertTagPrimitiveCollection2String(tagList), state);
		}


		private void AddToMyBoxFinished(object sender, add_to_myboxCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<AddToMyBoxResponse> addToMyBoxCompleted = (OperationFinished<AddToMyBoxResponse>)state[0];
			AddToMyBoxResponse response;

			if (e.Error != null)
			{
				response = new AddToMyBoxResponse
							{
								Status = AddToMyBoxStatus.Failed,
								UserState = state,
								Error = e.Error
							};
			}
			else
			{
				response = new AddToMyBoxResponse
				           	{
				           		Status = StatusMessageParser.ParseAddToMyBoxStatus(e.Result),
				           		UserState = state
				           	};

				response.Error = response.Status == AddToMyBoxStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			addToMyBoxCompleted(response);
		}

		#endregion

		#region PublicShare

		/// <summary>
		/// Publicly shares a file or folder
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="notificationMessage">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <returns>Operation status</returns>
		public PublicShareResponse PublicShare(
			long targetObjectID,
			ObjectType targetObjectType,
			string password,
			string notificationMessage,
			string[] emailList)
		{
			string publicName;
			string type = ObjectType2String(targetObjectType);
			string result = _service.public_share(_apiKey,
			                                      _token,
			                                      type,
			                                      targetObjectID,
			                                      password ?? string.Empty,
			                                      notificationMessage ?? string.Empty,
			                                      emailList ?? new string[0],
			                                      out publicName);

			return new PublicShareResponse
			       	{
			       		PublicName = publicName,
			       		Status = StatusMessageParser.ParsePublicShareStatus(result)
			       	};
		}

		/// <summary>
		/// Publicly shares a file or folder
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="notificationMessage">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="publicName">Unique identifier of a publicly shared object</param>
		/// <returns>Operation status</returns>
		[Obsolete("Use PublicShare(long, ObjectType, string, string, string[]):PublicShareResponse")]
		public PublicShareStatus PublicShare(
			long targetObjectID, 
			ObjectType targetObjectType, 
			string password, 
			string notificationMessage, 
			string[] emailList, 
			out string publicName)
		{
			string type = ObjectType2String(targetObjectType);
			string result = _service.public_share(_apiKey, _token, type, targetObjectID, password, notificationMessage, emailList, out publicName);

			return StatusMessageParser.ParsePublicShareStatus(result);
		}

		/// <summary>
		/// Asynchronously publicly shares a file or folder
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="message">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="sendNotification">Indicates if the notification about object sharing must be send</param>
		/// <param name="publicShareCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="publicShareCompleted"/> is null</exception>
		public void PublicShare(
			long targetObjectID, 
			ObjectType targetObjectType, 
			string password, 
			string message, 
			string[] emailList, 
			bool sendNotification,
			OperationFinished<PublicShareResponse> publicShareCompleted)
		{
			PublicShare(
				targetObjectID,
				targetObjectType,
				password,
				message,
				emailList,
				sendNotification,
				publicShareCompleted,
				null);
		}

		/// <summary>
		/// Asynchronously publicly shares a file or folder
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="message">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="sendNotification">Indicates if the notification about object sharing must be send</param>
		/// <param name="publicShareCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="publicShareCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="publicShareCompleted"/> is null</exception>
		public void PublicShare(
			long targetObjectID,
			ObjectType targetObjectType,
			string password,
			string message,
			string[] emailList,
			bool sendNotification,
			OperationFinished<PublicShareResponse> publicShareCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(publicShareCompleted, "publicShareCompleted");

			string type = ObjectType2String(targetObjectType);

			_service.public_shareCompleted += PublicShareFinished;

			object[] state = {publicShareCompleted, userState};

			_service.private_shareAsync(
				_apiKey,
				_token, 
				type, 
				targetObjectID, 
				emailList, 
				message, 
				sendNotification, 
				state);
		}


		private void PublicShareFinished(object sender, public_shareCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<PublicShareResponse> publicShareCompleted = (OperationFinished<PublicShareResponse>) state[0];
			PublicShareResponse response;

			if (e.Error != null)
			{
				response = new PublicShareResponse
							{
								Status = PublicShareStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new PublicShareResponse
				           	{
				           		PublicName = e.public_name,
				           		Status = StatusMessageParser.ParsePublicShareStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == PublicShareStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			publicShareCompleted(response);
		}

		#endregion

		#region PublicUnshare

		/// <summary>
		/// Unshares a shared object
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be unshared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <returns>Operation status</returns>
		public PublicUnshareStatus PublicUnshare(long targetObjectID, ObjectType targetObjectType)
		{
			string type = ObjectType2String(targetObjectType);
			string result = _service.public_unshare(_apiKey, _token, type, targetObjectID);

			return StatusMessageParser.ParsePublicUnshareStatus(result);
		}

		/// <summary>
		/// Asynchronously unshares a shared object
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be unshared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="publicUnshareCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="publicUnshareCompleted"/> is <c>null</c></exception>
		public void PublicUnshare(
			long targetObjectID, 
			ObjectType targetObjectType,
			OperationFinished<PublicUnshareResponse> publicUnshareCompleted)
		{
			PublicUnshare(targetObjectID, targetObjectType, publicUnshareCompleted, null);
		}

		/// <summary>
		/// Asynchronously unshares a shared object
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be unshared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="publicUnshareCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="publicUnshareCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="publicUnshareCompleted"/> is <c>null</c></exception>
		public void PublicUnshare(
			long targetObjectID,
			ObjectType targetObjectType,
			OperationFinished<PublicUnshareResponse> publicUnshareCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(publicUnshareCompleted, "publicUnshareCompleted");

			string type = ObjectType2String(targetObjectType);

			_service.public_unshareCompleted += PublicUnshareCompleted;
			object[] state = {publicUnshareCompleted, userState};

			_service.public_unshareAsync(_apiKey, _token, type, targetObjectID, state);
		}


		private void PublicUnshareCompleted(object sender, public_unshareCompletedEventArgs e)
		{
			object[] state = (object[]) e.UserState;
			OperationFinished<PublicUnshareResponse> publicUnshareCompleted = (OperationFinished<PublicUnshareResponse>)state[0];
			PublicUnshareResponse response;

			if (e.Error != null)
			{
				response = new PublicUnshareResponse
							{
								Status = PublicUnshareStatus.Failed,
								UserState = state[1],
								Error = e.Error
							};
			}
			else
			{
				response = new PublicUnshareResponse
				           	{
				           		Status = StatusMessageParser.ParsePublicUnshareStatus(e.Result),
				           		UserState = state[1]
				           	};

				response.Error = response.Status == PublicUnshareStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}


			publicUnshareCompleted(response);
		}

		#endregion

		#region PrivateShare

		/// <summary>
		/// Privately shares an object with another user(s)
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="notificationMessage">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="sendNotification">Indicates if the notification about object sharing must be send</param>
		/// <returns>Operation status</returns>
		public PrivateShareStatus PrivateShare(
			long targetObjectID, 
			ObjectType targetObjectType, 
			string password, 
			string notificationMessage, 
			string[] emailList, 
			bool sendNotification)
		{
			string type = ObjectType2String(targetObjectType);
			string result = _service.private_share(_apiKey, _token, type, targetObjectID, emailList, notificationMessage, sendNotification);

			return StatusMessageParser.ParsePrivateShareStatus(result);
		}

		/// <summary>
		/// Asynchronously privately shares an object with another user(s)
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="notificationMessage">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="sendNotification">Indicates if the notification about object sharing must be send</param>
		/// <param name="privateShareCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="privateShareCompleted"/> is <c>null</c></exception>
		public void PrivateShare(
			long targetObjectID, 
			ObjectType targetObjectType, 
			string password, 
			string notificationMessage, 
			string[] emailList, 
			bool sendNotification, 
			OperationFinished<PrivateShareResponse> privateShareCompleted)
		{
			PrivateShare(targetObjectID, targetObjectType, password, notificationMessage, emailList, sendNotification,
			             privateShareCompleted, null);
		}

		/// <summary>
		/// Asynchronously privately shares an object with another user(s)
		/// </summary>
		/// <param name="targetObjectID">ID of the object to be shared</param>
		/// <param name="targetObjectType">Type of the object</param>
		/// <param name="password">Password to protect shared object or Null</param>
		/// <param name="notificationMessage">Message to be included in a notification email</param>
		/// <param name="emailList">Array of emails for which to notify users about a newly shared file or folder</param>
		/// <param name="sendNotification">Indicates if the notification about object sharing must be send</param>
		/// <param name="privateShareCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="privateShareCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="privateShareCompleted"/> is <c>null</c></exception>
		public void PrivateShare(
			long targetObjectID, 
			ObjectType targetObjectType, 
			string password, 
			string notificationMessage, 
			string[] emailList, 
			bool sendNotification, 
			OperationFinished<PrivateShareResponse> privateShareCompleted, 
			object userState)
		{
			ThrowIfParameterIsNull(privateShareCompleted, "privateShareCompleted");
			
			string type = ObjectType2String(targetObjectType);

			_service.private_shareCompleted += PrivateShareFinished;

			object[] data = {userState, privateShareCompleted};

			_service.private_shareAsync(_apiKey, _token, type, targetObjectID, emailList, notificationMessage, sendNotification, data);
		}


		private void PrivateShareFinished(object sender, private_shareCompletedEventArgs e)
		{
			object[] userState = (object[]) e.UserState;
			OperationFinished<PrivateShareResponse> privateShareCompleted =
				(OperationFinished<PrivateShareResponse>) userState[1];
			PrivateShareResponse response;

			if (e.Error != null)
			{
				response = new PrivateShareResponse
				           	{
				           		Status = PrivateShareStatus.Failed,
				           		UserState = userState[0],
				           		Error = e.Error
				           	};
			}
			else
			{
				response = new PrivateShareResponse
				           	{
				           		Status = StatusMessageParser.ParsePrivateShareStatus(e.Result),
				           		UserState = userState[0]
				           	};

				response.Error = response.Status == PrivateShareStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			privateShareCompleted(response);
		}

		#endregion

		#region GetAccountInfo
		/// <summary>
		/// Gets information about current logged in user
		/// </summary>
		/// <returns>Operation response</returns>
		public GetAccountInfoResponse GetAccountInfo()
		{
			GetAccountInfoResponse response = new GetAccountInfoResponse();
			SOAPUser user;

			string result = _service.get_account_info(_apiKey, _token, out user);

			response.Status = StatusMessageParser.ParseGetAccountInfoStatus(result);

			if (response.Status == GetAccountInfoStatus.Successful)
			{
				response.User = new User(user);
			}

			return response;
		}

        /// <summary>
		/// Gets information about current logged in user
        /// </summary>
		/// <param name="getAccountInfoCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getAccountInfoCompleted"/> is <c>null</c></exception>
		public void GetAccountInfo(OperationFinished<GetAccountInfoResponse> getAccountInfoCompleted)
		{
			GetAccountInfo(getAccountInfoCompleted, null);
		}

		/// <summary>
		/// Gets information about current logged in user
		/// </summary>
		/// <param name="getAccountInfoCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getAccountInfoCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getAccountInfoCompleted"/> is <c>null</c></exception>
		public void GetAccountInfo(OperationFinished<GetAccountInfoResponse> getAccountInfoCompleted, object userState)
		{
			ThrowIfParameterIsNull(getAccountInfoCompleted, "getAccountInfoCompleted");

			_service.get_account_infoCompleted += GetAccountInfoCompleted;

			object[] data = { userState, getAccountInfoCompleted };

			_service.get_account_infoAsync(_apiKey, _token, data);
		}

		private void GetAccountInfoCompleted(object sender, get_account_infoCompletedEventArgs e)
		{
			object[] userState = (object[])e.UserState;
			OperationFinished<GetAccountInfoResponse> getAccountInfoCompleted =
				(OperationFinished<GetAccountInfoResponse>) userState[1];
			GetAccountInfoResponse response;

			if (e.Error != null)
			{
				response = new GetAccountInfoResponse
							{
								Status = GetAccountInfoStatus.Failed,
								UserState = userState[0],
								Error = e.Error
							};
			}
			else
			{
				GetAccountInfoStatus status = StatusMessageParser.ParseGetAccountInfoStatus(e.Result);
				User user = status == GetAccountInfoStatus.Successful ? new User(e.user) : null;

				response = new GetAccountInfoResponse
				           	{
				           		Status = status,
				           		UserState = userState[0],
				           		User = user
				           	};

				response.Error = response.Status == GetAccountInfoStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			getAccountInfoCompleted(response);
		}
		#endregion

		#region GetComments
		
		/// <summary>
		/// Adds comment to the object
		/// </summary>
		/// <param name="objectID">ID of the object which should be commented</param>
		/// <param name="objectType">Object type</param>
		/// <returns>Response which contains operation status and related data</returns>
		public GetCommentsResponse GetComments(long objectID, ObjectType objectType)
		{
			SOAPComment[] comments;

			string status = _service.get_comments(_apiKey, _token, objectID, ObjectType2String(objectType), out comments);

			return null;
		}

		/// <summary>
		/// Adds comment to an object
		/// </summary>
		/// <param name="objectID">ID of the object which should be commented</param>
		/// <param name="objectType">Object type</param>
		/// <param name="getCommentsCompleted">Callback method which will be invoked after operation completes</param>
		public void GetComments(long objectID, ObjectType objectType, OperationFinished<GetCommentsResponse> getCommentsCompleted)
		{
			GetComments(objectID, objectType, getCommentsCompleted, null);
		}

		/// <summary>
		/// Adds comment to an object
		/// </summary>
		/// <param name="objectID">ID of the object which should be commented</param>
		/// <param name="objectType">Object type</param>
		/// <param name="getCommentsCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getCommentsCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getCommentsCompleted"/> is <c>null</c></exception>
		public void GetComments(long objectID,
								ObjectType objectType,
								OperationFinished<GetCommentsResponse> getCommentsCompleted,
								object userState)
		{
			ThrowIfParameterIsNull(getCommentsCompleted, "getCommentsCompleted");

			AsyncCallState<OperationFinished<GetCommentsResponse>> state = new AsyncCallState
				<OperationFinished<GetCommentsResponse>>
			                                                               	{
			                                                               		CallbackDelegate = getCommentsCompleted,
			                                                               		UserState = userState
			                                                               	};

			_service.get_commentsCompleted += GetCommentsFinished;

			_service.get_commentsAsync(_apiKey, _token, objectID, ObjectType2String(objectType), state);
		}

		private void GetCommentsFinished(object sender, get_commentsCompletedEventArgs e)
		{
			AsyncCallState<OperationFinished<GetCommentsResponse>> state =
				(AsyncCallState<OperationFinished<GetCommentsResponse>>) e.UserState;

			GetCommentsResponse response;

			if (e.Error != null)
			{
				response = new GetCommentsResponse
				           	{
				           		Error = e.Error,
				           		Status = GetCommentsStatus.Failed,
				           		UserState = state.UserState
				           	};
			}
			else
			{
                response = new GetCommentsResponse
				           	{
                                //Status = StatusMessageParser.ParseGetFileInfoStatus(e.Result),
                                //File = new File
                                //        {
                                //            Created = UnixTimeConverter.Instance.FromUnixTime(e.info.created),
                                //            Description = e.info.description,
                                //            ID = e.info.file_id,
                                //            IsShared = e.info.shared == 1,
                                //            Name = e.info.file_name,
                                //            PublicName = e.info.public_name,
                                //            SHA1Hash = e.info.sha1,
                                //            Size = e.info.size,
                                //            Updated = UnixTimeConverter.Instance.FromUnixTime(e.info.updated)
                                //        },
				           		UserState = state.UserState
				           	};

				response.Error = response.Status == GetCommentsStatus.Unknown
									?
										new UnknownOperationStatusException(e.Result)
									:
										null;
			}

			state.CallbackDelegate(response);
		}


		#endregion

		#region AddComment
		/// <summary>
		/// Adds user comment to an item
		/// </summary>
		/// <param name="objectID">ID of the object to comment</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="commentText">The comment's message to be added</param>
		/// <returns>Operation response</returns>
		public AddCommentResponse AddComment(long objectID, ObjectType objectType, string commentText)
		{
			ThrowIfParameterIsNull(commentText, "commentText");

			SOAPComment comment;

			string status = _service.add_comment(_apiKey, _token, objectID, ObjectType2String(objectType), commentText, out comment);
			AddCommentStatus parsedStatus = StatusMessageParser.ParseAddCommentStatus(status);

			return new AddCommentResponse
			       	{
			       		PostedComment = parsedStatus == AddCommentStatus.Successful
			       		                	?
			       		                		new Comment(comment)
			       		                	:
			       		                		null,
			       		Status = parsedStatus
			       	};
		}

		/// <summary>
		/// Asynchronously adds user comment to an item
		/// </summary>
		/// <param name="objectID">ID of the object to comment</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="commentText">The comment's message to be added</param>
		/// <param name="addCommentCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addCommentCompleted"/> is <c>null</c></exception>
		public void AddComment(
			long objectID,
			ObjectType objectType,
			string commentText,
			OperationFinished<AddCommentResponse> addCommentCompleted)
		{
			AddComment(objectID, objectType, commentText, addCommentCompleted, null);
		}

		/// <summary>
		/// Asynchronously adds user comment to an item
		/// </summary>
		/// <param name="objectID">ID of the object to comment</param>
		/// <param name="objectType">Type of the object</param>
		/// <param name="commentText">The comment's message to be added</param>
		/// <param name="addCommentCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="addCommentCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="addCommentCompleted"/> is <c>null</c></exception>
		public void AddComment(
			long objectID, 
			ObjectType objectType, 
			string commentText, 
			OperationFinished<AddCommentResponse> addCommentCompleted,
			object userState)
		{
			ThrowIfParameterIsNull(addCommentCompleted, "addCommentCompleted");

			AsyncCallState<OperationFinished<AddCommentResponse>> state = new AsyncCallState
				<OperationFinished<AddCommentResponse>>
			                                                              	{
			                                                              		CallbackDelegate = addCommentCompleted,
			                                                              		UserState = userState
			                                                              	};

			_service.add_commentCompleted += AddCommentFinished;

			_service.add_commentAsync(_apiKey, _token, objectID, ObjectType2String(objectType), commentText, state);
		}

		private void AddCommentFinished(object sender, add_commentCompletedEventArgs e)
		{
			AsyncCallState<OperationFinished<AddCommentResponse>> state =
				(AsyncCallState<OperationFinished<AddCommentResponse>>) e.UserState;
			AddCommentResponse response;

			if(e.Error != null)
			{
				response = new AddCommentResponse
				           	{
				           		Error = e.Error,
								Status = AddCommentStatus.Failed,
                                PostedComment = null,
				           		UserState = state.UserState
				           	};
			}
			else
			{
				AddCommentStatus parsedStatus = StatusMessageParser.ParseAddCommentStatus(e.Result);

				response = new AddCommentResponse
				           	{
				           		Error = e.Error,
				           		Status = parsedStatus,
				           		PostedComment = parsedStatus == AddCommentStatus.Successful
				           		                	?
				           		                		new Comment(e.comment)
				           		                	:
				           		                		null,
				           		UserState = state.UserState
				           	};

				response.Error = response.Status == AddCommentStatus.Unknown
									?
										new UnknownOperationStatusException(e.Result)
									:
										null;
			}

			state.CallbackDelegate(response);
		}
		#endregion

		#region GetUpdates
		public GetUpdatesResponse GetUpdates(DateTime fromDate, DateTime toDate, GetUpdatesOptions options)
		{
			const string invalidDateTimeArgumentExceptionText =
				"Argument can not be equal to DateTime.MinValue or DateTime.MaxValue";

			if(fromDate == DateTime.MinValue || fromDate == DateTime.MaxValue)
			{
				throw new ArgumentException(invalidDateTimeArgumentExceptionText, "fromDate");
			}

			if (toDate == DateTime.MinValue || toDate == DateTime.MaxValue)
			{
				throw new ArgumentException(invalidDateTimeArgumentExceptionText, "toDate");
			}

			byte[] updates;

			double fromDateUnixConverted = UnixTimeConverter.Instance.ToUnixTime(fromDate);
			double toDateUnixConverted = UnixTimeConverter.Instance.ToUnixTime(toDate);

			string status = _service.get_updates(_apiKey, _token, (long)fromDateUnixConverted, (long)toDateUnixConverted, options.ToStringArray(), out updates);

			return new GetUpdatesResponse
			       	{
			       		Status = StatusMessageParser.ParseGetUpdatesStatus(status)
			       	};
		}
		#endregion

		#region GetFileInfo

		/// <summary>
		/// Retrieves the details for a specified file by its ID
		/// </summary>
		/// <param name="fileID">File ID</param>
		/// <returns>Information about a file</returns>
		public GetFileInfoResponse GetFileInfo(long fileID)
		{
			SOAPFileInfo fileInfo;
			
			string status = _service.get_file_info(_apiKey, _token, fileID, out fileInfo);

			File file = new File
			            	{
			            		Created = UnixTimeConverter.Instance.FromUnixTime(fileInfo.created),
			            		Description = fileInfo.description,
			            		ID = fileInfo.file_id,
								IsShared = fileInfo.shared == 1,
								Name = fileInfo.file_name,
								PublicName = fileInfo.public_name,
								SHA1Hash = fileInfo.sha1,
								Size = fileInfo.size,
                                Updated = UnixTimeConverter.Instance.FromUnixTime(fileInfo.updated)
			            	};

			return new GetFileInfoResponse
			       	{
						Status = StatusMessageParser.ParseGetFileInfoStatus(status),
						File = file
			       	};
		}

		/// <summary>
		/// Retrieves the details for a specified file by its ID
		/// </summary>
		/// <param name="fileID">File ID</param>
		/// <param name="getFileInfoCompleted">Callback method which will be invoked after operation completes</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFileInfoCompleted"/> is <c>null</c></exception>
		public void GetFileInfo(long fileID, OperationFinished<GetFileInfoResponse> getFileInfoCompleted)
		{
			GetFileInfo(fileID, getFileInfoCompleted, null);
		}

		/// <summary>
		/// Retrieves the details for a specified file by its ID
		/// </summary>
		/// <param name="fileID">File ID</param>
		/// <param name="getFileInfoCompleted">Callback method which will be invoked after operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="getFileInfoCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="getFileInfoCompleted"/> is <c>null</c></exception>
		public void GetFileInfo(long fileID, OperationFinished<GetFileInfoResponse> getFileInfoCompleted, object userState)
		{
			ThrowIfParameterIsNull(getFileInfoCompleted, "getFileInfoCompleted");

			_service.get_file_infoCompleted += GetFileInfoFinished;

			AsyncCallState<OperationFinished<GetFileInfoResponse>> state = new AsyncCallState
				<OperationFinished<GetFileInfoResponse>>
			                                                               	{
			                                                               		CallbackDelegate = getFileInfoCompleted,
			                                                               		UserState = userState
			                                                               	};

			_service.get_file_infoAsync(_apiKey, _token, fileID, state);
		}

		private void GetFileInfoFinished(object sender, get_file_infoCompletedEventArgs e)
		{
			AsyncCallState<OperationFinished<GetFileInfoResponse>> state =
				(AsyncCallState<OperationFinished<GetFileInfoResponse>>) e.UserState;
			GetFileInfoResponse response;

			if (e.Error != null)
			{
				response = new GetFileInfoResponse
				           	{
				           		Error = e.Error,
				           		Status = GetFileInfoStatus.Failed,
				           		File = null,
				           		UserState = state.UserState
				           	};
			}
			else
			{
				response = new GetFileInfoResponse
				           	{
				           		Status = StatusMessageParser.ParseGetFileInfoStatus(e.Result),
				           		File = new File
				           		       	{
				           		       		Created = UnixTimeConverter.Instance.FromUnixTime(e.info.created),
				           		       		Description = e.info.description,
				           		       		ID = e.info.file_id,
				           		       		IsShared = e.info.shared == 1,
				           		       		Name = e.info.file_name,
				           		       		PublicName = e.info.public_name,
				           		       		SHA1Hash = e.info.sha1,
				           		       		Size = e.info.size,
				           		       		Updated = UnixTimeConverter.Instance.FromUnixTime(e.info.updated)
				           		       	},
				           		UserState = state.UserState
				           	};

				response.Error = response.Status == GetFileInfoStatus.Unknown
				                 	?
				                 		new UnknownOperationStatusException(e.Result)
				                 	:
				                 		null;
			}

			state.CallbackDelegate(response);
		}

		#endregion

		#region Helper method

		/// <summary>
		/// Throws ArgumentException if <paramref name="parameter"/> is null
		/// </summary>
		/// <param name="parameter">Parameter which needs to be checked</param>
		/// <param name="parameterName">Parameter name</param>
		internal static void ThrowIfParameterIsNull(object parameter, string parameterName)
		{
			if (parameter == null)
			{
				throw new ArgumentException(string.Format("'{0}' can not be null", parameterName));
			}
		}

		/// <summary>
		/// Converts list of tags to comma-separated string which contains tags' IDs
		/// </summary>
		/// <param name="tagList">List of tags</param>
		/// <returns>Comma-separated string which contains tags' IDs</returns>
		private static string ConvertTagPrimitiveCollection2String(TagPrimitiveCollection tagList)
		{
			StringBuilder result = new StringBuilder();

			foreach (TagPrimitive tag in tagList)
			{
				result.Append(tag.ID + ",");
			}

			if (result.Length > 0)
			{
				result.Remove(result.Length - 1, 1);
			}

			return result.ToString();
		}

		/// <summary>
		/// Converts <paramref name="objectType"/> to string representation
		/// </summary>
		/// <param name="objectType">Object type</param>
		/// <returns>String representation of <paramref name="objectType"/> variable</returns>
		/// <exception cref="NotSupportedObjectTypeException">Thrown when method can't convert <paramref name="objectType"/> variable to String</exception>
		private static string ObjectType2String(ObjectType objectType)
		{
			string type;

			switch (objectType)
			{
				case ObjectType.File:
					type = "file";
					break;
				case ObjectType.Folder:
					type = "folder";
					break;
				case ObjectType.Comment:
					type = "comment";
					break;
				default:
					throw new NotSupportedObjectTypeException(objectType);
			}

			return type;
		}

		/// <summary>
		/// Parses XML folder structure message
		/// </summary>
		/// <param name="message">Folder structure message</param>
		/// <returns>Parsed folder structure</returns>
		private Folder ParseFolderStructureXmlMessage(string message)
		{
			Expression<Func<long, TagPrimitive>> materializeTag = tagID => GetTag(tagID);

			return MessageParser.Instance.ParseFolderStructureMessage(message, materializeTag);
		}

		/// <summary>
		/// Extracts first file from zip archive
		/// </summary>
		/// <param name="input">ZIP archive content</param>
		/// <returns>Content of the first ZIPed file or empty byte array</returns>
		private static byte[] Unzip(byte[] input)
		{
			byte[] output;
			byte[] buffer = new byte[1024];

			using (MemoryStream resultStream = new MemoryStream())
			{
				using (MemoryStream inputStream = new MemoryStream())
				{
					inputStream.Write(input, 0, input.Length);
					inputStream.Flush();
					inputStream.Seek(0, SeekOrigin.Begin);
					
					ZipFile zipArchive = new ZipFile(inputStream);

					if (zipArchive.Count > 0 && zipArchive[0].IsFile && zipArchive[0].CanDecompress)
					{
						using (Stream decompressor = zipArchive.GetInputStream(0))
						{
							int readBytes;

							while ((readBytes = decompressor.Read(buffer, 0, buffer.Length)) != 0)
							{
								resultStream.Write(buffer, 0, readBytes);
							}

							decompressor.Close();
						}
					}

					zipArchive.Close();
					
					inputStream.Close();
				}

				output = new byte[resultStream.Length];

				resultStream.Flush();
				resultStream.Seek(0, SeekOrigin.Begin);
				resultStream.Read(output, 0, output.Length);

				resultStream.Close();
			}

			return output;
		}

		#endregion
	}
}
