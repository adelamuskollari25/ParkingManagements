using AutoMapper;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.DTOs.ParkingLot;
using ParkingManagements.Server.DTOs.ParkingSpot;
using ParkingManagements.Server.DTOs.Tariff;

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
    }
}

