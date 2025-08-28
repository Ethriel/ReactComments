using AutoMapper;
using ReactComments.Services.Model;

namespace ReactComments.Services.Mapper
{
    public class SubmitCommentProfile : Profile
    {
        public SubmitCommentProfile()
        {
            CreateMap<CommentDTO, SubmitComment>()
                .ForMember(sc => sc.Id, options => options.MapFrom(dto => dto.Id))
                .ForMember(sc => sc.Text, options => options.MapFrom(dto => dto.Text))
                .ForMember(sc => sc.ParentCommentId, options => options.MapFrom(dto => dto.ParentCommentId))
                .ForMember(sc => sc.PersonId, options => options.MapFrom(dto => dto.PersonId));
                //.ForMember(sc => sc.PersonName, options => options.MapFrom(dto => dto.PersonName));

            CreateMap<SubmitComment, CommentDTO>()
                .ForMember(dto => dto.Id, options => options.MapFrom(sc => sc.Id))
                .ForMember(dto => dto.Text, options => options.MapFrom(sc => sc.Text))
                .ForMember(dto => dto.ParentCommentId, options => options.MapFrom(sc => sc.ParentCommentId))
                .ForMember(dto => dto.PersonId, options => options.MapFrom(sc => sc.PersonId));
                //.ForMember(dto => dto.PersonName, options => options.MapFrom(sc => sc.PersonName));
        }
    }
}
