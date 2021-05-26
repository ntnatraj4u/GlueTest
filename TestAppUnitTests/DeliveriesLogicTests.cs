using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeliveryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryLogic.Models;

namespace TestAppUnitTests
{
    [TestClass()]
    public class DeliveriesLogicTests
    {
        private DeliveryContext _context;
        private DeliveriesLogic logic;
        private DateTime accessDateTime;
        private Delivery initialDelivery;

        [TestInitialize]
        public void Setup()
        {
            _context = new InMemoryDeliveryContext().GetDeliveryContext();
            logic = new DeliveriesLogic(_context);
            accessDateTime = DateTime.Now;
            initialDelivery = new Delivery
            {
                AccessWindow = new AccessWindow
                {
                    StartTime = accessDateTime,
                    EndTime = accessDateTime.AddMinutes(5)
                },
                Recipient = new Recipient
                {
                    Name = "Name",
                    Address = "Address",
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                },
                Order = new Order
                {
                    OrderNumber = "1",
                    Sender = "IKEA"
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod()]
        public void CreateDelivery_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.AreEqual(createdDelivery.DeliveryId, 1);
            Assert.AreEqual(createdDelivery.State, Enums.DeliveryStatus.Created);
        }

        [TestMethod()]
        public void GetDelivery_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;
            var delivery = logic.GetDelivery(createdDelivery.DeliveryId).Result;

            Assert.AreEqual(delivery, createdDelivery);
        }

        [TestMethod()]
        public void DeleteDelivery_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsTrue(logic.DeleteDelivery(createdDelivery.DeliveryId).Result);
        }

        [TestMethod()]
        public void DeliveryExists_Exists_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsTrue(logic.DeliveryExists(createdDelivery.DeliveryId).Result);
        }

        [TestMethod()]
        public void DeliveryExists_NotExists_Success()
        {
            Assert.IsFalse(logic.DeliveryExists(0).Result);
        }

        [TestMethod()]
        public void UpdateDeliveryStatus_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsTrue(logic.UpdateDeliveryStatus(createdDelivery.DeliveryId, Enums.DeliveryStatus.Completed).Result);

            var updatedDelivery = logic.GetDelivery(createdDelivery.DeliveryId).Result;

            Assert.AreEqual(Enums.DeliveryStatus.Completed, updatedDelivery.State);
        }

        [TestMethod()]
        public void ValidateDelivery_Approved_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsTrue(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Approved).Result);
        }

        [TestMethod()]
        public void ValidateDelivery_Approved_Failure()
        {
            initialDelivery.State = Enums.DeliveryStatus.Completed;
            var createdDelivery = _context.Deliveries.Add(initialDelivery).Entity;

            Assert.IsFalse(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Approved).Result);
        }

        [TestMethod()]
        public void ValidateDelivery_Cancelled_Success()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsTrue(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Cancelled).Result);
        }

        [TestMethod()]
        public void ValidateDelivery_Cancelled_Failure()
        {
            initialDelivery.State = Enums.DeliveryStatus.Completed;
            var createdDelivery = _context.Deliveries.Add(initialDelivery).Entity;

            Assert.IsFalse(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Approved).Result);
        }

        [TestMethod()]
        public void ValidateDelivery_Completed_Success()
        {
            initialDelivery.State = Enums.DeliveryStatus.Approved;
            var createdDelivery = _context.Deliveries.Add(initialDelivery).Entity;
            _context.SaveChanges();

            Assert.IsTrue(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Completed).Result);
        }

        [TestMethod()]
        public void ValidateDelivery_Completed_Failure()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.IsFalse(logic.ValidateDelivery(createdDelivery.DeliveryId, Enums.DeliveryStatus.Completed).Result);
        }

        [TestMethod()]
        public void ExpireOldDeliveries_Success()
        {
            initialDelivery.State = Enums.DeliveryStatus.Approved;
            initialDelivery.AccessWindow.EndTime = DateTime.Now.AddMinutes(-1);
            var createdDelivery = _context.Deliveries.Add(initialDelivery).Entity;
            _context.SaveChanges();

            logic.ExpireOldDeliveries().GetAwaiter().GetResult();

            Assert.AreEqual(logic.GetDelivery(createdDelivery.DeliveryId).Result.State, Enums.DeliveryStatus.Expired);
        }

        [TestMethod()]
        public void ExpireOldDeliveries_Failure()
        {
            var createdDelivery = logic.CreateDelivery(initialDelivery).Result;

            Assert.AreNotEqual(logic.GetDelivery(createdDelivery.DeliveryId).Result.State, Enums.DeliveryStatus.Expired);
        }
    }
}