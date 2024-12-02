using AutoMapper;
using ATM.Core.Domain.Users;
using ATM.Core.Services.Users;

namespace ATM.Core.Core
{
    /// <summary>
    /// Auto mapper profile.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // todo: make an interface for all models, and then scan everything and auto apply
            // user
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
            CreateMap<User, CreateUserRequest>();
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, UpdateUserRequest>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, DeleteUserRequest>();
            CreateMap<DeleteUserRequest, User>();

        }
    }
}
