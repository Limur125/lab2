using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Gateway.ServiceInterfaces;
using Gateway.Controllers;
using Gateway.Models;
using Gateway.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Lab2Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestGetAllHotels()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            int? page = 1, size = 5;
            Task<PaginationResponse<IEnumerable<Hotels>>?> returnHotelsTask = Task.Run(() => Builder.BuildHotelsPages(page, size));
            reservationMock.Setup(a => a.GetHotelsAsync(page, size)).Returns(returnHotelsTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.GetAllHotels(page, size);
            var response = await responseTask;
            var hotels = response.Value;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsAsync(page, size), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(hotels.TotalElements.Equals(3));
        }

        [TestMethod]
        public async Task TestGetUserInfoByUsername()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            string username = "TestUsername";
            Task<IEnumerable<Reservation>?> returnReservationsTask = Task.Run(() => Builder.BuildReservationsList(username));
            reservationMock.Setup(a => a.GetReservationsByUsernameAsync(username)).Returns(returnReservationsTask);

            int id = 1;
            Task<Hotels?> returnHotelByIdTask = Task.Run(() => Builder.BuildHotelById(id));
            reservationMock.Setup(a => a.GetHotelsByIdAsync(id)).Returns(returnHotelByIdTask);

            id = 2;
            returnHotelByIdTask = Task.Run(() => Builder.BuildHotelById(id));
            reservationMock.Setup(a => a.GetHotelsByIdAsync(id)).Returns(returnHotelByIdTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Guid guid = Guid.Empty;
            Task<Payment?> returnPaymentByGuidTask = Task.Run(() => Builder.BuildPaymentByUId(guid));
            paymentMock.Setup(a => a.GetPaymentByUidAsync(guid)).Returns(returnPaymentByGuidTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Task<Loyalty?> returnLoyaltyByUsernameTask = Task.Run(() => Builder.BuildLoyaltyByUsername(username));
            loyaltyMock.Setup(a => a.GetLoyaltyByUsernameAsync(username)).Returns(returnLoyaltyByUsernameTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.GetUserInfoByUsername(username);
            var response = await responseTask;
            var info = response.Value;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetReservationsByUsernameAsync(username), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByIdAsync(1), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByIdAsync(2), Times.Once());

            paymentMock.Verify(mock => mock.HealthCheckAsync(), Times.Exactly(2));
            paymentMock.Verify(mock => mock.GetPaymentByUidAsync(guid), Times.Exactly(2));

            loyaltyMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            loyaltyMock.Verify(mock => mock.GetLoyaltyByUsernameAsync(username), Times.Once());

            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(info.Reservations.Count.Equals(2));
        }

        [TestMethod]
        public async Task TestGetReservationsInfoByUsername()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            string username = "TestUsername";
            Task<IEnumerable<Reservation>?> returnReservationsTask = Task.Run(() => Builder.BuildReservationsList(username));
            reservationMock.Setup(a => a.GetReservationsByUsernameAsync(username)).Returns(returnReservationsTask);

            int id = 1;
            Task<Hotels?> returnHotelByIdTask = Task.Run(() => Builder.BuildHotelById(id));
            reservationMock.Setup(a => a.GetHotelsByIdAsync(id)).Returns(returnHotelByIdTask);

            id = 2;
            returnHotelByIdTask = Task.Run(() => Builder.BuildHotelById(id));
            reservationMock.Setup(a => a.GetHotelsByIdAsync(id)).Returns(returnHotelByIdTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Guid guid = System.Guid.Empty;
            Task<Payment?> returnPaymentByGuidTask = Task.Run(() => Builder.BuildPaymentByUId(guid));
            paymentMock.Setup(a => a.GetPaymentByUidAsync(guid)).Returns(returnPaymentByGuidTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.GetUserInfoByUsername(username);
            var response = await responseTask;
            var info = response.Value;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetReservationsByUsernameAsync(username), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByIdAsync(1), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByIdAsync(2), Times.Once());

            paymentMock.Verify(mock => mock.HealthCheckAsync(), Times.Exactly(2));
            paymentMock.Verify(mock => mock.GetPaymentByUidAsync(guid), Times.Exactly(2));

            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(info.Reservations.Count.Equals(2));
        }

        [TestMethod]
        public async Task TestGetReservationsInfoByGuid()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Guid guid = System.Guid.Empty;
            Task<Reservation?> returnReservationTask = Task.Run(() => Builder.BuildReservationByGuid(guid));
            reservationMock.Setup(a => a.GetReservationsByUidAsync(guid)).Returns(returnReservationTask);

            int id = 1;
            Task<Hotels?> returnHotelByIdTask = Task.Run(() => Builder.BuildHotelById(id));
            reservationMock.Setup(a => a.GetHotelsByIdAsync(id)).Returns(returnHotelByIdTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Task<Payment?> returnPaymentByGuidTask = Task.Run(() => Builder.BuildPaymentByUId(guid));
            paymentMock.Setup(a => a.GetPaymentByUidAsync(guid)).Returns(returnPaymentByGuidTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            string username = "TestUsername";

            var responseTask = controller.GetReservationsInfoByUsername(guid, username);
            var response = await responseTask;
            var info = response.Value;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetReservationsByUidAsync(guid), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByIdAsync(1), Times.Once());

            paymentMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            paymentMock.Verify(mock => mock.GetPaymentByUidAsync(guid), Times.Once());

            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(info.Hotel.Stars.Equals(1));
        }

        [TestMethod]
        public async Task TestCreateReservation()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Guid guid = System.Guid.Empty;
            Task<Hotels?> returnHotelByUidTask = Task.Run(() => Builder.BuildHotelByUid(guid));
            reservationMock.Setup(a => a.GetHotelsByUidAsync(guid)).Returns(returnHotelByUidTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            string username = "TestUsername";
            Task<Loyalty?> returnLoyaltyByUsernameTask = Task.Run(() => Builder.BuildLoyaltyByUsername(username));
            loyaltyMock.Setup(a => a.GetLoyaltyByUsernameAsync(username)).Returns(returnLoyaltyByUsernameTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            int sum = 36000;
            Task<Payment?> returnCreatedPaymentTask = Task.Run(() => Builder.BuildPaymentResponse(sum));
            paymentMock.Setup(a => a.CreatePaymentAsync(It.IsAny<Payment>())).Returns(returnCreatedPaymentTask);

            //Task<Payment?> returnCreatedPaymentTask = Task.Run(() => Builder.BuildPaymentResponse(sum));
            //paymentMock.Setup(a => a.CreatePaymentAsync(Builder.BuildPaymentRequest(sum))).Returns(returnCreatedPaymentTask);


            Task<Loyalty?> returnPutedLoyaltyTask = Task.Run(() => Builder.BuildLoyaltyByUsername(username));
            loyaltyMock.Setup(a => a.PutLoyaltyByUsernameAsync(username)).Returns(returnPutedLoyaltyTask);

            Task<Reservation?> returnCreatedReservationTask = Task.Run(() => Builder.BuildReservationResponse());
            reservationMock.Setup(a => a.CreateReservationAsync(username, It.IsAny<Reservation>())).Returns(returnCreatedReservationTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.CreateReservation(username, Builder.BuildReservationRequestMessage());
            var response = await responseTask;
            var info = response.Value;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetHotelsByUidAsync(guid), Times.Once());
            //reservationMock.Verify(mock => mock.CreateReservationAsync(username, Builder.BuildReservationRequest()), Times.Once());
            
            paymentMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            //paymentMock.Verify(mock => mock.CreatePaymentAsync(Builder.BuildPaymentRequest(sum)), Times.Once());

            loyaltyMock.Verify(mock => mock.HealthCheckAsync(), Times.Exactly(2));
            loyaltyMock.Verify(mock => mock.GetLoyaltyByUsernameAsync(username), Times.Once());
            loyaltyMock.Verify(mock => mock.PutLoyaltyByUsernameAsync(username), Times.Once());

            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            //Assert.IsTrue(info.Payment.Price.Equals(sum));
            //Assert.IsTrue(info.Discount.Equals(10));
        }

        [TestMethod]
        public async Task TestDeleteReservationsByUid()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Guid guid = System.Guid.Empty;
            Task<Reservation?> returnReservationTask = Task.Run(() => Builder.BuildReservationByGuid(guid));
            reservationMock.Setup(a => a.GetReservationsByUidAsync(guid)).Returns(returnReservationTask);

            Task<Reservation?> returnReservationDeleteTask = Task.Run(() => Builder.BuildReservationByGuid(guid));
            reservationMock.Setup(a => a.DeleteReservationAsync(guid)).Returns(returnReservationDeleteTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            Task<Payment?> returnCanceledPaymentTask = Task.Run(() => Builder.BuildPaymentByUId(guid));
            paymentMock.Setup(a => a.CancelPaymentByUidAsync(guid)).Returns(returnCanceledPaymentTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            string username = "TestUsername";
            Task<Loyalty?> returnLoyaltyByUsernameTask = Task.Run(() => Builder.BuildLoyaltyByUsername(username));
            loyaltyMock.Setup(a => a.DeleteLoyaltyByUsernameAsync(username)).Returns(returnLoyaltyByUsernameTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.DeleteReservationsByUid(guid, username);
            var response = await responseTask;

            reservationMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            reservationMock.Verify(mock => mock.GetReservationsByUidAsync(guid), Times.Once());
            reservationMock.Verify(mock => mock.DeleteReservationAsync(guid), Times.Once());

            paymentMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            paymentMock.Verify(mock => mock.CancelPaymentByUidAsync(guid), Times.Once());

            loyaltyMock.Verify(mock => mock.DeleteLoyaltyByUsernameAsync(username), Times.Once());

            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
        }

        [TestMethod]
        public async Task TestGetLoyaltyInfoByUsername()
        {
            var loggerMock = new Mock<ILogger<GatewayController>>();

            var reservationMock = new Mock<IReservationService>();
            Task<bool> healthCheckTask = Task.Run(() => true);
            reservationMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            var paymentMock = new Mock<IPaymentService>();
            paymentMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            var loyaltyMock = new Mock<ILoyaltyService>();
            loyaltyMock.Setup(a => a.HealthCheckAsync()).Returns(healthCheckTask);

            string username = "TestUsername";
            Task<Loyalty?> returnLoyaltyByUsernameTask = Task.Run(() => Builder.BuildLoyaltyByUsername(username));
            loyaltyMock.Setup(a => a.GetLoyaltyByUsernameAsync(username)).Returns(returnLoyaltyByUsernameTask);

            GatewayController controller = new GatewayController(loggerMock.Object, reservationMock.Object, paymentMock.Object, loyaltyMock.Object);

            var responseTask = controller.GetLoyaltyInfoByUsername(username);
            var response = await responseTask;

            loyaltyMock.Verify(mock => mock.HealthCheckAsync(), Times.Once());
            loyaltyMock.Verify(mock => mock.GetLoyaltyByUsernameAsync(username), Times.Once());
        }
        }
    }