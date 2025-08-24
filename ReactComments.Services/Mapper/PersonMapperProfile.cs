using AutoMapper;
using ReactComments.DAL.Model;
using ReactComments.Services.Model;

namespace ReactComments.Services.Mapper
{
    public class PersonMapperProfile : Profile
    {
        public PersonMapperProfile()
        {
            CreateMap<Person, PersonDTO>()
                .ForMember(dto => dto.Id, options => options.MapFrom(model => model.Id))
                .ForMember(dto => dto.UserName, options => options.MapFrom(model => model.UserName))
                .ForMember(dto => dto.Email, options => options.MapFrom(model => model.Email))
                .ForMember(dto => dto.Role, options => options.MapFrom(model => model.AppRole.Name))
                .ForMember(dto => dto.HomePage, options => options.MapFrom(model => model.HomePage))
                .ForMember(dto => dto.RegisteredDate, options => options.MapFrom(model => model.RegisteredDate.ToShortDateString()))
                .ForMember(dto => dto.Comments, options => options.MapFrom(model => model.Comments));

            CreateMap<PersonDTO, Person>()
                .ForMember(model => model.Id, options => options.MapFrom(dto => dto.Id))
                .ForMember(model => model.UserName, options => options.MapFrom(dto => dto.UserName))
                .ForMember(model => model.Email, options => options.MapFrom(dto => dto.Email))
                .ForMember(model => model.HomePage, options => options.MapFrom(dto => dto.HomePage))
                .ForMember(model => model.RegisteredDate, options => options.MapFrom(dto => DateTime.Parse(dto.RegisteredDate)))
                .ForMember(model => model.Comments, options => options.MapFrom(dto => dto.Comments));
        }
    }
}
