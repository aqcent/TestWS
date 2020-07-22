using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TestWS.Extensions;
using TestWS.Models.Tickets;
using Type = TestWS.Models.Tickets.Type;

namespace TestWS.Services
{
    public class JsonTicketService : ITicketService
    {

        private HttpContext Context { get; set; }
        private const string PathToJSON = "/Files/Tickets.json";


        public JsonTicketService()
        {
            Context = HttpContext.Current;
        }

        #region GetMovie
        public Movie GetMovieByID(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.Movies.FirstOrDefault(movie => movie.Id == id);
        }

        public Movie[] GetAllMovies()
        {
            var fullModel = GetDataFromFile();
            return fullModel.Movies;
        }
        #endregion

        #region GetHall
        public Hall GetHallByID(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.Halls.FirstOrDefault(hall => hall.Id == id);
        }
        public Hall[] GetAllHalls()
        {
            var fullModel = GetDataFromFile();
            return fullModel.Halls;
        }
        #endregion

        #region GetTarif
        public Tariff GetTariffByID(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.Tariffs.FirstOrDefault(tariff => tariff.Id == id);
        }
        public Tariff[] GetAllTariffs()
        {
            var fullModel = GetDataFromFile();
            return fullModel.Tariffs;
        }
        #endregion

        #region GetTimeSlot
        public TimeSlot GetTimeSlotByID(int id)
        {
            var fullModel = GetDataFromFile();
            return fullModel.TimeSlots.FirstOrDefault(timeslot => timeslot.Id == id);
        }
        public TimeSlot[] GetAllTimeSlots()
        {
            var fullModel = GetDataFromFile();
            return fullModel.TimeSlots;
        }
        public TimeSlot[] GetTimeSlotsByMovieID(int movieID)
        {
            var fullModel = GetDataFromFile();
            var resoultList = fullModel.TimeSlots.Where(x => x.MovieID == movieID).ToArray();
            return resoultList;
        }
        #endregion


        private TicketsJSONModel GetDataFromFile()
        {
            var jsonfilepath = Context.Server.MapPath(PathToJSON);

            if (!System.IO.File.Exists(jsonfilepath))
                return null;

            var jsonModel = System.IO.File.ReadAllText(jsonfilepath);
            var deserializedModel = JsonConvert.DeserializeObject<TicketsJSONModel>(jsonModel);

            return deserializedModel;
        }
        private void SaveDataToFile(TicketsJSONModel fullmodel)
        {
            var jsonFilePath = Context.Server.MapPath(PathToJSON);
            var serrializedModel = JsonConvert.SerializeObject(fullmodel);
            System.IO.File.WriteAllText(jsonFilePath, serrializedModel);
        }

