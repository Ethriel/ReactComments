using AutoMapper;
using ReactComments.DAL.Model;
using ReactComments.Services.Model;

namespace ReactComments.Services.Mapper
{
    public class CommentMapperProfile : Profile
    {
        public CommentMapperProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(dto => dto.Id, options => options.MapFrom(model => model.Id.ToString()))
                .ForMember(dto => dto.Text, options => options.MapFrom(model => model.Text))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(model => model.CreatedAt.ToString("o")))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(model => model.UpdatedAt.ToString("o")))
                .ForMember(dto => dto.ImageAttachment, options => options.MapFrom(model => model.ImageAttachment))
                .ForMember(dto => dto.TextFileAttachment, options => options.MapFrom(model => model.TextFileAttachment))
                .ForMember(dto => dto.ParentCommentId, options => options.Ignore())
                .AfterMap((model, dto) => dto.ParentCommentId = model.ParentCommentId?.ToString())
                .ForMember(dto => dto.Replies, options => options.MapFrom(model => model.Replies))
                .ForMember(dto => dto.PersonId, options => options.MapFrom(model => model.PersonId))
                .ForMember(dto => dto.PersonName, options => options.MapFrom(model => model.Person.UserName));

            CreateMap<CommentDTO, Comment>()
                .ForMember(model => model.Id, options => options.MapFrom(dto => Guid.Parse(dto.Id)))
                .ForMember(model => model.Text, options => options.MapFrom(dto => dto.Text))
                .ForMember(model => model.CreatedAt, options => options.MapFrom(dto => DateTime.Parse(dto.CreatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind)))
                .ForMember(model => model.UpdatedAt, options => options.MapFrom(dto => DateTime.Parse(dto.UpdatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind)))
                .ForMember(model => model.ImageAttachment, options => options.MapFrom(dto => dto.ImageAttachment))
                .ForMember(model => model.TextFileAttachment, options => options.MapFrom(dto => dto.TextFileAttachment))
                .ForMember(model => model.ParentCommentId, options => options.Ignore())
                .AfterMap((dto, model) =>
                {
                    if (dto.ParentCommentId is not null && Guid.TryParse(dto.ParentCommentId, out Guid id)) model.ParentCommentId = id;
                    else model.ParentCommentId = null;
                })
                .ForMember(model => model.Replies, options => options.MapFrom(dto => dto.Replies))
                //.ForMember(model => model.Person.UserName, options => options.MapFrom(dto => dto.PersonName))
                .ForMember(model => model.PersonId, options => options.MapFrom(dto => dto.PersonId));
        }
}
}
