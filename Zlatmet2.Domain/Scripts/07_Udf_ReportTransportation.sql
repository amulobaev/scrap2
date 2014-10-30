IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ReportTransportation')
   exec('CREATE PROCEDURE [dbo].[ReportTransportation] AS BEGIN SET NOCOUNT ON; END')

GO

ALTER PROCEDURE [dbo].[ReportTransportation]
@IsAuto BIT,
@IsTrain BIT,
@DateFrom date = null,
@DateTo date = null,
@Suppliers nvarchar(max),
@SupplierDivisions nvarchar(max),
@Customers nvarchar(max),
@CustomerDivisions nvarchar(max),
@Nomenclatures nvarchar(max)
AS
BEGIN
DECLARE @CMD nvarchar(max)

print @CMD

EXEC sp_executesql @CMD
END

