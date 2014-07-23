using System;
using System.Data;
using Zlatmet2.Core.Tools;

namespace Zlatmet2.Migration.Migrations
{
    [FluentMigrator.Migration(1)]
    public class CreateTables : FluentMigrator.Migration
    {
        public override void Up()
        {
            // Пользователи
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Login").AsString().Unique().NotNullable()
                .WithColumn("Password").AsString(Int32.MaxValue).Nullable();
            Insert.IntoTable("Users")
                .Row(
                    new
                    {
                        Id = Guid.Parse("{C0B709EA-1DC2-41F8-83AF-380D3AA32019}"),
                        Login = "Пользователь",
                        Password = Helpers.Sha1Pass("123")
                    });

            #region Справочники

            // Номенклатура
            Create.Table("ReferenceNomenclatures")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().Unique().NotNullable();

            // Организации
            Create.Table("ReferenceOrganizations")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("Name").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("FullName").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Address").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Phone").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Inn").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Bik").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Bank").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Contract").AsString(Int32.MaxValue).Nullable();

            // Подразделения
            Create.Table("ReferenceDivisions")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("OrganizationId").AsGuid().ForeignKey("ReferenceOrganizations", "Id").OnDelete(Rule.Cascade).NotNullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Name").AsString().NotNullable();

            // Сотрудники
            Create.Table("ReferenceEmployees")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("Name").AsString().Unique().NotNullable()
                .WithColumn("FullName").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Phone").AsString(Int32.MaxValue).Nullable();

            // Транспорт
            Create.Table("ReferenceTransports")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().Unique().NotNullable()
                .WithColumn("Number").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Tara").AsDouble().NotNullable()
                .WithColumn("DriverId").AsGuid().ForeignKey("ReferenceEmployees", "Id").Nullable();

            #endregion

            #region Документы

            Create.Table("DocumentTransportation")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsGuid().Nullable().ForeignKey("Users", "Id").OnDelete(Rule.SetNull)
                .WithColumn("Type").AsInt32().NotNullable().WithColumnDescription("Тип документа")
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("DateOfLoading").AsDate().NotNullable()
                .WithColumn("DateOfUnloading").AsDate().NotNullable()
                .WithColumn("SupplierId").AsGuid().NotNullable()
                .WithColumn("SupplierDivisionId").AsGuid().Nullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("CustomerDivisionId").AsGuid().Nullable()
                .WithColumn("ResponsiblePersonId").AsGuid().NotNullable()
                .WithColumn("TransportId").AsGuid().Nullable()
                .WithColumn("DriverId").AsGuid().Nullable()
                .WithColumn("Wagon").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Psa").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Ttn").AsString(Int32.MaxValue).Nullable()
                .WithColumn("Comment").AsString(Int32.MaxValue).Nullable();

            Create.Table("DocumentTransportationItems")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("DocumentId").AsGuid().NotNullable().ForeignKey("DocumentTransportation", "Id").OnDelete(Rule.Cascade)
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("LoadingNomenclatureId").AsGuid().NotNullable()
                .WithColumn("LoadingWeight").AsDouble().NotNullable()
                .WithColumn("UnloadingNomenclatureId").AsGuid().NotNullable()
                .WithColumn("UnloadingWeight").AsDouble().NotNullable()
                .WithColumn("Netto").AsDouble().NotNullable()
                .WithColumn("Garbage").AsDouble().NotNullable()
                .WithColumn("Price").AsDecimal(9, 2).NotNullable();

            Create.Table("DocumentProcessing")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsGuid().Nullable().ForeignKey("Users", "Id").OnDelete(Rule.SetNull)
                .WithColumn("Type").AsInt32().NotNullable().WithColumnDescription("Тип документа")
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("ResponsiblePersonId").AsGuid().NotNullable()
                .WithColumn("Comment").AsString(Int32.MaxValue).Nullable();

            Create.Table("DocumentProcessingItems")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("DocumentId").AsGuid().NotNullable().ForeignKey("DocumentProcessing", "Id").OnDelete(Rule.Cascade)
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("InputNomenclatureId").AsGuid().NotNullable()
                .WithColumn("InputWeight").AsDouble().NotNullable()
                .WithColumn("OutputNomenclatureId").AsGuid().NotNullable()
                .WithColumn("OutputWeight").AsDouble().NotNullable();

            Create.Table("DocumentRemains")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserId").AsGuid().Nullable().ForeignKey("Users", "Id").OnDelete(Rule.SetNull)
                .WithColumn("Type").AsInt32().NotNullable().WithColumnDescription("Тип документа")
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();

            Create.Table("DocumentRemainsItems")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("DocumentId").AsGuid().NotNullable().ForeignKey("DocumentRemains", "Id").OnDelete(Rule.Cascade)
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("NomenclatureId").AsGuid().NotNullable()
                .WithColumn("Weight").AsDouble().NotNullable();

            #endregion

            // Шаблоны
            Create.Table("Templates")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().Unique().NotNullable()
                .WithColumn("Data").AsBinary(Int32.MaxValue).Nullable();

            // Создание представлений
            Execute.EmbeddedScript("Create_Documents_View.sql");
            Execute.EmbeddedScript("Create_DocumentNumbersAndDates_View.sql");

            // Создание хранимых процедур
            Execute.EmbeddedScript("Create_GetDocuments_Udf.sql");
            Execute.EmbeddedScript("Create_GetMaxDocumentNumber_Udf.sql");
        }

        public override void Down()
        {
            Delete.Table("ReferenceNomenclatures");

            Delete.Table("ReferenceOrganizations");

            Delete.Table("ReferenceDivisions");
        }
    }
}
