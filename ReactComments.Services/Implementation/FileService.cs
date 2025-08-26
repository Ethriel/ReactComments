using ReactComments.DAL.Model;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;

namespace ReactComments.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IMapperService<FileAttachment, FileAttachmentDTO> mapperService;

        public FileService(IMapperService<FileAttachment, FileAttachmentDTO> mapperService)
        {
            this.mapperService = mapperService;
        }
        public CommentDTO? UploadFile(CommentUploadFile uploadFile)
        {
            var fileType = default(UploadFileTypes);

            if (uploadFile.File.ContentType.Contains("image"))
            {
                fileType = UploadFileTypes.Image;
            }
            else if (uploadFile.Comment.TextFileAttachment is not null)
            {
                fileType = UploadFileTypes.Text;
            }
            else
            {
                return null;
            }

            using var ms = new MemoryStream();
            uploadFile.File.CopyTo(ms);

            var fileAttachment = new FileAttachment() { Contents = ms.ToArray(), Name = uploadFile.File.Name, Type = fileType };
            var fileAttachmentDto = mapperService.MapDto(fileAttachment);

            switch (fileType)
            {
                case UploadFileTypes.Image:
                    {
                        uploadFile.Comment.ImageAttachment = fileAttachmentDto;
                    }
                    break;
                case UploadFileTypes.Text:
                    {
                        uploadFile.Comment.TextFileAttachment = fileAttachmentDto;
                    }
                    break;
                default:
                    {
                        ms.Dispose();
                        return null;
                    }
            }

            return uploadFile.Comment;
        }

        public async Task<CommentDTO?> UploadFileAsync(CommentUploadFile uploadFile)
        {
            return await Task.FromResult(UploadFile(uploadFile));
        }
    }
}
