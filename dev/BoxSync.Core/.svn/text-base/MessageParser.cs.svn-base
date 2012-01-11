using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using File=BoxSync.Core.Primitives.File;


namespace BoxSync.Core
{
	internal sealed class MessageParser
	{
		private readonly static MessageParser _instance = new MessageParser();

		internal static MessageParser Instance
		{
			get
			{
				return _instance;
			}
		}

		internal Folder ParseFolderStructureMessage(string message, Expression<Func<long, TagPrimitive>> materializeTag)
		{
			if(string.IsNullOrEmpty(message))
			{
				return new Folder();
			}

			XDocument messageDocument = XDocument.Parse(message);

			return ParseFolderElement(messageDocument.Root, materializeTag);
		}

		internal FileNewCopyResponse ParseFileNewCopyResponseMessage(string message)
		{
			FileNewCopyResponse response = new FileNewCopyResponse();
			XDocument doc = XDocument.Parse(message);

			XElement statusElement = GetStatusElement(doc.Root);

			response.Status = ParseFileNewCopyStatus(statusElement.Value);

			return response;
		}

		internal OverwriteFileResponse ParseOverwriteFileResponseMessage(string message)
		{
			OverwriteFileResponse response = new OverwriteFileResponse();
			XDocument doc = XDocument.Parse(message);

			XElement filesElement = GetFilesElement(doc.Root);
			XElement statusElement = GetStatusElement(doc.Root);

			IEnumerable<XElement> fileElements = GetFileElements(filesElement);

			File file;
			UploadFileError error;
			foreach (XElement fileElement in fileElements)
			{
				ParseFileElement(fileElement, out file, out error);

				response.UploadedFileStatus.Add(file, error);
			}

			response.Status = ParseOverwriteFileStatus(statusElement.Value);

			return response;
		}

		internal UploadFileResponse ParseUploadResponseMessage(string message)
		{
			UploadFileResponse response = new UploadFileResponse();
			XDocument doc = XDocument.Parse(message);

			XElement filesElement = GetFilesElement(doc.Root);
			XElement statusElement = GetStatusElement(doc.Root);

			IEnumerable<XElement> fileElements = GetFileElements(filesElement);

			File file;
			UploadFileError error;
			foreach (XElement fileElement in fileElements)
			{
				ParseFileElement(fileElement, out file, out error);

				response.UploadedFileStatus.Add(file, error);
			}

			response.Status = ParseUploadStatus(statusElement.Value);

			return response;
		}
        
		internal TagPrimitiveCollection ParseExportTagsMessage(string message)
		{
			TagPrimitiveCollection toReturn = new TagPrimitiveCollection();
			
			XDocument messageDocument = XDocument.Parse(message);
			XElement tagsElement = GetTagsElement(messageDocument.Root);
			IEnumerable<XElement> tagELements = GetTagElements(tagsElement);
			
			foreach (XElement tagElement in tagELements)
			{
				toReturn.AddTag(ParseTagElement(tagElement));
			}

			return toReturn;
		}


