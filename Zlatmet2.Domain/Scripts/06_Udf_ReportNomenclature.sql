CREATE PROCEDURE [dbo].[ReportNomenclature]
@DateFrom date = null,
@DateTo date = null
AS
BEGIN

SELECT Nomenclature, SUM(x.Purchased) AS Purchased, SUM(x.Sold) AS Sold FROM (
-- Перевозка от поставщика на базу
SELECT rn.Name AS Nomenclature, dti.LoadingWeight AS Purchased, 0 AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.LoadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= @DateFrom AND CONVERT(date, dt.DateOfUnloading) >= @DateFrom
	AND CONVERT(date, dt.DateOfLoading) <= @DateTo AND CONVERT(date, dt.DateOfUnloading) <= @DateTo
	AND ro1.Type = 0 AND ro2.Type = 1

UNION ALL

-- Перевозка с базы к заказчику
SELECT rn.Name AS Nomenclature, 0 AS Purchased, dti.UnloadingWeight AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.UnloadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= @DateFrom AND CONVERT(date, dt.DateOfUnloading) >= @DateFrom
	AND CONVERT(date, dt.DateOfLoading) <= @DateTo AND CONVERT(date, dt.DateOfUnloading) <= @DateTo
	AND ro1.Type = 1 AND ro2.Type = 0

UNION ALL

-- Перевозка от поставщика к заказчику (погрузка)
SELECT rn.Name AS Nomenclature, dti.LoadingWeight AS Purchased, 0 AS Sold
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.DocumentId = dt.Id
JOIN [ReferenceOrganizations] ro1 ON dt.SupplierId = ro1.Id
JOIN [ReferenceOrganizations] ro2 ON dt.CustomerId = ro2.Id
JOIN [ReferenceNomenclatures] rn ON dti.LoadingNomenclatureId = rn.Id
WHERE
	CONVERT(date, dt.DateOfLoading) >= @DateFrom AND CONVERT(date, dt.DateOfUnloading) >= @DateFrom
	AND CONVERT(date, dt.DateOfLoading) <= @DateTo AND CONVERT(date, dt.DateOfUnloading) <= @DateTo
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
	CONVERT(date, dt.DateOfLoading) >= @DateFrom AND CONVERT(date, dt.DateOfUnloading) >= @DateFrom
	AND CONVERT(date, dt.DateOfLoading) <= @DateTo AND CONVERT(date, dt.DateOfUnloading) <= @DateTo
	AND ro1.Type = 0 AND ro2.Type = 0
) x GROUP BY Nomenclature ORDER BY Nomenclature

END