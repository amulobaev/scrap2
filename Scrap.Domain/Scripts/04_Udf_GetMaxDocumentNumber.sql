CREATE PROCEDURE [dbo].[GetMaxDocumentNumber]
@Year int
AS
BEGIN
  SELECT MAX(Number) FROM DocumentNumbersAndDates WHERE YEAR(Date) = @Year
END
