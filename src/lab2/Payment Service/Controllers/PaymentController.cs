using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payment_Service
{
    [Route("/")]
    [ApiController]
    public class PaymentController(ILogger<PaymentController> logger, PaymentDBContext paymentContext) : ControllerBase
    {
        [HttpGet("manage/health")]
        public async Task<ActionResult> HealthCheck()
        {
            return Ok();
        }

        [HttpGet("api/v1/payments/{paymentUid}")]
        public async Task<ActionResult<Payment>> GetByUid([FromRoute] Guid paymentUid)
        {
            var reservation = await paymentContext.Payments.AsNoTracking()
                .FirstOrDefaultAsync(r => r.PaymentUid.Equals(paymentUid));

            return reservation;
        }

        [HttpDelete("api/v1/payments/{paymentUid}")]
        public async Task<ActionResult<Payment>> UpdateByUid([FromRoute] Guid paymentUid)
        {
            var res = await paymentContext.Payments
                .FirstOrDefaultAsync(r => r.PaymentUid.Equals(paymentUid));
            res.Status = PaymentStatuses.CANCELED;
            await paymentContext.SaveChangesAsync();
            return res;
        }


        [HttpPost("api/v1/payments")]
        public async Task<ActionResult<Payment>> CreatePayment(
            [FromBody] Payment request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            Payment newReservation = new()
            {
                Status = PaymentStatuses.PAID,
                PaymentUid = Guid.NewGuid(),
                Price = request.Price,

            };
            await paymentContext.Payments.AddAsync(newReservation);
            await paymentContext.SaveChangesAsync();

            return newReservation;
        }
    }
}
