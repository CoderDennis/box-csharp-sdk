using System;
using System.IO;
using System.Net;
using System.Text;

using BoxSync.Core.Primitives;

using File=System.IO.File;


namespace BoxSync.Core
{
	internal sealed class MultipartWebRequest
	{
		/// <summary>
		/// Initializes request object
		/// </summary>
		/// <param name="submitUrl">Destination Url to submit data</param>
		public MultipartWebRequest(string submitUrl) :
			this(submitUrl, null)
		{ }

		/// <summary>
		/// Initializes request object
		/// </summary>
		/// <param name="submitUrl">Destination Url to submit data</param>
		/// <param name="proxy">Gets or sets proxy information for the request</param>
		public MultipartWebRequest(string submitUrl, IWebProxy proxy) :
			this(submitUrl, proxy, "ISO-8859-1", "gzip,deflate")
		{ }

		/// <summary>
		/// Initializes request object
		/// </summary>
		/// <param name="submitUrl">Destination Url to submit data</param>
		/// <param name="proxy">Gets or sets proxy information for the request</param>
		/// <param name="acceptCharset">Value of "Accept-Charset" header</param>
		/// <param name="acceptEncoding">Value of "Accept-Encoding" header</param>
		public MultipartWebRequest(string submitUrl, IWebProxy proxy, string acceptCharset, string acceptEncoding)
		{
			Boundary = Guid.NewGuid().ToString().Replace("-", string.Empty);
			Url = submitUrl;
			Proxy = proxy;
			AcceptCharset = acceptCharset;
			AcceptEncoding = acceptEncoding;
		}


		/// <summary>
		/// Gets or sets proxy information for the request
		/// </summary>
		public IWebProxy Proxy
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets value of "Accept-Charset" header
		/// </summary>
		public string AcceptCharset
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets value of "Accept-Encoding" header
		/// </summary>
		public string AcceptEncoding
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets destination Url to submit data
		/// </summary>
		public string Url
		{
			get; 
			private set;
		}

		/// <summary>
		/// Separator of multipart data
		/// </summary>
		public string Boundary
		{
			get; 
			private set;
		}


		/// <summary>
		/// Submits user files to specified destination Url
		/// </summary>
		/// <param name="filePaths">List of file paths to submit</param>
		/// <param name="isShared">Indicates if files should be shared or not</param>
		/// <returns>Server response</returns>
		public string SubmitFiles(string[] filePaths, bool isShared)
		{
			return SubmitFiles(filePaths, isShared, null, null);
		}

