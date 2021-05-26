using DeliveryLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace TestAppUnitTests
{
    public class InMemoryDeliveryContext
    {
        public DeliveryContext GetDeliveryContext()
        {
            var options = new DbContextOptionsBuilder<DeliveryContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            var dbContext = new DeliveryContext(options);

            return dbContext;
        }
    }
}
