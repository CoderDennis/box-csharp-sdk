using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;


namespace BoxSync.Core
{
    public sealed partial class BoxManager
    {
        /// <summary>
        /// Creates a copy of existing file.
        /// Newly created file will have name in format "file_name (copy_number).extension"
        /// </summary>
        /// <param name="filePath">Path to file on local hard drive</param>
        /// <param name="fileID">ID of the file for which additional copy should be created</param>
        /// <returns>Operation status</returns>
        [Obsolete("Use CopyFile(long, long) method instead")]
        public FileNewCopyResponse FileNewCopy(
            string filePath,
            long fileID)
        {
            return FileNewCopy(filePath, fileID, false, null, null);
        }

        /// <summary>
        /// Creates a copy of existing file.
        /// Newly created file will have name in format "file_name (copy_number).extension"
        /// </summary>
        /// <param name="filePath">Path to file on local hard drive</param>
        /// <param name="fileID">ID of the file for which additional copy should be created</param>
        /// <param name="isFileShared">Indicates if file should be marked as shared</param>
        /// <param name="message">Message to send to all emails in <paramref name="emailsToNotify"/> list</param>
        /// <param name="emailsToNotify">List of emails which should be notified about newly created copy of the file</param>
        /// <returns>Operation status</returns>
        [Obsolete("Use CopyFile(long, long) method instead")]
        public FileNewCopyResponse FileNewCopy(
            string filePath,
            long fileID,
            bool isFileShared,
            string message,
            string[] emailsToNotify)
        {
            string destinationUrl = string.Format(FILE_NEW_COPY_URI_TEMPLATE, _token, fileID);
            MultipartWebRequest request = new MultipartWebRequest(destinationUrl, Proxy);
            FileNewCopyResponse response;

            try
            {
                string serverResponse = request.SubmitFiles(new[] { filePath }, isFileShared, message, emailsToNotify);
                response = MessageParser.Instance.ParseFileNewCopyResponseMessage(serverResponse);
            }
            catch (Exception ex)
            {
                response = new FileNewCopyResponse
                {
                    Error = ex,
                    Status = FileNewCopyStatus.Failed
                };
            }

            return response;
        }

        /// <summary>
        /// Asynchronously creates a copy of existing file.
        /// Newly created file will have name in format "file_name (copy_number).extension"
        /// </summary>
        /// <param name="filePath">Path to file on local hard drive</param>
        /// <param name="fileID">ID of the file for which additional copy should be created</param>
        /// <param name="fileNewCopyCompleted">Callback method which will be invoked after operation completes</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="fileNewCopyCompleted"/> is null</exception>
        [Obsolete("Use CopyFile(long, long, OperationFinished<CopyFileResponse>) method instead")]
        public void FileNewCopy(
            string filePath,
            long fileID,
            OperationFinished<FileNewCopyResponse> fileNewCopyCompleted)
        {
            FileNewCopy(filePath, fileID, false, null, null, fileNewCopyCompleted, null);
        }

        /// <summary>
        /// Asynchronously creates a copy of existing file.
        /// Newly created file will have name in format "file_name (copy_number).extension"
        /// </summary>
        /// <param name="filePath">Path to file on local hard drive</param>
        /// <param name="fileID">ID of the file for which additional copy should be created</param>
        /// <param name="isFileShared">Indicates if file should be marked as shared</param>
        /// <param name="message">Message to send to all emails in <paramref name="emailsToNotify"/> list</param>
        /// <param name="emailsToNotify">List of emails which should be notified about newly created copy of the file</param>
        /// <param name="fileNewCopyCompleted">Callback method which will be invoked after operation completes</param>
        /// <param name="userState">A user-defined object containing state information. 
        /// This object is passed to the <paramref name="fileNewCopyCompleted"/> delegate as a part of response when the operation is completed</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="fileNewCopyCompleted"/> is null</exception>
        [Obsolete("Use CopyFile(long, long, OperationFinished<CopyFileResponse>) method instead")]
        public void FileNewCopy(
            string filePath,
            long fileID,
            bool isFileShared,
            string message,
            string[] emailsToNotify,
            OperationFinished<FileNewCopyResponse> fileNewCopyCompleted,
            object userState)
        {
            ThrowIfParameterIsNull(fileNewCopyCompleted, "fileNewCopyCompleted");

            string destinationUrl = string.Format(FILE_NEW_COPY_URI_TEMPLATE, _token, fileID);

            MultipartWebRequest request = new MultipartWebRequest(destinationUrl, Proxy);

            object[] state = new[] { fileNewCopyCompleted, userState, fileID };

            request.SubmitFiles(new[] { filePath }, isFileShared, message, emailsToNotify, FileNewCopyFinished, state);
        }

        private void FileNewCopyFinished(MultipartRequestUploadResponse uploadFilesResponse)
        {
            object[] state = (object[])uploadFilesResponse.UserState;
            OperationFinished<FileNewCopyResponse> fileNewCopyCompleted = (OperationFinished<FileNewCopyResponse>)state[0];

            FileNewCopyResponse fileNewCopyResponse = uploadFilesResponse.Error == null
                                                        ?
                                                            MessageParser.Instance.ParseFileNewCopyResponseMessage(
                                                                uploadFilesResponse.Status)
                                                        :
                                                            new FileNewCopyResponse();

            fileNewCopyResponse.UserState = state[1];
            fileNewCopyResponse.Error = uploadFilesResponse.Error;

            fileNewCopyCompleted(fileNewCopyResponse);
        }
    }
}