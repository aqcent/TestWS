IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UnprofitableMovies]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[UnprofitableMovies]
GO

CREATE PROCEDURE [dbo].[UnprofitableMovies]
	@DateFrom Datetime,
	@DateTo Datetime, 
	@Threshold Money
AS
BEGIN
	SELECT 
		ResultTable.[Name] [MovieName],
		ResultTable.[GuaranteedProfit] [Profit] 
	FROM (
		SELECT 
			mv.Name, 
			SUM(CASE
					WHEN rs.Status = 0 THEN tf.Cost
					WHEN rs.Status = 1 THEN 0
				END) [GuaranteedProfit]
		FROM Movies mv
			JOIN Timeslots ts ON ts.MovieId = mv.Id
			LEFT JOIN Tariffs tf ON tf.Id = ts.TariffId
			LEFT JOIN RequestedSeats rs ON rs.TimeslotId = ts.Id
	
		WHERE 
			ts.StartTime>@DateFrom AND 
			ts.StartTime<@DateTo
		GROUP BY mv.Name) ResultTable
	Where 
		ResultTable.GuaranteedProfit < @Threshold OR 
		ResultTable.GuaranteedProfit IS NULL
END
