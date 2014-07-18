using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Repositories.Documents
{
    public class DocumentsRepository : BaseRepository
    {
        public DocumentsRepository(IModelContext context)
            : base(context)
        {
        }

        public IEnumerable<Document> GetAll(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using (var connection = ConnectionFactory.Create())
            {
                var p = new DynamicParameters();
                p.Add("@DateFrom", dateFrom, DbType.Date);
                p.Add("@DateTo", dateTo, DbType.Date);

                return connection.Query<Document>("usp_GetDocuments", p, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int GetNextDocumentNumber()
        {
            var year = DateTime.Today.Year;

            using (var connection = ConnectionFactory.Create())
            {
                var p = new DynamicParameters();
                p.Add("@Year", year, DbType.Int32);
                var result =
                    connection.Query<int?>("GetMaxDocumentNumber", p, commandType: CommandType.StoredProcedure).First();
                return result.HasValue ? result.Value + 1 : 1;
            }
        }

        /// <summary>
        /// Загрузка документа
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseDocument LoadDocument(DocumentType documentType, Guid id)
        {
            return null;
        }

        /// <summary>
        /// Сохранение документа
        /// </summary>
        /// <param name="document"></param>
        public void SaveDocument(BaseDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            if (document is Transportation)
                SaveTransportation(document as Transportation);
            else if (document is Processing)
                SaveProcessing(document as Processing);
            else if (document is Remains)
                SaveRemains(document as Remains);
        }

        private void SaveTransportation(Transportation document)
        {
            //int start = Environment.TickCount;
            //using (ZlatmetEntities context = new ZlatmetEntities())
            //{
            //    Debug.WriteLine("context {0} мс", Environment.TickCount - start);

            //    var documentEntity = context.DocumentTransportation.FirstOrDefault(x => x.Id == document.Id);
            //    if (documentEntity == null)
            //    {
            //        start = Environment.TickCount;
            //        documentEntity = Mapper.Map<Transportation, TransportationEntity>(document);
            //        Debug.WriteLine("Map {0} мс", Environment.TickCount - start);

            //        //documentEntity = new TransportationEntity
            //        //{
            //        //    Id = document.Id,
            //        //    UserId = document.UserId,
            //        //    Type = (int)document.Type,
            //        //    Number = document.Number,
            //        //    Date = document.Date,
            //        //    DateOfLoading = document.DateOfLoading,
            //        //    DateOfUnloading = document.DateOfUnloading,
            //        //    SupplierId = document.Supplier.Id,
            //        //    SupplierDivisionId =
            //        //        document.SupplierDivision != null ? document.SupplierDivision.Id : (Guid?)null
            //        //};

            //        start = Environment.TickCount;
            //        context.DocumentTransportation.Add(documentEntity);
            //        Debug.WriteLine("Add {0} мс", Environment.TickCount - start);

            //        start = Environment.TickCount;
            //        context.SaveChanges();
            //        Debug.WriteLine("SaveChanges {0} мс", Environment.TickCount - start);
            //    }
            //    else
            //    {

            //    }
            //}
        }

        private void SaveProcessing(Processing document)
        {

        }

        private void SaveRemains(Remains document)
        {

        }

    }
}