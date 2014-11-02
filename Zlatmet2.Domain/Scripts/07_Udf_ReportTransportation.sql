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
SELECT dt.Type AS TransportTypeData, dt.DateOfLoading AS DateData, rn.Name AS Nomenclature,
	ro1.Name AS SupplierData, rd1.Name AS SupplierDivisionData, ro2.Name AS CustomerData, rd2.Name AS CustomerDivisionData,
	rt.Name AS TransportName, rt.Number AS TransportNumber, dt.Wagon, 
	dti.LoadingWeight, dti.UnloadingWeight, dti.Netto, dt.Comment
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceNomenclatures] rn ON dti.LoadingNomenclatureId = rn.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
LEFT JOIN [ReferenceDivisions] rd1 ON dt.SupplierDivisionId = rd1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
LEFT JOIN [ReferenceDivisions] rd2 ON dt.CustomerDivisionId = rd2.Id
LEFT JOIN [ReferenceTransports] rt ON dt.[TransportId] = rt.[Id]
WHERE
	-- Тип перевозок
	dt.[Type] IN (' + @TransportType + ')
	-- Период
	AND CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	-- Поставщики
	AND dt.SupplierId IN (' + @Suppliers + ')
	AND (dt.SupplierDivisionId IN (' + @SupplierDivisions + ') OR dt.SupplierDivisionId IS NULL)
	-- Заказчики
	AND dt.CustomerId IN (' + @Customers + ')
	AND (dt.CustomerDivisionId IN (' + @CustomerDivisions + ') OR dt.CustomerDivisionId IS NULL)
	-- Номенклатура
	AND dti.[LoadingNomenclatureId] IN (' + @Nomenclatures + ')
	ORDER BY dt.Date
'

print @CMD

EXEC sp_executesql @CMD

END