using DeliveryLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DeliveryLogic.Models.Enums;

namespace DeliveryLogic.Interfaces
{
    public interface IDeliveriesLogic
    {
        Task<List<Delivery>> GetDeliveries();

        Task<Delivery> GetDelivery(int deliveryId);

        Task<Delivery> CreateDelivery(Delivery delivery);

        Task<bool> UpdateDeliveryStatus(int deliveryId, DeliveryStatus deliveryStatus);

        Task<bool> DeleteDelivery(int deliveryId);

        Task<bool> DeliveryExists(int deliveryId);

        Task<bool> ValidateDelivery(int deliveryId, DeliveryStatus deliveryStatus);

        Task ExpireOldDeliveries();
    }
}
