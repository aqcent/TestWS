using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWS.Models.Tickets;

namespace TestWS.Services
{
    public interface ITicketService
    {
        Movie GetMovieByID(int id);
        Movie[] GetAllMovies();
        bool UpdateMovie(Movie updatedMovie);
        bool CreateMovie(Movie newMovie);
        bool RemoveMovie(int removableMovieID);

        Hall GetHallByID(int id);
        Hall[] GetAllHalls();
        bool UpdateHall(Hall updatedHall);
        bool CreateHall(Hall newHall);

        Tariff GetTariffByID(int id);
        Tariff[] GetAllTariffs();
        bool UpdateTariff(Tariff updatedTariff);
        bool CreateTariff(Tariff newTariff);

        TimeSlot GetTimeSlotByID(int id);
        TimeSlot[] GetAllTimeSlots();
        TimeSlot[] GetTimeSlotsByMovieID(int movieID);
        bool UpdateTimeSlot(TimeSlot updatedTimeSlot);
        bool CreateTimeSlot(TimeSlot newTimeSlot);
        bool RemoveTimeSlot(int removableTimeSlotID);

        MovieListItem[] GetFullMoviesInfo();
        TimeSlotTag[] GetTimeSlotTagsByMovieID(int movieID);
        bool AddRequestedSeatsToTimeslot(SeatsProcessRequest request);
        MovieListItem[] SearchFilmByName(string searchRequest);
        bool RemoveTariff(int tariffID);
        bool RemoveHall(int hallID);
    }
}
