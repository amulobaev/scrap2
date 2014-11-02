IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ReportTransportation')
   exec('CREATE PROCEDURE [dbo].[ReportTransportation] AS BEGIN SET NOCOUNT ON; END')

GO

ALTER PROCEDURE [dbo].[ReportTransportation]
@TransportType nvarchar(max),
@DateFrom date = null,
@DateTo date = null,
@Suppliers nvarchar(max),
@SupplierDivisions nvarchar(max),
@Customers nvarchar(max),
@CustomerDivisions nvarchar(max),
@Nomenclatures nvarchar(max)
AS
BEGIN

IF @DateFrom IS NULL
	SET @DateFrom = '1900-01-01'
IF @DateTo IS NULL
	SET @DateTo = '9999-01-01'

DECLARE @CMD nvarchar(max)

SET @CMD = '
SELECT *
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
'

print @CMD

EXEC sp_executesql @CMD

END