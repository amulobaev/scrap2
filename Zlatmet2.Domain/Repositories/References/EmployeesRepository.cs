using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain.Entities.References;

namespace Zlatmet2.Domain.Repositories.References
{
    public abstract class EmployeesRepository : BaseRepository<Employee>
    {
        static EmployeesRepository()
        {
            Mapper.CreateMap<Employee, EmployeeEntity>();
            Mapper.CreateMap<EmployeeEntity, Employee>()
                .ConstructUsing(x => new Employee(x.Id, (EmployeeType)x.Type))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Type, opt => opt.Ignore());
        }

        protected EmployeesRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Employee data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                EmployeeEntity employeeEntity = Mapper.Map<Employee, EmployeeEntity>(data);
                context.Employees.Add(employeeEntity);
                context.SaveChanges();
            }
        }

        protected IEnumerable<Employee> GetAll(EmployeeType type)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                EmployeeEntity[] entities = context.Employees.Where(x => x.Type == (int)type).ToArray();
                return Mapper.Map<EmployeeEntity[], Employee[]>(entities);
            }
        }

        public override Employee GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                EmployeeEntity entity = context.Employees.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<EmployeeEntity, Employee>(entity) : null;
            }
        }

        public override void Update(Employee data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                EmployeeEntity entity = context.Employees.FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(data, entity);
                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                EmployeeEntity entity = context.Employees.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.Employees.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}