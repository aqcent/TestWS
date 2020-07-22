IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddMovie]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[AddMovie]
GO
CREATE PROCEDURE [dbo].[AddMovie] 
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
	INSERT INTO Movies	(
	[Name],
	[Description],
	MinAge,
	Duration,
	Rating,
	[Types],
	Genres,
	ImageUrl)
	VALUES(
	@Name,
	@Description,
	@MinAge,
	@Duration,
	@Rating,
	@Types,
	@Genres,
	@ImageUrl)
END