		private Folder ParseFolderElement(XElement folderElement, Expression<Func<long, TagPrimitive>> materializeTag)
		{
			Folder folder = new Folder();

			XAttribute idAttribute = folderElement.Attribute(XName.Get("id"));
			XAttribute nameAttribute = folderElement.Attribute(XName.Get("name"));
			XAttribute descriptionAttribute = folderElement.Attribute(XName.Get("description"));
			XAttribute userIdAttribute = folderElement.Attribute(XName.Get("user_id"));
			XAttribute sharedAttribute = folderElement.Attribute(XName.Get("shared"));
			XAttribute sharedLinkAttribute = folderElement.Attribute(XName.Get("shared_link"));
			XAttribute permissionsAttribute = folderElement.Attribute(XName.Get("permissions"));
			XAttribute sizeAttribute = folderElement.Attribute(XName.Get("size"));
			XAttribute fileCountAttribute = folderElement.Attribute(XName.Get("file_count"));
			XAttribute createdAttribute = folderElement.Attribute(XName.Get("created"));
			XAttribute updatedAttribute = folderElement.Attribute(XName.Get("updated"));


			folder.Description = descriptionAttribute == null ? null : descriptionAttribute.Value;
			folder.ID = long.Parse(idAttribute.Value);
			folder.IsShared = sharedAttribute == null ? null : (bool?)sharedAttribute.Value.Equals("1");
			folder.Name = nameAttribute.Value;
			folder.SharedLink = sharedLinkAttribute == null ? null : sharedLinkAttribute.Value;
			folder.OwnerID = userIdAttribute == null ? null : (long?)long.Parse(userIdAttribute.Value);
			folder.Size = sizeAttribute == null ? null : (long?)long.Parse(sizeAttribute.Value);
			folder.PermissionFlags = permissionsAttribute == null ? null : (UserPermissionFlags?)ParsePermissionString(permissionsAttribute.Value);

			if (createdAttribute != null && !string.IsNullOrEmpty(createdAttribute.Value))
			{
				folder.Created = UnixTimeConverter.Instance.FromUnixTime(double.Parse(createdAttribute.Value));
			}

			if (updatedAttribute != null && !string.IsNullOrEmpty(updatedAttribute.Value))
			{
				folder.Updated = UnixTimeConverter.Instance.FromUnixTime(double.Parse(updatedAttribute.Value));
			}

			if (fileCountAttribute != null && !string.IsNullOrEmpty(fileCountAttribute.Value))
			{
				folder.FileCount = int.Parse(fileCountAttribute.Value);
			}


			XElement folders = GetFoldersElement(folderElement);
			IEnumerable<XElement> folderElements = GetFolderElements(folders);
			foreach (XElement fElement in folderElements)
			{
				folder.Folders.Add(ParseFolderElement(fElement, materializeTag));
			}


			XElement files = GetFilesElement(folderElement);
			IEnumerable<XElement> fileElements = GetFileElements(files);
			foreach (XElement fileElement in fileElements)
			{
				folder.Files.Add(ParseFileElement(fileElement));
			}

			XElement tagsElement = GetTagsElement(folderElement);
			IEnumerable<XElement> tagELements = GetTagElements(tagsElement);
			foreach (XElement tagElement in tagELements)
			{
				folder.Tags.Add(ParseTagElement(tagElement, materializeTag));
			}

			return folder;
		}

		private File ParseFileElement(XElement fileElement)
		{
			XAttribute nameAttribute = fileElement.Attribute(XName.Get("file_name"));
			
			if(nameAttribute == null)
			{
				return new File();
			}

			switch (Path.GetExtension(nameAttribute.Value))
			{
				case ".jpeg":
				case ".jpg":
					return ParseImageTypeElement(fileElement);
				default:
					return ParseUnknownFileTypeElement(fileElement);
			}
		}


