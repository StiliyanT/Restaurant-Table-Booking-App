using RestaurantTableBookingApp.Core;
using RestaurantTableBookingApp.Core.ViewModels;

namespace RestaurantTableBookingApp.Data
{
    public interface IReservationRepository
    {
        Task<int> CreateOrUpdateReservationAsync(ReservationModel reservation);
        Task<TimeSlot> GetTimeSlotByIdAsync(int timeSlotId);

        Task<DiningTableWithTimeSlotsModel> UpdateReservationAsync(DiningTableWithTimeSlotsModel reservation);
        Task<List<ReservationDetailsModel>> GetReservationDetailsAsync();
    }
}