        public bool UpdateMovie(Movie updatedMovie)
        {
            var fullmodel = GetDataFromFile();
            var movieToUpdate = fullmodel.Movies.FirstOrDefault(movie => movie.Id == updatedMovie.Id);
            if (movieToUpdate == null)
                return false;

            if (updatedMovie.Genres != null)
                movieToUpdate.Genres = updatedMovie.Genres;
            if (updatedMovie.Types != null)
                movieToUpdate.Types = updatedMovie.Types;

            movieToUpdate.Description = updatedMovie.Description;
            movieToUpdate.Duration = updatedMovie.Duration;
            movieToUpdate.ImageURL = updatedMovie.ImageURL;
            movieToUpdate.MinAge = updatedMovie.MinAge;
            movieToUpdate.Name = updatedMovie.Name;
            movieToUpdate.Rating = updatedMovie.Rating;


            SaveDataToFile(fullmodel);
            return true;
        }
        public bool UpdateHall(Hall updatedHall)
        {
            var fullmodel = GetDataFromFile();
            var hallToUpdate = fullmodel.Halls.FirstOrDefault(hall => hall.Id == updatedHall.Id);
            if (hallToUpdate == null)
                return false;

            hallToUpdate.Name = updatedHall.Name;
            SaveDataToFile(fullmodel);
            return true;
        }
        public bool UpdateTariff(Tariff updatedTariff)
        {
            var fullmodel = GetDataFromFile();
            var updateToTariff = fullmodel.Tariffs.FirstOrDefault(tariff => tariff.Id == updatedTariff.Id);
            if (updatedTariff == null)
                return false;

            updateToTariff.Name = updatedTariff.Name;
            updateToTariff.Cost = updatedTariff.Cost;
            SaveDataToFile(fullmodel);
            return true;
        }
        public bool UpdateTimeSlot(TimeSlot updatedTimeSlot)
        {
            var fullmodel = GetDataFromFile();
            var updateToTimeSlot = fullmodel.TimeSlots.FirstOrDefault(timeslot => timeslot.Id == updatedTimeSlot.Id);
            if (updatedTimeSlot == null)
                return false;

            updateToTimeSlot.HallID = updatedTimeSlot.HallID;
            updateToTimeSlot.MovieID = updatedTimeSlot.MovieID;
            updateToTimeSlot.StartTime = updatedTimeSlot.StartTime;
            updateToTimeSlot.TariffID = updatedTimeSlot.TariffID;
            SaveDataToFile(fullmodel);
            return true;
        }

        public bool CreateMovie(Movie newMovie)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var newMovieID = 0;
                if (fullmodel.Movies != null && fullmodel.Movies.Any())
                    newMovieID = fullmodel.Movies.Max(x => x.Id) + 1;
                newMovie.Id = newMovieID;

                var existingMoviesList = fullmodel.Movies.ToList();
                existingMoviesList.Add(newMovie);

                fullmodel.Movies = existingMoviesList.ToArray();
                SaveDataToFile(fullmodel);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool CreateTimeSlot(TimeSlot newTimeSlot)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var newTimeSlotID = fullmodel.TimeSlots.Max(x => x.Id) + 1;
                newTimeSlot.Id = newTimeSlotID;

                var existingMoviesList = fullmodel.TimeSlots.ToList();
                existingMoviesList.Add(newTimeSlot);
                fullmodel.TimeSlots = existingMoviesList.ToArray();
                SaveDataToFile(fullmodel);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool CreateHall(Hall newHall)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var newHallID = fullmodel.Halls.Max(x => x.Id) + 1;
                newHall.Id = newHallID;

                var existingHallsList = fullmodel.Halls.ToList();
                existingHallsList.Add(newHall);
                fullmodel.Halls = existingHallsList.ToArray();

                SaveDataToFile(fullmodel);
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool CreateTariff(Tariff newTariff)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var newTariffID = 0;
                if (fullmodel.Tariffs != null && fullmodel.Tariffs.Any())
                    newTariffID = fullmodel.Tariffs.Max(x => x.Id) + 1;
                newTariff.Id = newTariffID;

                var existingTariffsList = fullmodel.Tariffs.ToList();
                existingTariffsList.Add(newTariff);
                fullmodel.Tariffs = existingTariffsList.ToArray();

