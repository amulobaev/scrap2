
ALTER PROCEDURE [dbo].[ReportAutoTransport]
@DateFrom date = null,
@DateTo date = null,
@Transports nvarchar(max)
AS
BEGIN
	IF @DateFrom IS NULL
		SET @DateFrom = '1900-01-01'
	IF @DateTo IS NULL
		SET @DateTo = '9999-01-01'

	DECLARE @CMD nvarchar(max)
	DECLARE @BasesCmd nvarchar(max)
	DECLARE @TransitCmd nvarchar(max)

	SET @CMD = '
	SELECT Name, SUM(x.LoadingWeight) AS LoadingWeight, SUM(x.UnloadingWeight) AS UnloadingWeight, SUM(x.Netto) AS Netto FROM (
	SELECT rt.name, dti.LoadingWeight, dti.UnloadingWeight, dti.Netto
	FROM [DocumentTransportationItems] dti
	JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
	JOIN [ReferenceTransports] rt ON dt.TransportId = rt.Id
	WHERE
		CONVERT(date, dt.DateOfLoading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
		AND CONVERT(date, dt.DateOfUnloading) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(255)) + '''
		AND CONVERT(date, dt.DateOfLoading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
		AND CONVERT(date, dt.DateOfUnloading) <= ''' + CAST(CONVERT(date, @DateTo) as varchar(255)) + '''
		AND dt.Type = 3
		AND dt.[TransportId] IN (' + @Transports + ')
	) x GROUP BY Name ORDER BY Name
	'

print @CMD

EXEC sp_executesql @CMD

END