		private File ParseUnknownFileTypeElement(XElement fileElement)
		{
			File file = new File();

			XAttribute idAttribute = fileElement.Attribute(XName.Get("id"));
			XAttribute nameAttribute = fileElement.Attribute(XName.Get("file_name"));
			XAttribute descriptionAttribute = fileElement.Attribute(XName.Get("description"));
			XAttribute sizeAttribute = fileElement.Attribute(XName.Get("size"));
			XAttribute createdAttribute = fileElement.Attribute(XName.Get("created"));
			XAttribute updatedAttribute = fileElement.Attribute(XName.Get("updated"));
			XAttribute userIdAttribute = fileElement.Attribute(XName.Get("user_id"));
			XAttribute permissionsAttribute = fileElement.Attribute(XName.Get("permissions"));
			XAttribute sharedAttribute = fileElement.Attribute(XName.Get("shared"));
			XAttribute sha1Attribute = fileElement.Attribute(XName.Get("sha1"));
			XAttribute sharedLinkAttribute = fileElement.Attribute(XName.Get("shared_link"));
			XAttribute publicNameAttribute = fileElement.Attribute(XName.Get("public_name"));
			

			if (idAttribute != null)
			{
				file.ID = long.Parse(idAttribute.Value);
			}

			if (nameAttribute != null)
			{
				file.Name = nameAttribute.Value;
			}

			if (descriptionAttribute != null)
			{
				file.Description = descriptionAttribute.Value;
			}

			if (sharedAttribute != null)
			{
				file.IsShared = sharedAttribute.Value.Equals("1");
			}

			if (createdAttribute != null)
			{
				file.Created = UnixTimeConverter.Instance.FromUnixTime(double.Parse(createdAttribute.Value));
			}

			if (updatedAttribute != null)
			{
				file.Updated = UnixTimeConverter.Instance.FromUnixTime(double.Parse(updatedAttribute.Value));
			}

			if (sizeAttribute != null)
			{
				file.Size = long.Parse(sizeAttribute.Value);
			}

			if (userIdAttribute != null)
			{
				file.OwnerID = long.Parse(userIdAttribute.Value);
			}

			if (permissionsAttribute != null)
			{
				file.PermissionFlags = ParsePermissionString(permissionsAttribute.Value);
			}

			if (sha1Attribute != null)
			{
				file.SHA1Hash = sha1Attribute.Value;
			}

			if (sharedLinkAttribute != null)
			{
				file.SharedLink = sharedLinkAttribute.Value;
			}

			if (publicNameAttribute != null)
			{
				file.PublicName = publicNameAttribute.Value;
			}

			return file;
		}
		private ImageFile ParseImageTypeElement(XElement fileElement)
		{
			ImageFile file = new ImageFile(ParseUnknownFileTypeElement(fileElement));

			XAttribute largeThumbnailAttribute = fileElement.Attribute(XName.Get("large_thumbnail"));
			XAttribute thumbnailAttribute = fileElement.Attribute(XName.Get("thumbnail"));
			XAttribute smallThumbnailAttribute = fileElement.Attribute(XName.Get("small_thumbnail"));
			XAttribute previewThumbnailAttribute = fileElement.Attribute(XName.Get("preview_thumbnail"));
			XAttribute largerThumbnailAttribute = fileElement.Attribute(XName.Get("larger_thumbnail"));
			
			
			if (largeThumbnailAttribute != null)
			{
				file.LargeThumbnailUrl = largeThumbnailAttribute.Value;
			}

			if (thumbnailAttribute != null)
			{
				file.ThumbnailUrl = thumbnailAttribute.Value;
			}

			if (smallThumbnailAttribute != null)
			{
				file.ThumbnailUrl = smallThumbnailAttribute.Value;
			}

			if (previewThumbnailAttribute != null)
			{
				file.PreviewThumbnailUrl = previewThumbnailAttribute.Value;
			}

			if (largerThumbnailAttribute != null)
			{
				file.LargerThumbnailUrl = largerThumbnailAttribute.Value;
			}


			return file;
		}


		private void ParseFileElement(XElement fileElement, out File file, out UploadFileError error)
		{
			XAttribute errorAttribute = fileElement.Attribute(XName.Get("error"));

			file = ParseFileElement(fileElement);
			
			error = errorAttribute != null ? ParseUploadFileError(errorAttribute.Value) : UploadFileError.None;
		}

		private TagPrimitive ParseTagElement(XElement tagElement, Expression<Func<long, TagPrimitive>> materializeTag)
		{
			XAttribute idAttribute = tagElement.Attribute(XName.Get("id"));

			if(idAttribute == null)
			{
				return new TagPrimitive(-1, string.Empty);
			}

			long id = long.Parse(idAttribute.Value);

			if (!tagElement.IsEmpty)
			{
				return new TagPrimitive(id, tagElement.Value);
			}

			return new TagPrimitive(id, materializeTag);
		}
		private static TagPrimitive ParseTagElement(XElement tagElement)
		{
			XAttribute idAttribute = tagElement.Attribute(XName.Get("id"));

			string text = tagElement.Value;
			long id = long.Parse(idAttribute.Value);

			return new TagPrimitive(id, text);
		}

		private IEnumerable<XElement> GetFolderElements(XElement parentElement)
		{
			if (parentElement == null)
			{
				return Enumerable.Empty<XElement>();
			}

			return parentElement.Elements("folder");
		}

		private IEnumerable<XElement> GetFileElements(XElement parentElement)
		{
			if (parentElement == null)
			{
				return Enumerable.Empty<XElement>();
			}

			return parentElement.Elements("file");
		}

		private XElement GetFoldersElement(XElement parentElement)
		{
			return parentElement.Elements("folders").FirstOrDefault();
		}

