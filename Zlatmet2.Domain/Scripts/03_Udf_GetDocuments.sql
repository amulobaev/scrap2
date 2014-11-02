-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDocuments]
@DateFrom date = null,
@DateTo date = null
AS
BEGIN

IF @DateFrom IS NULL
	SET @DateFrom = '1900-01-01'
IF @DateTo IS NULL
	SET @DateTo = '9999-01-01'

SELECT * FROM [Documents]
	WHERE CONVERT(date, Date) >= CONVERT(date, @DateFrom)
		  AND CONVERT(date, Date) <= CONVERT(date, @DateTo)
	ORDER BY Date

END