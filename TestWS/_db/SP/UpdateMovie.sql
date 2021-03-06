IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMovie]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[UpdateMovie]
GO
CREATE PROCEDURE [dbo].[UpdateMovie]
	@Id int, 
	@Name nvarchar(100),
	@Description nvarchar(MAX),
	@MinAge int,
	@Duration int,
	@Rating float,
	@Types nvarchar(20),
	@Genres nvarchar(20),
	@ImageUrl nvarchar(400)
AS
BEGIN
	UPDATE 
		Movies 
	SET
		[Name] = @Name,
		[Description] = @Description,
		MinAge = @MinAge,
		Duration = @Duration,
		Rating = @Rating,
		[Types] = @Types,
		Genres = @Genres,
		ImageUrl = @ImageUrl
	WHERE 
		Id = @Id
END
