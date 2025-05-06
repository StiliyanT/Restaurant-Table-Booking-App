using Microsoft.EntityFrameworkCore;
using RestaurantTableBookingApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestaurantTableBookingApp.Data
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantTableBookingDbContext _dbContext;

        public RestaurantRepository(RestaurantTableBookingDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Async/Await should be used where you need to manipulate the record, unitl that time return task is sufficient
        /// </summary>
        /// <returns></returns>
        public Task<List<RestaurantModel>> GetAllRestaurantsAsync()
        {
            var restaurants = _dbContext.Restaurants
                .OrderBy(x => x.Name)
                .Select(r => new RestaurantModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone,
                    Email = r.Email,
                    ImageUrl = r.ImageUrl

                }).ToListAsync();

            return restaurants;
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            var diningTables = await _dbContext.DiningTables
                .Where(dt => dt.RestaurantBranchId == branchId)
                .SelectMany(dt => dt.TimeSlots, (dt,ts) => new
                {
                    dt.RestaurantBranchId,
                    dt.TableName,
                    dt.Capacity,
                    ts.ReservationDay,
                    ts.MealType,
                    ts.TableStatus,
                    ts.Id
                })
                .Where(ts => ts.ReservationDay == date.Date)
                .OrderBy(ts => ts.Id)
                .ThenBy(ts => ts.MealType)
                .ToListAsync();

            return diningTables.Select(dt => new DiningTableWithTimeSlotsModel
            {
                BranchId = dt.RestaurantBranchId,
                ReservationDay = dt.ReservationDay.Date,
                TableName = dt.TableName,
                Capacity = dt.Capacity,
                MealType = dt.MealType,
                TableStatus = dt.TableStatus,
                TimeSlotId = dt.Id,
            });
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId)
        {

            var diningTables = await _dbContext.DiningTables
                .Where(dt => dt.RestaurantBranchId == branchId)
                .SelectMany(dt => dt.TimeSlots, (dt, ts) => new
                {
                    dt.RestaurantBranchId,
                    dt.TableName,
                    dt.Capacity,
                    ts.ReservationDay,
                    ts.MealType,
                    ts.TableStatus,
                    ts.Id
                })
                .Where(ts => ts.ReservationDay == DateTime.Now.Date)
                .OrderBy(ts => ts.Id)
                .ThenBy(ts => ts.MealType)
                .ToListAsync();

            return diningTables.Select(dt => new DiningTableWithTimeSlotsModel
            {
                BranchId = dt.RestaurantBranchId,
                ReservationDay = dt.ReservationDay.Date,
                TableName = dt.TableName,
                Capacity = dt.Capacity,
                MealType = dt.MealType,
                TableStatus = dt.TableStatus,
                TimeSlotId = dt.Id,
            });
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchByRestaurantIdAsync(int restaurantId)
        {
            var branches = await _dbContext.RestaurantBranches
                .Where(rb => rb.RestaurantId == restaurantId)
                .Select(rb => new RestaurantBranchModel()
                {
                    Id = rb.Id,
                    RestaurantId = rb.RestaurantId,
                    Name = rb.Name,
                    Address = rb.Address,
                    Phone = rb.Phone,
                    Email = rb.Email,
                    ImageUrl = rb.ImageUrl

                }).ToListAsync();

            return branches;
        }
    }
}
