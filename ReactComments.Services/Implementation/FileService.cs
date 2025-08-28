using Microsoft.AspNetCore.Http;
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
        public CommentDTO? UploadFile(CommentDTO comment, IFormFile file)
        {
            if (file == null)
                return null;

            var uploadFileType = default(UploadFileTypes);

            if (file.ContentType.Contains("image"))
                uploadFileType = UploadFileTypes.Image;
            else if (file.ContentType.Contains("text"))
                uploadFileType = UploadFileTypes.Text;
            else
                return null;

            using var ms = new MemoryStream();
            file.CopyTo(ms);

            var fileAttachment = new FileAttachment() { Contents = ms.ToArray(), Name = file.FileName, Type = uploadFileType };
            var fileAttachmentDto = mapperService.MapDto(fileAttachment);

            switch (uploadFileType)
            {
                case UploadFileTypes.Image:
                    comment.ImageAttachment = fileAttachmentDto;
                    break;
                case UploadFileTypes.Text:
                    comment.TextFileAttachment = fileAttachmentDto;
                    break;
                default:
                    ms.Dispose();
                    return null;
            }

            return comment;
        }

        public async Task<CommentDTO?> UploadFileAsync(CommentDTO comment, IFormFile file)
        {
            return await Task.FromResult(UploadFile(comment, file));
        }
    }
}
