using AutoMapper;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.DTOs.ParkingLot;
using ParkingManagements.Server.DTOs.ParkingSpot;
using ParkingManagements.Server.DTOs.Payment;
using ParkingManagements.Server.DTOs.Tariff;
using ParkingManagements.Server.DTOs.Ticket;

public class ParkingMappingProfile : Profile
{
    public ParkingMappingProfile()
    {
        CreateMap<ParkingLot, ParkingLotKpiDTO>()
            .ForMember(dest => dest.TotalSpots, opt => opt.MapFrom(src => src.ParkingSpots.Count))
            .ForMember(dest => dest.OccupiedSpots, opt => opt.MapFrom(src => src.ParkingSpots.Count(s => s.Status == SpotStatus.Occupied)))
            .ForMember(dest => dest.FreeSpots, opt => opt.MapFrom(src => src.ParkingSpots.Count(s => s.Status == SpotStatus.Free)))
            .ForMember(dest => dest.UnavailableSpots, opt => opt.MapFrom(src => src.ParkingSpots.Count(s => s.Status == SpotStatus.Unavailable)));

        CreateMap<ParkingLotCreateDTO, ParkingLot>().ReverseMap();

        CreateMap<ParkingLot, ParkingLotDTO>();

        CreateMap<ParkingSpot, ParkingSpotDTO>();
        CreateMap<ParkingSpotDTO, ParkingSpot>().ReverseMap();
        CreateMap<ParkingSpotCreateDTO, ParkingSpot>().ReverseMap();
        CreateMap<ParkingSpotUpdateDTO, ParkingSpot>().ReverseMap();

        CreateMap<TariffUpdateDTO, Tariff>().ReverseMap();
        CreateMap<TariffDTO, Tariff>().ReverseMap();

        //CreateMap<TicketDTO, Ticket>().ReverseMap();
        // NEW:
        CreateMap<Ticket, TicketDTO>()
            .ForMember(dest => dest.SpotCode,
                opt => opt.MapFrom(src => src.ParkingSpot.SpotCode))
            .ForMember(dest => dest.Vehicle,
            opt => opt.MapFrom(src => src.Vehicle));

        CreateMap<TicketDTO, Ticket>();
        //no change below
        CreateMap<TicketCreateDTO, Ticket>().ReverseMap();
        CreateMap<TicketCloseDTO, Ticket>().ReverseMap();
        CreateMap<TicketPreviewExitDTO, Ticket>().ReverseMap();
        CreateMap<TicketSearchDTO, Ticket>().ReverseMap();

        CreateMap<PaymentDTO, Payment>().ReverseMap();

        CreateMap<VehicleDTO, Vehicle>().ReverseMap();


    }
}

