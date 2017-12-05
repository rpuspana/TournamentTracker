USE T

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.spTestPerson_GetByLastName
	@LastName nvarchar(100)
	-- @FirstName char(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.TestPerson
	WHERE LastName = @LastName;
END

GO
