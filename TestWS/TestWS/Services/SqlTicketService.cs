using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestWS.Models.Tickets;
using TestWS.Utils;

namespace TestWS.Services
{
    public class SqlTicketService : ITicketService
    {
        private SQLDatabaseUtil DatabaseUtil { get; set; }

        public SqlTicketService(IMapper mapper)
        {
            DatabaseUtil = new SQLDatabaseUtil(mapper);
        }

        public bool AddRequestedSeatsToTimeslot(SeatsProcessRequest request)
        {
            var requestTable = new DataTable("SeatsRequest");
            requestTable.Columns.Add("Row", typeof(int));
            requestTable.Columns.Add("Seat", typeof(int));

            foreach (var addedSeat in request.SeatsRequest.AddedSeats)
            {
                requestTable.Rows.Add(addedSeat.Row, addedSeat.Seat);
            }

            SqlParameter requestTableParameter = new SqlParameter()
            {
                ParameterName = "@SeatsTable",
                SqlDbType = SqlDbType.Structured,
                Value = requestTable
            };

            var parameters = new[]
            {
                requestTableParameter,
                new SqlParameter("@Status", request.SelectedStatus),
                new SqlParameter("@TimeslotID", request.TimeslotID)
            };

            return DatabaseUtil.ExecuteNonQuery("AddRequestedSeat", parameters) != 0;
        }

        public bool CreateHall(Hall newHall)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", newHall.Name),
                new SqlParameter("@Count", newHall.Count),
                new SqlParameter("@Layout", newHall.Layout)
            };

