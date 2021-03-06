IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PotentialRealProfit]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[PotentialRealProfit]
GO
CREATE PROCEDURE [dbo].[PotentialRealProfit]
	@DateFrom Datetime,
	@DateTo Datetime 
AS
BEGIN
	SELECT 
		mv.Name, 
		SUM(CASE
				WHEN rs.Status = 0 THEN tf.Cost
				WHEN rs.Status = 1 THEN 0
			END) [GuaranteedProfit],
		SUM(CASE
				WHEN rs.Status = 0 THEN 0
				WHEN rs.Status = 1 THEN tf.Cost
			END) [PotentialProfit]
	FROM Movies mv
		JOIN Timeslots ts ON ts.MovieId = mv.Id
		LEFT JOIN Tariffs tf ON tf.Id = ts.TariffId
		LEFT JOIN RequestedSeats rs ON rs.TimeslotId = ts.Id
	
	WHERE 
		ts.StartTime>@DateFrom AND 
		ts.StartTime<@DateTo

	GROUP BY mv.Name
END
