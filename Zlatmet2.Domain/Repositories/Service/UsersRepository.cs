using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes;
using Zlatmet2.Domain.Dto.Service;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.Service
{
    public class UsersRepository : BaseRepository<User, UserDto>
    {
        static UsersRepository()
        {
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public UsersRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(User data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                UserDto dto = Mapper.Map<User, UserDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        public override IEnumerable<User> GetAll()
        {
            using (var connection = ConnectionFactory.Create())
            {
                var dtos = connection.Query<UserDto>(QueryObject.GetAllQuery(typeof(UserDto))).ToList();
                List<User> users = new List<User>();
                foreach (UserDto dto in dtos)
                {
                    User user = new User(dto.Id);
                    Mapper.Map(dto, user);
                    users.Add(user);
                }
                return users;
            }
        }

        public override User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(User data)
        {
            throw new NotImplementedException();
        }
    }
}
