CREATE VIEW dbo.DocumentNumbersAndDates 
AS
  SELECT Number, Date FROM DocumentTransportation
UNION ALL
  SELECT Number, Date FROM DocumentProcessing
UNION ALL
  SELECT Number, Date FROM DocumentRemains