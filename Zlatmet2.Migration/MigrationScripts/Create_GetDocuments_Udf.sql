SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetDocuments]
@DateFrom date = null,
@DateTo date = null
AS
BEGIN

DECLARE @CMD nvarchar(max)

SET @CMD = 'SELECT * FROM [Documents]'

IF @DateFrom IS NOT NULL AND @DateTo IS NOT NULL
BEGIN
	SET @CMD = @CMD + ' WHERE CONVERT(date, Date) >= ''' + CAST(CONVERT(date, @DateFrom) as varchar(55)) +
	''' AND CONVERT(date, Date) <= ''' + CAST(Convert(date, @DateTo) as varchar(55)) + ''''
END
ELSE
BEGIN
	IF @DateFrom IS NOT NULL
		SET @CMD = @CMD + ' WHERE CONVERT(date, Date) >= ''' + CAST(Convert(date, @DateFrom) as varchar(55)) + ''''

	IF @DateTo IS NOT NULL
		SET @CMD = @CMD + ' WHERE CONVERT(date, Date) <= ''' + CAST(Convert(date, @DateTo) as varchar(55)) + ''''
END

SET @CMD = @CMD + ' ORDER BY Date'

print @CMD

EXEC sp_executesql @CMD

END