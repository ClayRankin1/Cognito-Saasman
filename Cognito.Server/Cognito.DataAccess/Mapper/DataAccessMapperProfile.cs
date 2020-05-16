using AutoMapper;
using Cognito.DataAccess.Repositories.Results;

namespace Cognito.DataAccess.Mapper
{
    public class DataAccessMapperProfile : Profile
    {
        public DataAccessMapperProfile()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>));
        }
    }
}
