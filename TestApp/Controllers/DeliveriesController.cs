using DeliveryLogic.Interfaces;
using DeliveryLogic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveriesLogic _deliveryLogic;

        public DeliveriesController(IDeliveriesLogic deliveryLogic)
        {
            _deliveryLogic = deliveryLogic;
        }

        /// <summary>
        /// Get all available deliveries
        /// </summary>
        /// <returns>List of all deliveries</returns>
        [HttpGet("GetAllDeliveries")]
        public async Task<ActionResult<List<Delivery>>> GetAllDeliveries()
        {
            return await _deliveryLogic.GetDeliveries();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(int id)
        {
            var delivery = await _deliveryLogic.GetDelivery(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        [HttpPut("UpdateDeliveryStatus")]
        public async Task<IActionResult> UpdateDeliveryStatus(int deliveryId, Enums.DeliveryStatus deliveryStatus)
        {
            if (await _deliveryLogic.DeliveryExists(deliveryId))
            {
                return Ok(_deliveryLogic.UpdateDeliveryStatus(deliveryId, deliveryStatus));
            }
            else
            {
                return NotFound();
            }            
        }

        [HttpPut("ApproveDelivery")]
        public async Task<IActionResult> ApproveDelivery(int deliveryId)
        {
            if (await _deliveryLogic.ValidateDelivery(deliveryId, Enums.DeliveryStatus.Approved))
            {
                return Ok(await _deliveryLogic.UpdateDeliveryStatus(deliveryId, Enums.DeliveryStatus.Approved));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateDelivery")]
        public async Task<ActionResult<Delivery>> CreateDelivery(Delivery delivery)
        {
            var result = await _deliveryLogic.CreateDelivery(delivery);

            return CreatedAtAction("GetDelivery", new { id = result.DeliveryId }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            if (!await _deliveryLogic.DeliveryExists(id))
            {
                return NotFound("Delivery does not exist");
            }

            return Ok(await _deliveryLogic.DeleteDelivery(id));
        }

        /// <summary>
        /// Partner can complete delivery only if the delivery is approved and within the access time window specified
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        [HttpPut("CompleteDelivery")]
        public async Task<IActionResult> CompleteDelivery(int deliveryId)
        {
            if (await _deliveryLogic.ValidateDelivery(deliveryId, Enums.DeliveryStatus.Completed))
            {
                return Ok(await _deliveryLogic.UpdateDeliveryStatus(deliveryId, Enums.DeliveryStatus.Completed));
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Partner/User can cancel delivery only if delivery is in created/approved state
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        [HttpPut("CancelDelivery")]
        public async Task<IActionResult> CancelDelivery(int deliveryId)
        {
            if (await _deliveryLogic.ValidateDelivery(deliveryId, Enums.DeliveryStatus.Cancelled))
            {
                return Ok(await _deliveryLogic.UpdateDeliveryStatus(deliveryId, Enums.DeliveryStatus.Cancelled));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
