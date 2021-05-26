using DeliveryLogic.Interfaces;
using DeliveryLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryLogic
{
    public class DeliveriesLogic : IDeliveriesLogic
    {
        private readonly DeliveryContext _context;

        public DeliveriesLogic(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<Delivery> CreateDelivery(Delivery delivery)
        {
            delivery.State = Enums.DeliveryStatus.Created;
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            return delivery;
        }

        public async Task<Delivery> GetDelivery(int deliveryId)
        {
            var delivery = await _context.Deliveries
                                            .Include(d => d.AccessWindow)
                                            .Include(d => d.Recipient)
                                            .Include(d => d.Order)
                                            .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);

            return delivery;
        }

        public async Task<bool> DeleteDelivery(int deliveryId)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);

            _context.Deliveries.Remove(delivery);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<bool> DeliveryExists(int deliveryId)
        {
            return await _context.Deliveries.AnyAsync(e => e.DeliveryId == deliveryId);
        }

        public async Task<List<Delivery>> GetDeliveries()
        {
            return await _context.Deliveries
                .Include(d => d.AccessWindow)
                .Include(d => d.Recipient)
                .Include(d => d.Order)
                .ToListAsync();
        }

        public async Task<bool> UpdateDeliveryStatus(int deliveryId, Enums.DeliveryStatus deliveryStatus)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                delivery.State = deliveryStatus;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateDelivery(int deliveryId, Enums.DeliveryStatus deliveryStatus)
        {
            var delivery = await GetDelivery(deliveryId);

            if (delivery != null)
            {
                switch (deliveryStatus)
                {
                    case Enums.DeliveryStatus.Approved:
                        if (delivery.State == Enums.DeliveryStatus.Created)
                        {
                            return true;
                        }
                        break;
                    case Enums.DeliveryStatus.Completed:
                        var currentDateTime = DateTime.Now;
                        if (delivery.State == Enums.DeliveryStatus.Approved
                            && currentDateTime >= delivery.AccessWindow.StartTime
                            && currentDateTime <= delivery.AccessWindow.EndTime)
                        {
                            return true;
                        }
                        break;
                    case Enums.DeliveryStatus.Cancelled:
                        if (delivery.State == Enums.DeliveryStatus.Created
                            || delivery.State == Enums.DeliveryStatus.Approved)
                        {
                            return true;
                        }
                        break;
                    case Enums.DeliveryStatus.Created:
                    case Enums.DeliveryStatus.Expired:
                        return true;
                }
            }

            return false;
        }

        public async Task ExpireOldDeliveries()
        {
            var currentDateTime = DateTime.Now;
            foreach (var delivery in await this.GetDeliveries())
            {
                if ((delivery.State != Enums.DeliveryStatus.Cancelled
                    || delivery.State != Enums.DeliveryStatus.Completed)
                    && currentDateTime > delivery.AccessWindow.EndTime)
                {
                    delivery.State = Enums.DeliveryStatus.Expired;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
