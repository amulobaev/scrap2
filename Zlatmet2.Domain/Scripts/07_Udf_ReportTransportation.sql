CREATE PROCEDURE [dbo].[ReportTransportation]
@IsAuto BIT,
@IsTrain BIT,
@DateFrom date = null,
@DateTo date = null,
@Suppliers nvarchar(max),
@SupplierDivisions nvarchar(max),
@Customers nvarchar(max),
@CustomerDivisions nvarchar(max),
@Nomenclatures nvarchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT <@Param1, sysname, @p1>, <@Param2, sysname, @p2>
END
GO
