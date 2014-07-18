SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW dbo.Documents 
AS
  SELECT doc.Id, doc.UserId as UserId, doc.Type AS Type, doc.Number, Date, org1.Name AS Supplier,
  org2.Name AS Customer, emp.Name AS ResponsiblePerson, Psa,
  nom.Name AS Nomenclature, ti.Netto,
  Comment FROM DocumentTransportation doc
  LEFT JOIN [ReferenceOrganizations] org1 ON doc.SupplierId = org1.id
  LEFT JOIN [ReferenceOrganizations] org2 ON doc.CustomerId = org2.id
  LEFT JOIN [ReferenceEmployees] emp ON doc.ResponsiblePersonId = emp.Id
  LEFT JOIN [DocumentTransportationItems] ti ON doc.Id = ti.DocumentId AND ti.Number = 1
  LEFT JOIN [ReferenceNomenclatures] nom ON ti.UnloadingNomenclatureId = nom.Id
UNION ALL
  SELECT doc.Id, doc.UserId as UserId, doc.Type AS Type, doc.Number, Date, NULL AS Supplier, NULL AS Customer,
  emp.Name AS ResponsiblePerson, NULL AS Psa,
  nom.Name AS Nomenclature, NULL AS Netto,
  Comment FROM DocumentProcessing doc
  LEFT JOIN [ReferenceEmployees] emp ON doc.ResponsiblePersonId = emp.Id
  LEFT JOIN [DocumentTransportationItems] ti ON doc.Id = ti.DocumentId AND ti.Number = 1
  LEFT JOIN [ReferenceNomenclatures] nom ON ti.UnloadingNomenclatureId = nom.Id
UNION ALL
  SELECT doc.Id, doc.UserId as UserId, doc.Type AS Type, doc.Number, Date, NULL AS Supplier, NULL AS Customer, 
  NULL AS ResponsiblePerson, NULL AS Psa,
  NULL AS Nomenclature, NULL AS Netto,
  NULL AS Comment FROM DocumentRemains doc