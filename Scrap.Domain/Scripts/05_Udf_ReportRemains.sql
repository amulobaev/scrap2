--
-- Отчёт "Остатки на базе"
--

--IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ReportRemains')
--   exec('CREATE PROCEDURE [dbo].[ReportRemains] AS BEGIN SET NOCOUNT ON; END')

--GO

CREATE PROCEDURE [dbo].[ReportRemains]
@Date date,
@Base uniqueidentifier,
@Nomenclatures nvarchar(max)
AS
BEGIN
DECLARE @CMD nvarchar(max)

SET @CMD = '
SELECT t1.Name AS Nomenclature, t2.Weight AS Weight FROM [ReferenceNomenclatures] t1
LEFT JOIN (
SELECT NomenclatureId, SUM(x.Weight) AS Weight FROM (
-- Перевозки на базу
SELECT rn.[Id] AS NomenclatureId, dti.[UnloadingWeight] AS Weight
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.[DocumentId] = dt.[Id]
JOIN [ReferenceNomenclatures] rn ON dti.[UnloadingNomenclatureId] = rn.[Id]
WHERE CONVERT(date, dt.[DateOfLoading]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) +'''
 AND CONVERT(date, dt.[DateOfUnloading]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) + '''
 AND dt.[CustomerId] = ''' + CAST(@Base as varchar(255)) + '''
 AND dti.[UnloadingNomenclatureId] IN (' + @Nomenclatures + ')
UNION ALL

-- Перевозки с базы
SELECT rn.[Id] AS NomenclatureId, -dti.[LoadingWeight] AS Weight
FROM [DocumentTransportationItems] dti
JOIN [DocumentTransportation] dt ON dti.[DocumentId] = dt.[Id]
JOIN [ReferenceNomenclatures] rn ON dti.[LoadingNomenclatureId] = rn.[Id]
WHERE CONVERT(date, dt.[DateOfLoading]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) +'''
 AND CONVERT(date, dt.[DateOfUnloading]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) + '''
 AND dt.[SupplierId] = ''' + CAST(@Base as varchar(255)) + '''
 AND dti.[LoadingNomenclatureId] IN (' + @Nomenclatures + ')
UNION ALL

-- Переработка (вход)
SELECT rn.[Id] AS NomenclatureId, -dpi.[InputWeight] AS Weight
FROM [DocumentProcessingItems] dpi
JOIN [DocumentProcessing] dp ON dpi.[DocumentId] = dp.[Id]
JOIN [ReferenceNomenclatures] rn ON dpi.[InputNomenclatureId] = rn.[Id]
WHERE CONVERT(date, dp.[Date]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) +'''
 AND dp.[BaseId] = ''' + CAST(@Base as varchar(255)) + '''
 AND dpi.[InputNomenclatureId] IN (' + @Nomenclatures + ')
UNION ALL

-- Переработка (выход)
SELECT rn.[Id] AS NomenclatureId, dpi.[OutputWeight] AS Weight
FROM [DocumentProcessingItems] dpi
JOIN [DocumentProcessing] dp ON dpi.[DocumentId] = dp.[Id]
JOIN [ReferenceNomenclatures] rn ON dpi.[OutputNomenclatureId] = rn.[Id]
WHERE CONVERT(date, dp.[Date]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) +'''
 AND dp.[BaseId] = ''' + CAST(@Base as varchar(255)) + '''
 AND dpi.[OutputNomenclatureId] IN (' + @Nomenclatures + ')
UNION ALL

-- Корректировка остатков
SELECT rn.[Id] AS NomenclatureId, dri.[Weight] AS Weight
FROM [DocumentRemainsItems] dri
JOIN [DocumentRemains] dr ON dri.[DocumentId] = dr.[Id]
JOIN [ReferenceNomenclatures] rn ON dri.[NomenclatureId] = rn.[Id]
WHERE CONVERT(date, dr.[Date]) <= ''' + CAST(CONVERT(date, @Date) as varchar(255)) +'''
 AND dr.[BaseId] = ''' + CAST(@Base as varchar(255)) + '''
 AND dri.[NomenclatureId] IN (' + @Nomenclatures + ')
) x GROUP BY NomenclatureId
) t2 ON t1.Id = t2.NomenclatureId
WHERE t1.Id IN (' + @Nomenclatures + ')
ORDER BY t1.Name'

print @CMD

EXEC sp_executesql @CMD

END
