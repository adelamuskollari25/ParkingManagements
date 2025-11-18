namespace ParkingManagements.Server.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using ParkingManagements.Data;
    using ParkingManagements.Data.Entities;
    using ParkingManagements.Server.DTOs.Tariff;
    using ParkingManagements.Server.Interfaces;

    public class TariffService : ITariffService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TariffService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TariffDTO?> GetCurrentTariffAsync(Guid lotId)
        {
            return await _context.Tariffs
                .Where(t => t.LotId == lotId)
                .OrderByDescending(t => t.EffectiveFrom)
                .ProjectTo<TariffDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TariffDTO>> GetTariffHistoryAsync(Guid lotId)
        {
            return await _context.Tariffs
                .Where(t => t.LotId == lotId)
                .OrderByDescending(t => t.EffectiveFrom)
                .ProjectTo<TariffDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TariffDTO> UpdateTariffAsync(Guid lotId, TariffUpdateDTO dto)
        {
            var tariff = new Tariff
            {
                Id = Guid.NewGuid(),
                LotId = lotId,
                RatePerHour = dto.RatePerHour,
                BillingPeriodMinutes = dto.BillingPeriodMinutes,
                GracePeriodMinutes = dto.GracePeriodMinutes,
                DailyMaximum = dto.DailyMaximum,
                LostTicketFee = dto.LostTicketFee,
                EffectiveFrom = dto.EffectiveFrom
            };

            _context.Tariffs.Add(tariff);
            await _context.SaveChangesAsync();

            return _mapper.Map<TariffDTO>(tariff);
        }
    }

}