                SaveDataToFile(fullmodel);
            }
            catch { return false; }
            return true;
        }

        public bool RemoveTimeSlot(int removableTimeSlotID)
        {
            var fullmodel = GetDataFromFile();

            try
            {
                var resoultTimeslots = fullmodel.TimeSlots.Where(x => x.Id != removableTimeSlotID);
                fullmodel.TimeSlots = resoultTimeslots.ToArray();
                SaveDataToFile(fullmodel);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveMovie(int removableMovieID)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var resoultMovies = fullmodel.Movies.Where(x => x.Id != removableMovieID);
                fullmodel.Movies = resoultMovies.ToArray();

                var resoultTimeSlots = fullmodel.TimeSlots.ToList();
                resoultTimeSlots.RemoveAll(x => x.MovieID == removableMovieID);
                fullmodel.TimeSlots = resoultTimeSlots.ToArray();

                SaveDataToFile(fullmodel);
            }
            catch { return false; }

            return true;
        }

        public MovieListItem[] GetFullMoviesInfo()
        {
            var allMovies = GetAllMovies();
            var resoultModel = new List<MovieListItem>();
            foreach (var movie in allMovies)
            {
                //movie timeslottag[]
                resoultModel.Add(new MovieListItem
                {
                    Movie = movie,
                    AvailableTimeSlots = GetTimeSlotTagsByMovieID(movie.Id)
                });
            }
            return resoultModel.ToArray();
        }

        public TimeSlotTag[] GetTimeSlotTagsByMovieID(int movieID)
        {
            var timeslots = GetTimeSlotsByMovieID(movieID);
            var tariffs = GetAllTariffs();

            var resoultModel = new List<TimeSlotTag>();
            foreach (var timeslot in timeslots)
            {
                resoultModel.Add(new TimeSlotTag
                {
                    TimeSlotID = timeslot.Id,
                    StartTime = timeslot.StartTime,
                    Cost = tariffs.FirstOrDefault(x => x.Id == timeslot.TariffID)?.Cost ?? 0
                });
            }
            return resoultModel.ToArray();
        }

        public bool AddRequestedSeatsToTimeslot(SeatsProcessRequest request)
        {
            var fullmodel = GetDataFromFile();
            var timeslotToUpdate = fullmodel.TimeSlots.FirstOrDefault(timeslot => timeslot.Id == request.TimeslotID);

            if (timeslotToUpdate == null)
                return false;

            List<TimeslotSeatRequest> requestToProcess;
            if (timeslotToUpdate.RequestedSeats != null && timeslotToUpdate.RequestedSeats.Any())
                requestToProcess = timeslotToUpdate.RequestedSeats.ToList();
            else
                requestToProcess = new List<TimeslotSeatRequest>();

            if (request?.SeatsRequest?.AddedSeats != null && request.SeatsRequest.AddedSeats.Any())           
            {
                foreach (var addedSeats in request.SeatsRequest.AddedSeats)
                {
                    requestToProcess.Add(new TimeslotSeatRequest
                    {
                        Row = addedSeats.Row,
                        Seat = addedSeats.Seat,
                        Status = request.SelectedStatus
                    });
                }

                timeslotToUpdate.RequestedSeats = requestToProcess.ToArray();
                SaveDataToFile(fullmodel);
                return true;
            }

            return false;
        }

        public MovieListItem[] SearchFilmByName(string searchRequest)
        {
            var allMovies = GetFullMoviesInfo();
            var searchResult = allMovies.Where(x => x.Movie.Name.CompareIgnoreCase(searchRequest));
            return searchResult.ToArray();            
        }

        public bool RemoveTariff(int tariffID)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var result = fullmodel.Tariffs.Where(x => x.Id != tariffID);
                fullmodel.Tariffs = result.ToArray();

                var resoultTimeSlots = fullmodel.TimeSlots.ToList();
                resoultTimeSlots.RemoveAll(x => x.TariffID == tariffID);
                fullmodel.TimeSlots = resoultTimeSlots.ToArray();

                SaveDataToFile(fullmodel);
            }
            catch { return false; }

            return true;
        }

        public bool RemoveHall(int hallID)
        {
            var fullmodel = GetDataFromFile();
            try
            {
                var result = fullmodel.Halls.Where(x => x.Id != hallID);
                fullmodel.Halls = result.ToArray();

                var resoultTimeSlots = fullmodel.TimeSlots.ToList();
                resoultTimeSlots.RemoveAll(x => x.HallID == hallID);
                fullmodel.TimeSlots = resoultTimeSlots.ToArray();

                SaveDataToFile(fullmodel);
            }
            catch { return false; }

            return true;
        }
    }
}