		private XElement GetFilesElement(XElement parentElement)
		{
			return parentElement.Elements(XName.Get("files")).FirstOrDefault();
		}

		private XElement GetStatusElement(XElement parentElement)
		{
			return parentElement.Elements(XName.Get("status")).FirstOrDefault();
		}

		private static XElement GetTagsElement(XElement parentElement)
		{
			return parentElement.Elements("tags").FirstOrDefault();
		}

		private static IEnumerable<XElement> GetTagElements(XElement parentElement)
		{
			if (parentElement == null)
			{
				return Enumerable.Empty<XElement>();
			}

			return parentElement.Elements("tag");
		}

		private UserPermissionFlags ParsePermissionString(string permissions)
		{
			UserPermissionFlags toReturn = UserPermissionFlags.None;

			if(permissions.Contains('d'))
			{
				toReturn = toReturn | UserPermissionFlags.Download;
			}

			if (permissions.Contains('e'))
			{
				toReturn = toReturn | UserPermissionFlags.Delete;
			}

			if (permissions.Contains('n'))
			{
				toReturn = toReturn | UserPermissionFlags.Rename;
			}

			if (permissions.Contains('s'))
			{
				toReturn = toReturn | UserPermissionFlags.Share;
			}

			if (permissions.Contains('u'))
			{
				toReturn = toReturn | UserPermissionFlags.Upload;
			}

			if (permissions.Contains('v'))
			{
				toReturn = toReturn | UserPermissionFlags.View;
			}

			if ((((int)toReturn) & 127) > 0)
			{
				toReturn = toReturn ^ UserPermissionFlags.None;
			}

			return toReturn;
		}
		private FileNewCopyStatus ParseFileNewCopyStatus(string status)
		{
			FileNewCopyStatus toReturn;

			switch (status)
			{
				case "upload_ok":
					toReturn = FileNewCopyStatus.Successful;
					break;
				case "upload_some_files_failed":
					toReturn = FileNewCopyStatus.Failed;
					break;
				case "not_logged_id":
					toReturn = FileNewCopyStatus.NotLoggedID;
					break;
				case "application_restricted":
					toReturn = FileNewCopyStatus.ApplicationRestricted;
					break;
				default:
					toReturn = FileNewCopyStatus.Unknown;
					break;
			}

			return toReturn;
		}
		private OverwriteFileStatus ParseOverwriteFileStatus(string status)
		{
			OverwriteFileStatus toReturn;

			switch (status)
			{
				case "upload_ok":
					toReturn = OverwriteFileStatus.Successful;
					break;
				case "upload_some_files_failed":
					toReturn = OverwriteFileStatus.Failed;
					break;
				case "not_logged_id":
					toReturn = OverwriteFileStatus.NotLoggedID;
					break;
				case "application_restricted":
					toReturn = OverwriteFileStatus.ApplicationRestricted;
					break;
				default:
					toReturn = OverwriteFileStatus.Unknown;
					break;
			}

			return toReturn;
		}
		private UploadFileStatus ParseUploadStatus(string status)
		{
			UploadFileStatus toReturn;

			switch (status)
			{
				case "upload_ok":
					toReturn = UploadFileStatus.Successful;
					break;
				case "upload_some_files_failed":
					toReturn = UploadFileStatus.Failed;
					break;
				case "not_logged_id":
					toReturn = UploadFileStatus.NotLoggedID;
					break;
				case "application_restricted":
					toReturn = UploadFileStatus.ApplicationRestricted;
					break;
				default:
					toReturn = UploadFileStatus.Unknown;
					break;
			}

			return toReturn;
		}
		private UploadFileError ParseUploadFileError(string error)
		{
			UploadFileError toReturn;

			switch (error)
			{
				case "not_enough_free_space":
					toReturn = UploadFileError.NotEnoughFreeSpace;
					break;
				case "filesize_limit_exceeded":
					toReturn = UploadFileError.FileSizeLimitExceeded;
					break;
				case "access_denied":
					toReturn = UploadFileError.AccessDenied;
					break;
				default:
					toReturn = UploadFileError.Unknown;
					break;
			}

			return toReturn;
		}
	}
}
