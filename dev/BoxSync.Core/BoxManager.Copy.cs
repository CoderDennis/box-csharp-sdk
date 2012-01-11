using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;


namespace BoxSync.Core
{
    public sealed partial class BoxManager
    {
        /// <summary>
        /// Copies file into specific folder
        /// </summary>
        /// <param name="targetFileID">ID of the file to copy</param>
        /// <param name="destinationFolderID">Destination folder ID</param>
        /// <returns>Operation status</returns>
        public CopyObjectStatus CopyFile(
            long targetFileID,
            long destinationFolderID)
        {
            string type = ObjectType2String(ObjectType.File);
            long newID;

            //string response = _service.copy(_apiKey, _token, type, targetFileID, destinationFolderID, out newID);

            string response = "";

            return StatusMessageParser.ParseCopyObjectStatus(response);
        }

        /// <summary>
        /// Copies file into specific folder
        /// </summary>
        /// <param name="targetFileID">ID of the file to copy</param>
        /// <param name="destinationFolderID">Destination folder ID</param>
        /// <param name="copyFileCompleted">Callback method which will be invoked after operation completes</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="copyFileCompleted"/> is null</exception>
        public void CopyFile(
            long targetFileID,
            long destinationFolderID,
            OperationFinished<CopyFileResponse> copyFileCompleted)
        {
            CopyFile(targetFileID, destinationFolderID, copyFileCompleted, null);
        }

        /// <summary>
        /// Copies file into specific folder
        /// </summary>
        /// <param name="targetFileID">ID of the file to copy</param>
        /// <param name="destinationFolderID">Destination folder ID</param>
        /// <param name="copyFileCompleted">Callback method which will be invoked after operation completes</param>
        /// <param name="userState">A user-defined object containing state information. 
        /// This object is passed to the <paramref name="copyFileCompleted"/> delegate as a part of response when the operation is completed</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="copyFileCompleted"/> is null</exception>
        public void CopyFile(
            long targetFileID,
            long destinationFolderID,
            OperationFinished<CopyFileResponse> copyFileCompleted,
            object userState)
        {
            ThrowIfParameterIsNull(copyFileCompleted, "copyFileCompleted");

            string type = ObjectType2String(ObjectType.File);

            _service.copyCompleted += CopyFileFinished;

            object[] state = { copyFileCompleted, userState };

            _service.copyAsync(_apiKey, _token, type, targetFileID, destinationFolderID, state);
        }

        private void CopyFileFinished(object sender, copyCompletedEventArgs e)
        {
            object[] state = (object[]) e.UserState;
            OperationFinished<CopyFileResponse> copyFileCompleted = (OperationFinished<CopyFileResponse>) state[0];
            CopyFileResponse copyFileResponse;

            if (e.Error != null)
            {
                copyFileResponse = new CopyFileResponse
                                       {
                                           Error = e.Error,
                                           Status = CopyObjectStatus.Failed,
                                           UserState = state[1]
                                       };
            }
            else
            {
                copyFileResponse = new CopyFileResponse
                                       {
                                           Status = StatusMessageParser.ParseCopyObjectStatus(e.Result),
                                           UserState = state[1]
                                       };

                copyFileResponse.Error = copyFileResponse.Status == CopyObjectStatus.Unknown
                                             ?
                                                 new UnknownOperationStatusException(e.Result)
                                             :
                                                 null;
            }

            copyFileCompleted(copyFileResponse);
        }
    }
}
