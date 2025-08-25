using AutoMapper;
using System.Reflection;

namespace ReactComments.Services.Utility
{
    public class MapperProfilesUtility
    {
        public MapperProfilesUtility() { }

        public Type[] GetAllMapperProfiles()
        {
            var parentType = typeof(Profile);
            var assembly = Assembly.GetExecutingAssembly();
            var allTypes = assembly.GetTypes();

            return [.. allTypes.Where(x => x.IsSubclassOf(parentType))];
        }
    }
}
