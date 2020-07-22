CREATE TRIGGER RequestedSeatsDuplicatesCheck ON dbo.RequestedSeats
INSTEAD OF INSERT
AS

if exists ( select * from RequestedSeats rs 
    inner join inserted i on 
		i.Seat = rs.Seat and 
		i.[Row] = rs.[Row] and 
		i.[TimeslotId] = rs.TimeslotId)
begin
    rollback
    RAISERROR ('Request for sold seat detected', 16, 1);
end
ELSE
BEGIN
	INSERT INTO RequestedSeats ([Row],[Seat],[Status],TimeslotId)
	SELECT i.[Row],i.Seat,i.[Status],i.[TimeslotId]
	FROM inserted i
END
go