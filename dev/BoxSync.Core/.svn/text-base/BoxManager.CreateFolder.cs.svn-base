using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;


namespace BoxSync.Core
{
    public sealed partial class BoxManager
    {
        /// <summary>
        /// Creates folder
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <param name="parentFolderID">ID of the parent folder where new folder needs to be created or '0'</param>
        /// <param name="isShared">Indicates if new folder will be publicly shared</param>
        /// <returns>Operation status</returns>
        public CreateFolderResponse CreateFolder(
            string folderName,
            long parentFolderID,
            bool isShared)
        {
            SOAPFolder soapFolder;
            string response = _service.create_folder(_apiKey, _token, parentFolderID, folderName, isShared ? 1 : 0,
                                                     out soapFolder);

            return new CreateFolderResponse
            {
                Folder = new Folder(soapFolder),
                Status = StatusMessageParser.ParseAddFolderStatus(response)
            };
        }

        /// <summary>
        /// Creates folder
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <param name="parentFolderID">ID of the parent folder where new folder needs to be created or '0'</param>
        /// <param name="isShared">Indicates if new folder will be publicly shared</param>
        /// <param name="folder">Contains all information about newly created folder</param>
        /// <returns>Operation status</returns>
        [Obsolete("Use CreateFolder(string, long, bool):CreateFolderResponse")]
        public CreateFolderStatus CreateFolder(
            string folderName,
            long parentFolderID,
            bool isShared,
            out FolderBase folder)
        {
            SOAPFolder soapFolder;
            string response = _service.create_folder(_apiKey, _token, parentFolderID, folderName, isShared ? 1 : 0, out soapFolder);

            folder = new FolderBase(soapFolder);

            return StatusMessageParser.ParseAddFolderStatus(response);
        }

        /// <summary>
        /// Asynchronously creates folder
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <param name="parentFolderID">ID of the parent folder where new folder needs to be created or '0'</param>
        /// <param name="isShared">Indicates if new folder will be publicly shared</param>
        /// <param name="createFolderCompleted">Callback method which will be invoked after operation completes</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="createFolderCompleted"/> is null</exception>
        public void CreateFolder(
            string folderName,
            long parentFolderID,
            bool isShared,
            OperationFinished<CreateFolderResponse> createFolderCompleted)
        {
            CreateFolder(folderName, parentFolderID, isShared, createFolderCompleted, null);
        }

        /// <summary>
        /// Asynchronously creates folder
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <param name="parentFolderID">ID of the parent folder where new folder needs to be created or '0'</param>
        /// <param name="isShared">Indicates if new folder will be publicly shared</param>
        /// <param name="createFolderCompleted">Callback method which will be invoked after operation completes</param>
        /// <param name="userState">A user-defined object containing state information. 
        /// This object is passed to the <paramref name="createFolderCompleted"/> delegate as a part of response when the operation is completed</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="createFolderCompleted"/> is <c>null</c></exception>
        public void CreateFolder(
            string folderName,
            long parentFolderID,
            bool isShared,
            OperationFinished<CreateFolderResponse> createFolderCompleted,
            object userState)
        {
            ThrowIfParameterIsNull(createFolderCompleted, "createFolderCompleted");

            _service.create_folderCompleted += CreateFolderFinished;

            object[] state = { createFolderCompleted, userState };

            _service.create_folderAsync(_apiKey, _token, parentFolderID, folderName, isShared ? 1 : 0,
                                        state);
        }


        private void CreateFolderFinished(object sender, create_folderCompletedEventArgs e)
        {
            object[] state = (object[])e.UserState;
            OperationFinished<CreateFolderResponse> createFolderFinishedHandler =
                (OperationFinished<CreateFolderResponse>)state[0];
            CreateFolderResponse response;

            if (e.Error != null)
            {
                response = new CreateFolderResponse
                {
                    Status = CreateFolderStatus.Failed,
                    UserState = state[1],
                    Error = e.Error
                };
            }
            else
            {
                response = new CreateFolderResponse
                {
                    Status = StatusMessageParser.ParseAddFolderStatus(e.Result),
                    UserState = state[1]
                };

                if (response.Status == CreateFolderStatus.Successful)
                {
                    response.Folder = new FolderBase(e.folder);
                }

                response.Error = response.Status == CreateFolderStatus.Unknown
                                    ?
                                        new UnknownOperationStatusException(e.Result)
                                    :
                                        null;
            }

            createFolderFinishedHandler(response);
        }
    }
}
