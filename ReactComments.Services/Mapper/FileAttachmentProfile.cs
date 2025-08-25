using AutoMapper;
using ReactComments.DAL.Model;
using ReactComments.Services.Model;

namespace ReactComments.Services.Mapper
{
    public class FileAttachmentProfile : Profile
    {
        public FileAttachmentProfile()
        {
            CreateMap<FileAttachment, FileAttachmentDTO>()
                .ForMember(dto => dto.Id, options => options.MapFrom(model => model.Id.ToString()))
                .ForMember(dto => dto.Name, options => options.MapFrom(model => model.Name))
                .ForMember(dto => dto.Type, options => options.MapFrom(model => model.Type.ToString()))
                .ForMember(dto => dto.ImageCommentId, options => options.Ignore())
                .ForMember(dto => dto.TextFileCommentId, options => options.Ignore())
                .AfterMap((model, dto) =>
                {
                    dto.ImageCommentId = model.ImageCommentId?.ToString();
                    dto.TextFileCommentId = model.TextFileCommentId?.ToString();
                });

            CreateMap<FileAttachmentDTO, FileAttachment>()
                .ForMember(model => model.Id, options => options.MapFrom(dto => Guid.Parse(dto.Id)))
                .ForMember(model => model.Name, options => options.MapFrom(dto => dto.Name))
                .ForMember(model => model.Type, options => options.MapFrom(dto => Enum.Parse<UploadFileTypes>(dto.Type)))
                .ForMember(model => model.ImageComment, options => options.Ignore())
                .ForMember(model => model.TextFileComment, options => options.Ignore())
                .AfterMap((dto, model) =>
                {
                    model.ImageCommentId = dto.ImageCommentId != null && Guid.TryParse(dto.ImageCommentId, out Guid imageId) ? imageId : null;
                    model.TextFileCommentId = dto.TextFileCommentId != null && Guid.TryParse(dto.TextFileCommentId, out Guid textFileId) ? textFileId : null;
                });
        }
    }
}
