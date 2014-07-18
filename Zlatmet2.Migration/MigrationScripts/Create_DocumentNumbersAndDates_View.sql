SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW dbo.DocumentNumbersAndDates 
AS
  SELECT Number, Date FROM DocumentTransportation
UNION ALL
  SELECT Number, Date FROM DocumentProcessing
UNION ALL
  SELECT Number, Date FROM DocumentRemains