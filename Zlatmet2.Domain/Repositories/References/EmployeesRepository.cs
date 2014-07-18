using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.References
{
    public abstract class EmployeesRepository : BaseRepository<Employee, EmployeeDto>
    {
        static EmployeesRepository()
        {
            Mapper.CreateMap<Employee, EmployeeDto>();
            Mapper.CreateMap<EmployeeDto, Employee>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        protected EmployeesRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Employee data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                EmployeeDto dto = Mapper.Map<Employee, EmployeeDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        protected IEnumerable<Employee> GetAll(EmployeeType type)
        {
            using (var connection = ConnectionFactory.Create())
            {
                List<EmployeeDto> dtos = connection.Query<EmployeeDto>(
                    "SELECT * FROM [ReferenceEmployees] WHERE Type = @Type", new { Type = (int)type }).ToList();

                List<Employee> employees = new List<Employee>();
                foreach (EmployeeDto dto in dtos)
                {
                    Employee employee = new Employee(dto.Id);
                    Mapper.Map(dto, employee);
                    employees.Add(employee);
                }
                return employees;
            }
        }

        public override Employee GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Employee data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                EmployeeDto dto = Mapper.Map<Employee, EmployeeDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);
            }
        }

    }
}