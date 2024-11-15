using Gateway.Models;
using System;
using System.Threading.Tasks;

namespace Gateway.ServiceInterfaces
{
    public interface IPaymentService
    {
        public Task<Payment?> GetPaymentByUidAsync(Guid paymentUid);

        public Task<Payment?> CancelPaymentByUidAsync(Guid paymentUid);

        public Task<Payment?> CreatePaymentAsync(Payment request);
    }
}
