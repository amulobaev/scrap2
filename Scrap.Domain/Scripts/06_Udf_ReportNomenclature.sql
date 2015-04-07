ALTER PROCEDURE [dbo].[ReportNomenclature]
@DateFrom date = null,
@DateTo date = null,
@IsBases int,
@Bases nvarchar(max),
@IsTransit int
AS
BEGIN

IF @DateFrom IS NULL
	SET @DateFrom = '1900-01-01'
IF @DateTo IS NULL
	SET @DateTo = '9999-01-01'

DECLARE @CMD nvarchar(max)
DECLARE @BasesCmd nvarchar(max)
DECLARE @TransitCmd nvarchar(max)

SET @CMD = 'SELECT Nomenclature, SUM(x.Purchased) AS Purchased, SUM(x.Sold) AS Sold FROM ('

SET @BasesCmd = '
-- Перевозка от поставщика на базу
SELECT rn.Name AS Nomenclature, dti.LoadingWeight AS Purchased, 0 AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.LoadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND ro1.Type = 0 AND ro2.Type = 1
	AND dt.[CustomerId] IN (' + @Bases + ')
UNION ALL

-- Перевозка с базы к заказчику
SELECT rn.Name AS Nomenclature, 0 AS Purchased, dti.UnloadingWeight AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.UnloadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND ro1.Type = 1 AND ro2.Type = 0
	AND dt.[SupplierId] IN (' + @Bases + ')'

SET @TransitCmd = '
-- Перевозка от поставщика к заказчику (погрузка)
SELECT rn.Name AS Nomenclature, dti.LoadingWeight AS Purchased, 0 AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.LoadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND ro1.Type = 0 AND ro2.Type = 0

UNION ALL

-- Перевозка от поставщика к заказчику (разгрузка)
SELECT rn.Name AS Nomenclature, 0 AS Purchased, dti.UnloadingWeight AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.UnloadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND ro1.Type = 0 AND ro2.Type = 0'

IF @IsBases = 1 AND @IsTransit = 1
	SET @CMD = @CMD + @BasesCmd +'
	UNION ALL
	' + @TransitCmd
ELSE
BEGIN
IF @IsBases = 1
	SET @CMD = @CMD + @BasesCmd
IF @IsTransit = 1
	SET @CMD = @CMD + @TransitCmd
END

SET @CMD = @CMD + '
) x GROUP BY Nomenclature ORDER BY Nomenclature'

print @CMD

EXEC sp_executesql @CMD

END