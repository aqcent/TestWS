using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestWS.Models.Tickets;
using TestWS.Reports;

namespace TestWS.Profiles
{
    public class SqlDataReaderProfile : Profile
    {
        public SqlDataReaderProfile()
        {
            CreateMap<SqlDataReader, Movie>()
                .ForMember(x => x.Id, x => x.MapFrom(z => z["Id"]))
                .ForMember(x => x.Name, x => x.MapFrom(z => z["Name"]))
                .ForMember(x => x.MinAge, x => x.MapFrom(z => z["MinAge"]))
                .ForMember(x => x.Description, x => x.MapFrom(z => z["Description"]))
                .ForMember(x => x.Duration, x => x.MapFrom(z => z["Duration"]))
                .ForMember(x => x.Rating, x => x.MapFrom(z => z["Rating"]))
                .ForMember(x => x.ImageURL, x => x.MapFrom(z => z["ImageURL"]))
                .ForMember(x => x.Types, x => x.Ignore())
                .ForMember(x => x.Genres, x => x.Ignore())
                .AfterMap((reader, movie) =>
                {
                    var types = (string)reader["Types"];
                    if (!string.IsNullOrEmpty(types))
                    {
                        var parsedTypes = types.Split(',').Select(x => (Models.Tickets.Type)Enum.Parse(typeof(Models.Tickets.Type), x));
                        movie.Types = parsedTypes.ToArray();
                    }
                    else
                    {
                        movie.Types = new Models.Tickets.Type[] { };
                    }

                    var genres = (string)reader["Genres"];
                    if (!string.IsNullOrEmpty(genres))
                    {
                        var parsedGenres = genres.Split(',').Select(x => (Models.Tickets.Genre)Enum.Parse(typeof(Models.Tickets.Genre), x));
                        movie.Genres = parsedGenres.ToArray();
                    }
                    else
                    {
                        movie.Genres = new Models.Tickets.Genre[] { };
                    }
                });


            CreateMap<SqlDataReader, TimeSlotTag>()
                .ForMember(x => x.Cost, x => x.MapFrom(z => z["Cost"]))
                .ForMember(x => x.StartTime, x => x.MapFrom(z => z["StartTime"]))
                .ForMember(x => x.TimeSlotID, x => x.MapFrom(z => z["TimeSlotID"]));


            CreateMap<SqlDataReader, MovieListItem>()
                .ForMember(x => x.Movie, x => x.MapFrom(z => z))
                .ForMember(x => x.AvailableTimeSlots, x => x.Ignore());


            CreateMap<SqlDataReader, TimeSlot>()
                .ForMember(x => x.HallID, x => x.MapFrom(z => z["HallID"]))
                .ForMember(x => x.MovieID, x => x.MapFrom(z => z["MovieID"]))
                .ForMember(x => x.TariffID, x => x.MapFrom(z => z["TariffID"]))
                .ForMember(x => x.StartTime, x => x.MapFrom(z => z["StartTime"]))
                .ForMember(x => x.Id, x => x.MapFrom(z => z["Id"]))
                .ForMember(x => x.RequestedSeats, x => x.Ignore());

            CreateMap<SqlDataReader, Tariff>()
                .ForMember(x => x.Cost, x => x.MapFrom(z => z["Cost"]))
                .ForMember(x => x.Id, x => x.MapFrom(z => z["Id"]))
                .ForMember(x => x.Name, x => x.MapFrom(z => z["Name"]));

            CreateMap<SqlDataReader, Hall>()
                .ForMember(x => x.Id, x => x.MapFrom(z => z["Id"]))
                .ForMember(x => x.Name, x => x.MapFrom(z => z["Name"]))
                .ForMember(x => x.Layout, x => x.MapFrom(z => z["Layout"]))
                .ForMember(x => x.Count, x => x.MapFrom(z => z["Count"]));

            CreateMap<SqlDataReader, TimeslotSeatRequest>()
                .ForMember(x => x.Row, x => x.MapFrom(z => z["Row"]))
                .ForMember(x => x.Seat, x => x.MapFrom(z => z["Seat"]))
                .ForMember(x => x.Status, x => x.MapFrom(z => z["Status"]));

            CreateMap<SqlDataReader, SeatsProcessRequest>()
                .ForMember(x => x.TimeslotID, x => x.MapFrom(z => z["TimeslotID"]))
                .ForMember(x => x.SelectedStatus, x => x.Ignore())
                .ForMember(x => x.SeatsRequest, x => x.Ignore());

            CreateMap<SqlDataReader, PotentialRealProfitRow>()
                .ForMember(x => x.Name, x => x.MapFrom(z => z["Name"]))
                .ForMember(x => x.RealProfit, x => x.MapFrom(z => z["RealProfit"]))
                .ForMember(x => x.PurchasedTickets, x => x.MapFrom(z => z["PurchasedTickets"]))
                .ForMember(x => x.PotentialProfit, x => x.MapFrom(z => z["PotentialProfit"]))
                .ForMember(x => x.ReservedTickets, x => x.MapFrom(z => z["ReservedTickets"]));

            CreateMap<SqlDataReader, SeatOccupancyRow>()
               .ForMember(x => x.Timeslot, x => x.MapFrom(z => z["StartTime"]))
               .ForMember(x => x.MovieName, x => x.MapFrom(z => z["Name"]))
               .ForMember(x => x.SeatCount, x => x.MapFrom(z => z["Count"]))
               .ForMember(x => x.SoldTickets, x => x.MapFrom(z => z["SoldTickets"]));

        }
    }
}