            return DatabaseUtil.ExecuteNonQuery("AddHall", parameters) != 0;
        }

        public bool CreateMovie(Movie newMovie)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", newMovie.Name),
                new SqlParameter("@Description", newMovie.Description ?? string.Empty),
                new SqlParameter("@MinAge", newMovie.MinAge),
                new SqlParameter("@Duration", newMovie.Duration),
                new SqlParameter("@Rating", newMovie.Rating),
                new SqlParameter("@Types", newMovie.Types != null && newMovie.Types.Any() ? string.Join(",", newMovie.Types) : string.Empty),
                new SqlParameter("@Genres", newMovie.Genres != null && newMovie.Genres.Any() ? string.Join(",", newMovie.Genres) : string.Empty),
                new SqlParameter("@ImageURL", newMovie.ImageURL ?? string.Empty)
            };

            return DatabaseUtil.ExecuteNonQuery("AddMovie", parameters) != 0;
        }

        public bool CreateTariff(Tariff newTariff)
        {
            var parameters = new[]
            {
                new SqlParameter("Cost", newTariff.Cost),
                new SqlParameter("Name", newTariff.Name)
            };

            return DatabaseUtil.ExecuteNonQuery("AddTariff", parameters) != 0;
        }

        public bool CreateTimeSlot(TimeSlot newTimeSlot)
        {
            var parameters = new[]
            {
                new SqlParameter("@StartTime", newTimeSlot.StartTime),
                new SqlParameter("@MovieID", newTimeSlot.MovieID),
                new SqlParameter("@HallID", newTimeSlot.HallID),
                new SqlParameter("@TariffID", newTimeSlot.TariffID),
            };

            return DatabaseUtil.ExecuteNonQuery("AddTimeSlot", parameters) != 0;
        }

        public Hall[] GetAllHalls()
        {
            return DatabaseUtil.Execute<Hall>("SelectAllHalls").ToArray();
        }

        public Movie[] GetAllMovies()
        {
            return DatabaseUtil.Execute<Movie>("SelectAllMovies").ToArray();
        }

        public Tariff[] GetAllTariffs()
        {
            return DatabaseUtil.Execute<Tariff>("SelectAllTariffs").ToArray();
        }

        public TimeSlot[] GetAllTimeSlots()
        {
            return DatabaseUtil.Execute<TimeSlot>("SelectAllTimeSlots").ToArray();
        }

        private readonly Func<SqlDataReader, List<MovieListItem>, IMapper, List<MovieListItem>> extendedReader = (reader, moviesList, mapper) =>
        {
            var currentCollection = new List<MovieListItem>();
            currentCollection.AddRange(moviesList);

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var currentMovieID = (int)reader["MovieID"];
                    var currentMovie = currentCollection.FirstOrDefault(x => x.Movie.Id == currentMovieID);

                    if (currentMovie == null)
                        continue;

                    var currentTimeslotsList = currentMovie.AvailableTimeSlots.ToList();
                    currentTimeslotsList.Add(mapper.Map<TimeSlotTag>(reader));
                    currentMovie.AvailableTimeSlots = currentTimeslotsList.ToArray();
                }
            }
            return currentCollection;
        };

        public MovieListItem[] GetFullMoviesInfo()
        {
            return DatabaseUtil.Execute<MovieListItem>("SelectAllMoviesWithTimeslotTags", null, extendedReader).ToArray();
        }

        public Hall GetHallByID(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", id)
            };

            return DatabaseUtil.Execute<Hall>("SelectHallByID", parameters).FirstOrDefault();
        }

        public Movie GetMovieByID(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", id)
            };

            return DatabaseUtil.Execute<Movie>("SelectMovieByID", parameters).FirstOrDefault();
        }

        public Tariff GetTariffByID(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", id)
            };

            return DatabaseUtil.Execute<Tariff>("SelectTariffByID", parameters).FirstOrDefault();
        }

        public TimeSlot GetTimeSlotByID(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", id)
            };

            Func<SqlDataReader, List<TimeSlot>, IMapper, List<TimeSlot>> extendedReader = (reader, timeslotList, mapper) =>
            {
                var currentCollection = new List<TimeSlot>();
                currentCollection.AddRange(timeslotList);

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var currentTimeslotID = (int)reader["TimeslotID"];
                        var currentTimeslot = currentCollection.FirstOrDefault(x => x.Id == currentTimeslotID);

                        if (currentTimeslot == null)
                            continue;

                        var currentTimeslotsList = currentTimeslot.RequestedSeats.ToList();
                        currentTimeslotsList.Add(mapper.Map<TimeslotSeatRequest>(reader));
                        currentTimeslot.RequestedSeats = currentTimeslotsList.ToArray();
                    }
                }
                return currentCollection;
            };

            return DatabaseUtil.Execute<TimeSlot>("SelectTimeslotByID", parameters, extendedReader).FirstOrDefault();
        }

        public TimeSlot[] GetTimeSlotsByMovieID(int movieID)
        {
            var parameters = new[]
            {
                new SqlParameter("@MovieID", movieID)
            };

            return DatabaseUtil.Execute<TimeSlot>("SelectTimeslotByMovieID", parameters).ToArray();
        }

        public TimeSlotTag[] GetTimeSlotTagsByMovieID(int movieID)
        {
            var parameters = new[]
            {
                new SqlParameter("@MovieID", movieID)
            };

            return DatabaseUtil.Execute<TimeSlotTag>("SelectTimeslotTagsByMovieID", parameters).ToArray();
        }

        public bool RemoveMovie(int removableMovieID)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", removableMovieID)

            };

            var timeslotsList = GetAllTimeSlots();
            var removableTimeslotID = timeslotsList.Where(x => x.MovieID == removableMovieID).Select(x => x.Id);

            if (removableTimeslotID.Any())
                foreach (var currentID in removableTimeslotID)
                    RemoveTimeSlot(currentID);

            return DatabaseUtil.ExecuteNonQuery("DeleteMovie", parameters) != 0;
        }

        public bool RemoveTimeSlot(int removableTimeSlotID)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", removableTimeSlotID)
            };


            DatabaseUtil.ExecuteNonQuery("DeleteRequestedSeatByTimeslotID", new[] { new SqlParameter("@TimeslotID", removableTimeSlotID) });


            return DatabaseUtil.ExecuteNonQuery("DeleteTimeSlot", parameters) != 0;
        }

        public MovieListItem[] SearchFilmByName(string searchRequest)
        {
            Func<SqlDataReader, List<MovieListItem>, IMapper, List<MovieListItem>> _extendedReader = (reader, movieList, mapper) =>
            {
                var currentCollection = new List<MovieListItem>();
                currentCollection.AddRange(movieList);

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var movieID = (int)reader["MovieID"];
                        var currentMovie = currentCollection.FirstOrDefault(x => x.Movie.Id == movieID);                        

                        if (currentMovie == null)
                            continue;

                                                
                        var currentTimeslotsList = currentMovie.AvailableTimeSlots.ToList();
                        currentTimeslotsList.Add(mapper.Map<TimeSlotTag>(reader));
                        currentMovie.AvailableTimeSlots = currentTimeslotsList.ToArray();                                               
                    }
                }
                return currentCollection;
            };

            var a = DatabaseUtil.Execute<MovieListItem>("SearchFilmByName", new[] { new SqlParameter("@string", searchRequest) }, _extendedReader).ToArray();

            return DatabaseUtil.Execute<MovieListItem>("SearchFilmByName", new[] { new SqlParameter("@string", searchRequest) }, _extendedReader).ToArray();
        }

        public bool UpdateHall(Hall updatedHall)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", updatedHall.Id),
                new SqlParameter("@Name", updatedHall.Name),
                new SqlParameter("@Layout", updatedHall.Layout),
                new SqlParameter("@Count", updatedHall.Count)
            };

            return DatabaseUtil.ExecuteNonQuery("UpdateHall", parameters) != 0;
        }

        public bool UpdateMovie(Movie updatedMovie)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", updatedMovie.Id),
                new SqlParameter("@Name", updatedMovie.Name),
                new SqlParameter("@Description", updatedMovie.Description),
                new SqlParameter("@MinAge", updatedMovie.MinAge),
                new SqlParameter("@Duration", updatedMovie.Duration),
                new SqlParameter("@Rating", updatedMovie.Rating),
                new SqlParameter("@Types", updatedMovie.Types != null && updatedMovie.Types.Any() ? string.Join(",", updatedMovie.Types) : string.Empty),
                new SqlParameter("@Genres", updatedMovie.Genres != null && updatedMovie.Genres.Any() ? string.Join(",", updatedMovie.Genres) : string.Empty),
                new SqlParameter("@ImageURL", updatedMovie.ImageURL)
            };

            return DatabaseUtil.ExecuteNonQuery("UpdateMovie", parameters) != 0;
        }

        public bool UpdateTariff(Tariff updatedTariff)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", updatedTariff.Id),
                new SqlParameter("@Name", updatedTariff.Name),
                new SqlParameter("@Cost", updatedTariff.Cost)
            };

            return DatabaseUtil.ExecuteNonQuery("UpdateTariff", parameters) != 0;
        }

        public bool UpdateTimeSlot(TimeSlot updatedTimeSlot)
        {
            var parameters = new[]
            {
                new SqlParameter("@ID", updatedTimeSlot.Id),
                new SqlParameter("@StartTime", updatedTimeSlot.StartTime),
                new SqlParameter("@MovieID", updatedTimeSlot.MovieID),
                new SqlParameter("@HallID", updatedTimeSlot.HallID),
                new SqlParameter("@TariffID", updatedTimeSlot.TariffID),
            };

            return DatabaseUtil.ExecuteNonQuery("UpdateTimeSlots", parameters) != 0;
        }

        public bool RemoveTariff(int tariffID)
        {
            var parameters = new[]
            {
                new SqlParameter("ID", tariffID)
            };

            var timeslotsList = GetAllTimeSlots();
            var removableTimeslotID = timeslotsList.Where(x => x.TariffID == tariffID).Select(x => x.Id);

            if (removableTimeslotID.Any())
                foreach (var currentID in removableTimeslotID)
                    RemoveTimeSlot(currentID);

            return DatabaseUtil.ExecuteNonQuery("DeleteTariff", parameters) != 0;
        }

        public bool RemoveHall(int hallID)
        {
            var parameters = new[]
            {
                new SqlParameter("ID", hallID)
            };

            var timeslotsList = GetAllTimeSlots();
            var removableTimeslotID = timeslotsList.Where(x => x.HallID == hallID).Select(x => x.Id);

            if (removableTimeslotID.Any())
                foreach (var currentID in removableTimeslotID)
                    RemoveTimeSlot(currentID);

            return DatabaseUtil.ExecuteNonQuery("DeleteHall", parameters) != 0;
        }
    }
}