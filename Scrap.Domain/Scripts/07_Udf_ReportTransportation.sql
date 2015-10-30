--
-- Отчёт "Перевозки"
--

ALTER PROCEDURE [dbo].[ReportTransportation]
@IsAuto int,
@IsTrain int,
@DateFrom date = NULL,
@DateTo date = NULL,
@ReportType int,
@SupplierDivisions nvarchar(max) = NULL,
@CustomerDivisions nvarchar(max) = NULL,
@Nomenclatures nvarchar(max),
@Transports nvarchar(max)
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
	-- Период
	CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
	-- Номенклатура
	AND dti.[LoadingNomenclatureId] IN (' + @Nomenclatures + ')'

-- Тип перевозок
IF @IsAuto = 1 AND @IsTrain = 1
	SET @CMD = @CMD + '
		AND ((dt.Type = 3 AND dt.[TransportId] IN (' + @Transports + ')) OR dt.Type = 4)'
ELSE
BEGIN
	IF @IsAuto = 1
		SET @CMD = @CMD + '
			AND dt.Type = 3 AND dt.[TransportId] IN (' + @Transports + ')'
	IF @IsTrain = 1
		SET @CMD = @CMD + '
			AND dt.Type = 4'
END

IF @ReportType = 0
BEGIN
	IF @SupplierDivisions IS NOT NULL
			SET @CMD = @CMD + ' AND (dt.SupplierDivisionId IN (' + @SupplierDivisions + ') OR dt.SupplierId IN (' + @SupplierDivisions + ') OR dt.CustomerDivisionId IN (' + @SupplierDivisions + ') OR dt.CustomerId IN (' + @SupplierDivisions + '))'
END
ELSE
BEGIN
-- Поставщики
IF @SupplierDivisions IS NOT NULL
	SET @CMD = @CMD + ' AND (dt.SupplierDivisionId IN (' + @SupplierDivisions + ') OR dt.SupplierId IN (' + @SupplierDivisions + '))'
-- Заказчики
IF @CustomerDivisions IS NOT NULL
	SET @CMD = @CMD + ' AND (dt.CustomerDivisionId IN (' + @CustomerDivisions + ') OR dt.CustomerId IN (' + @CustomerDivisions + '))'
END

SET @CMD = @CMD + '
	ORDER BY dt.Date'

print @CMD

EXEC sp_executesql @CMD

END