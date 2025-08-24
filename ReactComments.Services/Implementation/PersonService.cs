using Microsoft.Extensions.Logging;
using ReactComments.DAL.Model;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using ReactComments.Services.Utility.ApiResult.Implementation;
using System.Linq.Expressions;

namespace ReactComments.Services.Implementation
{
    public class PersonService : IPersonService
    {
        private readonly IEntityExtendedService<Person> personService;
        private readonly IMapperService<Person, PersonDTO> mapperService;
        private readonly ILogger<IPersonService> logger;

        public PersonService(IEntityExtendedService<Person> personService, IMapperService<Person, PersonDTO> mapperService, ILogger<IPersonService> logger)
        {
            this.personService = personService;
            this.mapperService = mapperService;
            this.logger = logger;
        }
        public IApiResult AddPerson(PersonDTO personDTO)
        {
            var apiResult = default(IApiResult);

            var existingPerson = FindPersonByEmail(personDTO.Email);
            if (existingPerson is null)
            {
                var personFromDto = mapperService.MapEntity(personDTO);
                if (personFromDto is null)
                {
                    var mappingErrorMsg = "Invalid mapping result";
                    var loggerErrorMsg = $"{mappingErrorMsg}. Attempted for user |email: {personDTO.Email}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [mappingErrorMsg], loggerErrorMessage: loggerErrorMsg);
                }
                else
                {
                    var addResult = personService.Create(personFromDto);
                    if (addResult)
                    {
                        var person = FindPersonByEmail(personDTO.Email);
                        var personToReturnDto = mapperService.MapDto(person);
                        apiResult = new ApiOkResult(ApiResultStatus.Ok, data: personToReturnDto);
                    }
                    else
                    {
                        var errorMessage = "Failed to add new person";
                        var loggerErrorMsg = $"{errorMessage}. Attempted for user |email: {personDTO.Email}|";
                        logger.LogError(loggerErrorMsg);
                        apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                    }
                }
            }
            else
            {
                var errorMessage = $"User |{personDTO.Email}| already exists!";
                logger.LogError(errorMessage);
                apiResult = new ApiErrorResult(ApiResultStatus.Conflict, errors: [errorMessage]);
            }

            return apiResult;
        }

        public async Task<IApiResult> AddPersonAsync(PersonDTO personDTO)
        {
            return await Task.FromResult(AddPerson(personDTO));
        }

        public IApiResult DeletePerson(object id)
        {
            var apiResult = default(IApiResult);

            var existingPerson = FindPersonById(id);
            if (existingPerson is null)
            {
                var errorMessage = "Failed to delete a person. Person does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for user |id: {id}|";
                logger.LogError(loggerErrorMsg);
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }
            else
            {
                var deleteResult = personService.Delete(id);
                if (deleteResult)
                {
                    apiResult = new ApiOkResult(ApiResultStatus.NoContent);
                }
                else
                {
                    var errorMessage = "Failed to delete a person";
                    var loggerErrorMsg = $"{errorMessage}. Attempted for user |id: {id}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> DeletePersonAsync(object id)
        {
            return await Task.FromResult(DeletePerson(id));
        }

        public IApiResult GetPeople()
        {
            var apiResult = default(IApiResult);

            var people = personService.Read().ToArray();
            if (people is null || people.Length == 0)
            {
                var noPeopleMessage = "No people to show";
                logger.LogWarning(noPeopleMessage);
                apiResult = new ApiOkResult(ApiResultStatus.NoContent, noPeopleMessage);
            }
            else
            {
                var peopleDtos = mapperService.MapDtos(people);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: peopleDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetPeopleAsync()
        {
            return await Task.FromResult(GetPeople());
        }

        public IApiResult GetPeopleByCondition(Expression<Func<Person, bool>> condition)
        {
            var apiResult = default(IApiResult);

            var people = personService.ReadEntitiesByCondition(condition).ToArray();
            if (people is null || people.Length == 0)
            {
                var noPeopleMessage = "No people to show";
                logger.LogWarning(noPeopleMessage);
                apiResult = new ApiOkResult(ApiResultStatus.NoContent, noPeopleMessage);
            }
            else
            {
                var peopleDtos = mapperService.MapDtos(people);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, data: peopleDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetPeopleByConditionAsync(Expression<Func<Person, bool>> condition)
        {
            return await Task.FromResult(GetPeopleByCondition(condition));
        }

        public IApiResult UpdatePerson(PersonDTO personDTO)
        {
            var apiResult = default(IApiResult);

            var existingPerson = FindPersonById(personDTO.Id);
            if (existingPerson is null)
            {
                var errorMessage = "Failed to update a person. Person does not exist!";
                var loggerErrorMsg = $"{errorMessage}. Attempted for user |id: {personDTO.Id}|";
                logger.LogError(loggerErrorMsg);
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
            }
            else
            {
                var toUpdatePerson = mapperService.MapEntity(personDTO);
                var updateResultPerson = personService.Update(existingPerson, toUpdatePerson);
                if (updateResultPerson is not null)
                {
                    var personToReturnDto = mapperService.MapDto(updateResultPerson);
                    apiResult = new ApiOkResult(ApiResultStatus.Ok, data: personToReturnDto);
                }
                else
                {
                    var errorMessage = "Failed to update a person";
                    var loggerErrorMsg = $"{errorMessage}. Attempted for user |id: {personDTO.Id}|";
                    logger.LogError(loggerErrorMsg);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, errors: [errorMessage], loggerErrorMessage: loggerErrorMsg);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> UpdatePersonAsync(PersonDTO personDTO)
        {
            return await Task.FromResult(UpdatePerson(personDTO));
        }

        private Person? FindPersonByEmail(string email)
        {
            return personService.ReadByCondition(p => p.Email == email);
        }

        private Person? FindPersonById(object id)
        {
            return personService.ReadById(id);
        }
    }
}
