using DeliveryLogic.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeliveryExpiry
{
    public class ExpireOldDeliveries
    {
        private readonly IDeliveriesLogic _deliveriesLogic;

        public ExpireOldDeliveries(IDeliveriesLogic deliveriesLogic)
        {
            _deliveriesLogic = deliveriesLogic;
        }

        [FunctionName("ExpireOldDeliveries")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await _deliveriesLogic.ExpireOldDeliveries();

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
