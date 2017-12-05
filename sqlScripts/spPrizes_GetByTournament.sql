-- use this database
USE [T]
GO

-- =============================================
-- Author:		Radu Puspana
-- Create date: 26 November 2017
-- Description:	Gets all the prizes for a given tournament
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- procedure name
ALTER PROCEDURE [dbo].[spPrizes_GetByTournament]
	-- procedure parameter
	@TournamentId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- do the join, and from the join result, select the row with TournamentId = 1
	SELECT p.*
	from dbo.Prizes p
	inner join dbo.TournamentPrizes t on p.id = t.PrizeId
	where t.TournamentId = 1;
END