		/// <summary>
		/// Submits user files to specified destination Url
		/// </summary>
		/// <param name="filePaths">List of file paths to submit</param>
		/// <param name="isShared">Indicates if files should be shared or not</param>
		/// <param name="message">Message to send to all emails from specified <paramref name="emailsToNotify"/></param>
		/// <param name="emailsToNotify">List of email addresses which must be notified about newly uploaded files</param>
		/// <returns>Server response</returns>
		public string SubmitFiles(
			string[] filePaths, 
			bool isShared, 
			string message, 
			string[] emailsToNotify)
		{
			byte[] buffer;

			using (MemoryStream resultStream = new MemoryStream())
			{
				if (filePaths != null)
				{
					buffer = AssembleFilesBlock(filePaths);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				if (!string.IsNullOrEmpty(message))
				{
					buffer = AssembleMessageBlock(message);
					resultStream.Write(buffer, 0, buffer.Length);
				}
				
				buffer = AssembleSharedBlock(isShared);
				resultStream.Write(buffer, 0, buffer.Length);

				if (emailsToNotify != null)
				{
					buffer = AssembleEmailsBlock(emailsToNotify);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				buffer = GetFormattedBoundary(true);
				resultStream.Write(buffer, 0, buffer.Length);

				resultStream.Flush();
				buffer = resultStream.ToArray();
			}

			HttpWebRequest myRequest = CreateRequest(buffer.Length);

			using (Stream newStream = myRequest.GetRequestStream())
			{
				newStream.Write(buffer, 0, buffer.Length);
				newStream.Close();
			}

			string response;

			using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse())
			{
				using (Stream responseStream = myHttpWebResponse.GetResponseStream())
				{
					TextReader reader = new StreamReader(responseStream);

					response = reader.ReadToEnd();
				}
			}

			return response;
		}

		/// <summary>
		/// Submits user files to specified destination Url
		/// </summary>
		/// <param name="filePaths">List of file paths to submit</param>
		/// <param name="isShared">Indicates if files should be shared or not</param>
		/// <param name="message">Message to send to all emails from specified <paramref name="emailsToNotify"/></param>
		/// <param name="emailsToNotify">List of email addresses which must be notified about newly uploaded files</param>
		/// <param name="uploadCompleted">Callback method which will be invoked after upload operation completes</param>
		/// <param name="userState">A user-defined object containing state information. 
		/// This object is passed to the <paramref name="uploadCompleted"/> delegate as a part of response when the operation is completed</param>
		/// <returns>Server response</returns>
		public void SubmitFiles(
			string[] filePaths,
			bool isShared,
			string message,
			string[] emailsToNotify,
			OperationFinished<MultipartRequestUploadResponse> uploadCompleted,
			object userState)
		{
			byte[] buffer;

			using (MemoryStream resultStream = new MemoryStream())
			{
				if (filePaths != null)
				{
					buffer = AssembleFilesBlock(filePaths);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				if (!string.IsNullOrEmpty(message))
				{
					buffer = AssembleMessageBlock(message);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				buffer = AssembleSharedBlock(isShared);
				resultStream.Write(buffer, 0, buffer.Length);

				if (emailsToNotify != null)
				{
					buffer = AssembleEmailsBlock(emailsToNotify);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				buffer = GetFormattedBoundary(true);
				resultStream.Write(buffer, 0, buffer.Length);

				resultStream.Flush();
				buffer = resultStream.ToArray();
			}

			HttpWebRequest myRequest = CreateRequest(buffer.Length);

			Stream writer = myRequest.GetRequestStream();

			State state = new State
			              	{
			              		UserState = userState,
			              		CallbackMethod = uploadCompleted,
								Writer = writer,
								Request = myRequest
			              	};

			try
			{
				writer.BeginWrite(buffer, 0, buffer.Length, UploadCompleted, state);
			}
			catch
			{
				try
				{
					writer.Close();
					writer.Dispose();
				}
				catch { }

				throw;
			}
		}

		private void UploadCompleted(IAsyncResult asyncResult)
		{
			State state = (State) asyncResult.AsyncState;
			bool stopExecution = false;

			MultipartRequestUploadResponse response = new MultipartRequestUploadResponse
			                                          	{
			                                          		UserState = state.UserState
			                                          	};

			try
			{
				state.Writer.EndWrite(asyncResult);
			}
			catch(Exception ex)
			{
				response.Error = ex;
				stopExecution = true;
			}
			finally
			{
				try
				{
					state.Writer.Close();
					state.Writer.Dispose();
				}
				catch
				{ }
			}

			if (!stopExecution)
			{
				HttpWebResponse myHttpWebResponse = null;

				try
				{
					myHttpWebResponse = (HttpWebResponse)state.Request.GetResponse();

					using (Stream responseStream = myHttpWebResponse.GetResponseStream())
					{
						TextReader reader = new StreamReader(responseStream);

						response.Status = reader.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					response.Error = ex;
				}
				finally
				{
					if(myHttpWebResponse != null)
					{
						try
						{
							myHttpWebResponse.Close();
							((IDisposable)myHttpWebResponse).Dispose();
						}
						catch
						{ }
					}
				}
			}

			state.CallbackMethod(response);
		}

		private HttpWebRequest CreateRequest(long contentLength)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);

			webRequest.Proxy = Proxy;
			webRequest.Method = "POST";
			webRequest.AllowWriteStreamBuffering = true;
			webRequest.ContentType = string.Concat("multipart/form-data;boundary=", Boundary);
			webRequest.Headers.Add("Accept-Encoding", AcceptEncoding);
			webRequest.Headers.Add("Accept-Charset", AcceptCharset);
			webRequest.ContentLength = contentLength;

			return webRequest;
		}

		private byte[] AssembleEmailsBlock(string[] emailList)
		{
			byte[] boundaryContent;
			byte[] stringFieldContent;
			byte[][] emailFieldList = new byte[emailList.Length][];

			for (int i = 0; i < emailList.Length; i++)
			{
				boundaryContent = GetFormattedBoundary(false);
				stringFieldContent = AssembleStringValue("emails[]", emailList[i]);

				emailFieldList[i] = new byte[boundaryContent.Length + stringFieldContent.Length];

				Array.Copy(boundaryContent, emailFieldList[i], boundaryContent.Length);
				Array.Copy(stringFieldContent, 0, emailFieldList[i], boundaryContent.Length, stringFieldContent.Length);
			}

			int resultArrayLenght = 0;

			for (int i = 0; i < emailFieldList.Length; i++)
			{
				resultArrayLenght += emailFieldList[i].Length;
			}

			byte[] result = new byte[resultArrayLenght];

			int endIndex = 0;
			for (int i = 0; i < emailFieldList.Length; i++)
			{
				Array.Copy(emailFieldList[i], 0, result, endIndex, emailFieldList[i].Length);

				endIndex += emailFieldList[i].Length;
			}

			return result;
		}

		private byte[] AssembleSharedBlock(bool isFilesShared)
		{
			byte[] boundaryContent = GetFormattedBoundary(false);
			byte[] messageContent = AssembleStringValue("share", isFilesShared ? "1" : "0");
			byte[] result = new byte[boundaryContent.Length + messageContent.Length];

			Array.Copy(boundaryContent, result, boundaryContent.Length);
			Array.Copy(messageContent, 0, result, boundaryContent.Length, messageContent.Length);

			return result;
		}

		private byte[] AssembleMessageBlock(string message)
		{
			byte[] boundaryContent = GetFormattedBoundary(false);
			byte[] messageContent = AssembleStringValue("message", message);
			byte[] result = new byte[boundaryContent.Length + messageContent.Length];

			Array.Copy(boundaryContent, result, boundaryContent.Length);
			Array.Copy(messageContent, 0, result, boundaryContent.Length, messageContent.Length);

			return result;
		}

		private byte[] AssembleFilesBlock(string[] filePaths)
		{
			byte[] buffer;

			using (MemoryStream resultStream = new MemoryStream())
			{
				for (int i = 0; i < filePaths.Length; i++)
				{
					buffer = GetFormattedBoundary(false);
					resultStream.Write(buffer, 0, buffer.Length);

					buffer = AssembleFile(filePaths[i]);
					resultStream.Write(buffer, 0, buffer.Length);
				}

				resultStream.Flush();
				buffer = resultStream.ToArray();
			}

			return buffer;
		}

		private byte[] GetFormattedBoundary(bool isEndBoundary)
		{
			string template = isEndBoundary ? "--{0}--{1}" : "--{0}{1}";

			return Encoding.ASCII.GetBytes(string.Format(template, Boundary, Environment.NewLine));
		}


		private static byte[] AssembleFile(string filePath)
		{
			byte[] buffer;

			using (MemoryStream resultStream = new MemoryStream())
			{
				buffer =
					Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}",
					                                      Guid.NewGuid(),
					                                      Path.GetFileName(filePath), Environment.NewLine));

				resultStream.Write(buffer, 0, buffer.Length);

				buffer =
					Encoding.ASCII.GetBytes("Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine);

				resultStream.Write(buffer, 0, buffer.Length);

				buffer = File.ReadAllBytes(filePath);
				resultStream.Write(buffer, 0, buffer.Length);

				buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
				resultStream.Write(buffer, 0, buffer.Length);

				resultStream.Flush();

				buffer = resultStream.ToArray();
			}

			return buffer;
		}

		private static byte[] AssembleStringValue(string paramName, string paramValue)
		{
			StringBuilder result = new StringBuilder();

			result.AppendFormat("Content-Disposition: form-data; name=\"{0}\"{1}", paramName, Environment.NewLine);
			result.AppendLine();
			result.AppendLine(paramValue);

			return Encoding.ASCII.GetBytes(result.ToString());
		}


		private struct State
		{
			public Stream Writer
			{
				get; 
				set;
			}

			public object UserState
			{
				get; 
				set;
			}

			public OperationFinished<MultipartRequestUploadResponse> CallbackMethod
			{
				get; 
				set;
			}

			public HttpWebRequest Request
			{
				get; 
				set;
			}
		}
	}
}
