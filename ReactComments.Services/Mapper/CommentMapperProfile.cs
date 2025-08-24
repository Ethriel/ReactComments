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
                .ForMember(dto => dto.Image, options => options.MapFrom(model => model.Image))
                .ForMember(dto => dto.ImageMimeType, options => options.MapFrom(model => model.ImageMimeType))
                .ForMember(dto => dto.TextFile, options => options.MapFrom(model => model.TextFile))
                .ForMember(dto => dto.TextFileName, options => options.MapFrom(model => model.TextFileName))
                .ForMember(dto => dto.ParentComment, options => options.MapFrom(model => model.ParentComment))
                .ForMember(dto => dto.Replies, options => options.MapFrom(model => model.Replies))
                .ForMember(dto => dto.Person, options => options.MapFrom(model => model.Person));

            CreateMap<CommentDTO, Comment>()
                .ForMember(model => model.Id, options => options.MapFrom(dto => dto.Id.ToString()))
                .ForMember(model => model.Text, options => options.MapFrom(dto => dto.Text))
                .ForMember(model => model.CreatedAt, options => options.MapFrom(dto => DateTime.Parse(dto.CreatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind)))
                .ForMember(model => model.UpdatedAt, options => options.MapFrom(dto => DateTime.Parse(dto.UpdatedAt, null, System.Globalization.DateTimeStyles.RoundtripKind)))
                .ForMember(model => model.Image, options => options.MapFrom(dto => dto.Image))
                .ForMember(model => model.ImageMimeType, options => options.MapFrom(dto => dto.ImageMimeType))
                .ForMember(model => model.TextFile, options => options.MapFrom(dto => dto.TextFile))
                .ForMember(model => model.TextFileName, options => options.MapFrom(dto => dto.TextFileName))
                .ForMember(model => model.ParentComment, options => options.MapFrom(dto => dto.ParentComment))
                .ForMember(model => model.Replies, options => options.MapFrom(dto => dto.Replies))
                .ForMember(model => model.Person, options => options.MapFrom(dto => dto.Person));
        }
    }
}
