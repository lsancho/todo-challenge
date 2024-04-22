using AutoMapper;
using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Api
{
    public class ApplicationMapperProfile : Profile
    {
        public ApplicationMapperProfile()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap();
        }
    }
}
