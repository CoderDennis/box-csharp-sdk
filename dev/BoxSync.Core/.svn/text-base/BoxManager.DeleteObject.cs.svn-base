using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;


namespace BoxSync.Core
{
    public sealed partial class BoxManager
    {
        /// <summary>
        /// Deletes specified object
        /// </summary>
        /// <param name="objectID">ID of the object to delete</param>
        /// <param name="objectType">Type of the object</param>
        /// <returns>Operation status</returns>
        public DeleteObjectStatus DeleteObject(long objectID, ObjectType objectType)
        {
            string type = ObjectType2String(objectType);
            string result = _service.delete(_apiKey, _token, type, objectID);

            return StatusMessageParser.ParseDeleteObjectStatus(result);
        }

        /// <summary>
        /// Asynchronously deletes specified object
        /// </summary>
        /// <param name="objectID">ID of the object to delete</param>
        /// <param name="objectType">Type of the object</param>
        /// <param name="deleteObjectCompleted">Callback method which will be invoked after delete operation completes</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="deleteObjectCompleted"/> is null</exception>
        public void DeleteObject(
            long objectID,
            ObjectType objectType,
            OperationFinished<DeleteObjectResponse> deleteObjectCompleted)
        {
            DeleteObject(objectID, objectType, deleteObjectCompleted, null);
        }

        /// <summary>
        /// Asynchronously deletes specified object
        /// </summary>
        /// <param name="objectID">ID of the object to delete</param>
        /// <param name="objectType">Type of the object</param>
        /// <param name="deleteObjectCompleted">Callback method which will be invoked after delete operation completes</param>
        /// <param name="userState">A user-defined object containing state information. 
        /// This object is passed to the <paramref name="deleteObjectCompleted"/> delegate as a part of response when the operation is completed</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="deleteObjectCompleted"/> is null</exception>
        public void DeleteObject(
            long objectID,
            ObjectType objectType,
            OperationFinished<DeleteObjectResponse> deleteObjectCompleted,
            object userState)
        {
            ThrowIfParameterIsNull(deleteObjectCompleted, "deleteObjectCompleted");

            string type = ObjectType2String(objectType);

            _service.deleteCompleted += DeleteObjectFinished;

            object[] state = { deleteObjectCompleted, userState };

            _service.deleteAsync(_apiKey, _token, type, objectID, state);
        }


        private void DeleteObjectFinished(object sender, deleteCompletedEventArgs e)
        {
            object[] state = (object[]) e.UserState;
            OperationFinished<DeleteObjectResponse> deleteObjectCompleted =
                (OperationFinished<DeleteObjectResponse>) state[0];
            DeleteObjectResponse response;

            if (e.Error != null)
            {
                response = new DeleteObjectResponse
                               {
                                   Status = DeleteObjectStatus.Failed,
                                   UserState = state[1],
                                   Error = e.Error
                               };
            }
            else
            {
                response = new DeleteObjectResponse
                               {
                                   Status = StatusMessageParser.ParseDeleteObjectStatus(e.Result),
                                   UserState = state[1]
                               };

                response.Error = response.Status == DeleteObjectStatus.Unknown
                                     ?
                                         new UnknownOperationStatusException(e.Result)
                                     :
                                         null;
            }

            deleteObjectCompleted(response);
        }
    }
}
