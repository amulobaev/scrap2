using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes;
using Zlatmet2.Domain.Entities;

namespace Zlatmet2.Domain.Repositories.Service
{
    public class UsersRepository : BaseRepository<User>
    {
        static UsersRepository()
        {
            Mapper.CreateMap<User, UserEntity>();
            Mapper.CreateMap<UserEntity, User>()
                .ConstructUsing(x => new User(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public UsersRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(User data)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity userEntity = Mapper.Map<User, UserEntity>(data);
                context.Users.Add(userEntity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<User> GetAll()
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity[] userEntities = context.Users.ToArray();
                return Mapper.Map<UserEntity[], User[]>(userEntities);
            }
        }

        public override User GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity userEntity = context.Users.FirstOrDefault(x => x.Id == id);
                return userEntity != null ? Mapper.Map<UserEntity, User>(userEntity) : null;
            }
        }

        public override void Update(User data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity userEntity = context.Users.FirstOrDefault(x => x.Id == data.Id);
                if (userEntity != null)
                {
                    if (data.Login != userEntity.Login)
                        userEntity.Login = data.Login;
                    if (!string.IsNullOrEmpty(data.Password))
                        userEntity.Password = data.Password;
                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity userEntity = context.Users.FirstOrDefault(x => x.Id == id);
                if (userEntity != null)
                {
                    context.Users.Remove(userEntity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public void CreateOrUpdate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                UserEntity userEntity = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (userEntity == null)
                {
                    userEntity = Mapper.Map<User, UserEntity>(user);
                    context.Users.Add(userEntity);
                }
                else
                {
                    Mapper.Map(user, userEntity);
                }
                context.SaveChanges();
            }
        }
    }
}