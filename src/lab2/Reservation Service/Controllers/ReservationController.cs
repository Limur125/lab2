using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation_Service
{
    [Route("/")]
    [ApiController]
    public class ReservationController(ILogger<ReservationController> logger, ReservationDBContext reservationsContext) : ControllerBase
    {
        [HttpGet("manage/health")]
        public async Task<ActionResult> HealthCheck()
        {
            return Ok();
        }

        [HttpGet("api/v1/reservations")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetByUsername(
            [FromHeader(Name = "X-User-Name")] string xUserName)
        {
            if (string.IsNullOrWhiteSpace(xUserName))
            {
                return BadRequest();
            }

            var query = reservationsContext.Reservations.AsNoTracking().AsQueryable();
            query = query.Where(r => r.Username.Equals(xUserName));
            var response = await query.ToListAsync();
            return response;
        }

        [HttpGet("api/v1/reservations/{reservationUid:guid}")]
        public async Task<ActionResult<Reservation?>> GetByUid([FromRoute] Guid reservationUid)
        {
            var reservation = await reservationsContext.Reservations.AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReservationUid.Equals(reservationUid));

            return reservation;
        }

        [HttpDelete("api/v1/reservations/{reservationUid}")]
        public async Task<ActionResult<Reservation?>> DeleteByUid([FromRoute] Guid reservationUid)
        {
            var res = await reservationsContext.Reservations
                .FirstOrDefaultAsync(r => r.ReservationUid.Equals(reservationUid));
            res.Status = ReservationStatuses.CANCELED;
            await reservationsContext.SaveChangesAsync();
            return res;
        }


        [HttpPost("api/v1/reservations")]
        public async Task<ActionResult<Reservation>> CreateReservation([FromHeader(Name = "X-User-Name")] string xUserName,
            [FromBody] Reservation request)
        {
            if (string.IsNullOrWhiteSpace(xUserName))
            {
                return BadRequest();
            }

            Reservation newReservation = new()
            {
                Status = ReservationStatuses.PAID,
                Username = xUserName,
                PaymentUid = request.PaymentUid,
                HotelId = request.HotelId,
                ReservationUid = Guid.NewGuid(),
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Id = request.Id,
            };
            await reservationsContext.Reservations.AddAsync(newReservation);
            await reservationsContext.SaveChangesAsync();
            logger.LogInformation($"\nReservation:\n{newReservation.Status}\n{newReservation.ReservationUid}\n{newReservation.StartDate}\n{newReservation.EndDate}\n{newReservation.Id}\n");
            return newReservation;
        }
    }
}
