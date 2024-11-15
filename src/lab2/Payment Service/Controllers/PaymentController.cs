using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Payment_Service
{
    [Route("/")]
    [ApiController]
    public class PaymentController(ILogger<PaymentController> logger, PaymentDBContext _paymentContext) : ControllerBase
    {
        [HttpGet("manage/health")]
        public async Task<ActionResult> HealthCheck()
        {
            return Ok();
        }

        [HttpGet("api/v1/payments/{paymentUid}")]
        public async Task<ActionResult<Payment>> GetByUid([FromRoute] Guid paymentUid)
        {
            var reservation = await _paymentContext.Payments.AsNoTracking()
                .FirstOrDefaultAsync(r => r.PaymentUid.Equals(paymentUid));

            return reservation;
        }

        [HttpPut("api/v1/payments/{paymentUid}")]
        public async Task<ActionResult<Payment?>> UpdateByUid([FromRoute] Guid paymentUid)
        {
            var res = await _paymentContext.Payments
                .FirstOrDefaultAsync(r => r.PaymentUid.Equals(paymentUid));
            res.Status = PaymentStatuses.CANCELED;
            await _paymentContext.SaveChangesAsync();
            return res;
        }

        [HttpDelete("api/v1/payments/{paymentUid}")]
        public async Task<ActionResult<Payment?>> DeleteByUid([FromRoute] Guid paymentUid)
        {
            var res = await _paymentContext.Payments
                .FirstOrDefaultAsync(r => r.PaymentUid.Equals(paymentUid));
            if (res != null)
            {
                _paymentContext.Remove(res);
                await _paymentContext.SaveChangesAsync();
            }
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

            var newReservation = new Payment()
            {
                Status = PaymentStatuses.PAID,
                PaymentUid = Guid.NewGuid(),
                Price = request.Price,
                
            };
            await _paymentContext.Payments.AddAsync(newReservation);
            await _paymentContext.SaveChangesAsync();

            return newReservation;
        }
    }
}
