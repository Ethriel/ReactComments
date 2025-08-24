using AutoMapper;
using System.Reflection;

namespace ReactComments.Services.Utility
{
    public static class MapperProfilesUtility
    {
        static Type[] mapperProfiles;
        static MapperProfilesUtility()
        {
            var parentType = typeof(Profile);
            var assembly = Assembly.GetExecutingAssembly();
            var allTypes = assembly.GetTypes();
            mapperProfiles = [.. allTypes.Where(x => x.IsSubclassOf(parentType))];
        }
        public static Type[] MapperProfiles { get => mapperProfiles; }
    }
}
