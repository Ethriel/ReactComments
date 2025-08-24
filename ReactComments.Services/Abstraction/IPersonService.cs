using ReactComments.DAL.Model;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using System.Linq.Expressions;

namespace ReactComments.Services.Abstraction
{
    public interface IPersonService
    {
        IApiResult AddPerson(PersonDTO personDTO);
        Task<IApiResult> AddPersonAsync(PersonDTO personDTO);
        IApiResult DeletePerson(object id);
        Task<IApiResult> DeletePersonAsync(object id);
        IApiResult GetPeople();
        Task<IApiResult> GetPeopleAsync();
        IApiResult GetPeopleByCondition(Expression<Func<Person, bool>> condition);
        Task<IApiResult> GetPeopleByConditionAsync(Expression<Func<Person, bool>> condition);
        IApiResult UpdatePerson(PersonDTO personDTO);
        Task<IApiResult> UpdatePersonAsync(PersonDTO personDTO);
    }
}
