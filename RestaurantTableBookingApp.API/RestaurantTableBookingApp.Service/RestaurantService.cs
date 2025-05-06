using RestaurantTableBookingApp.Core.ViewModels;
using RestaurantTableBookingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestaurantTableBookingApp.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository) 
        {
            this._restaurantRepository = restaurantRepository;
        }

        public async Task<List<RestaurantModel>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllRestaurantsAsync();
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            return await _restaurantRepository.GetDiningTablesByBranchAsync(branchId, date);
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId)
        {
            return await _restaurantRepository.GetDiningTablesByBranchAsync(branchId);
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchByRestaurantIdAsync(int restaurantId)
        {
            return await _restaurantRepository.GetRestaurantBranchByRestaurantIdAsync(restaurantId);
        }
    }
}
