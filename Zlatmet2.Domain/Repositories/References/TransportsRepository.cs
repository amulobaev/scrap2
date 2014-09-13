using System;
using System.Collections.Generic;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.References
{
    public sealed class TransportsRepository : BaseRepository<Transport>
    {
        static TransportsRepository()
        {
            Mapper.CreateMap<Transport, TransportDto>();
            Mapper.CreateMap<TransportDto, Transport>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public TransportsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Transport data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                TransportDto dto = Mapper.Map<Transport, TransportDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        public override IEnumerable<Transport> GetAll()
        {
            using (var connection = ConnectionFactory.Create())
            {
                var dtos = connection.Query<TransportDto>("SELECT * FROM [ReferenceTransports]");
                List<Transport> transports = new List<Transport>();
                foreach (TransportDto dto in dtos)
                {
                    Transport transport = new Transport(dto.Id);
                    Mapper.Map(dto, transport);
                    transports.Add(transport);
                }
                return transports;
            }
        }

        public override Transport GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Transport data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                TransportDto dto = Mapper.Map<Transport, TransportDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);
            }
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}