IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteMovie])') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteMovie] -- drop proc, if exist
GO
CREATE PROCEDURE DeleteMovie
	-- Add the parameters for the stored procedure here
	@ID int	
AS
BEGIN
	DELETE Movies		
	WHERE Id = @ID
END
GO
