using System;
using System.Collections.Generic;
using Gateway.DTO;
using Gateway.Models;
namespace Lab2Tests
{
    internal class Builder
    {
        public static PaginationResponse<IEnumerable<Hotels>>? BuildHotelsPages(int? page, int? size)
        {
            List<Hotels> hotels =
            [
                new()
                {
                    Id = 1,
                    HotelUid = Guid.NewGuid(),
                    Address = "Госпитальная набережная, д.2",
                    City = "Москва",
                    Country = "Россия",
                    Name = "Физра у Брызгалова",
                    Price = 10000,
                    Stars = 1
                },

                new()
                {
                    Id = 2,
                    HotelUid = Guid.NewGuid(),
                    Address = "Измайловский пр., 73А",
                    City = "Москва",
                    Country = "Россия",
                    Name = "Капибария",
                    Price = 20000,
                    Stars = 5
                },

                new()
                {
                    Id = 3,
                    HotelUid = Guid.NewGuid(),
                    Address = "Улица Пушкина, д. Колотушкина",
                    City = "в Небыляндии",
                    Country = "Где-то",
                    Name = "Прилети Сова",
                    Price = 15000,
                    Stars = 4
                }
            ];

            return new()
            {
                Page = page.Value,
                PageSize = size.Value,
                Items = hotels,
                TotalElements = hotels.Count
            };
        }


        public static Hotels? BuildHotelByUid(Guid guid)
        {
            return new()
            {
                Id = 2,
                HotelUid = guid,
                Address = "Измайловский пр., 73А",
                City = "Москва",
                Country = "Россия",
                Name = "Капибария",
                Price = 20000,
                Stars = 5
            };
        }

        public static Hotels? BuildHotelById(int id)
        {
            return id switch
            {
                1 => new()
                    {
                        Id = 1,
                        HotelUid = Guid.NewGuid(),
                        Address = "Госпитальная набережная, д.2",
                        City = "Москва",
                        Country = "Россия",
                        Name = "Физра у Брызгалова",
                        Price = 10000,
                        Stars = 1
                },
                2 => new()
                    {
                        Id = 2,
                        HotelUid = Guid.NewGuid(),
                        Address = "Измайловский пр., 73А",
                        City = "Москва",
                        Country = "Россия",
                        Name = "Капибария",
                        Price = 20000,
                        Stars = 5
                },
                _ => new()
                    {
                        Id = 3,
                        HotelUid = Guid.NewGuid(),
                        Address = "Улица Пушкина, д. Колотушкина",
                        City = "в Небыляндии",
                        Country = "Где-то",
                        Name = "Прилети Сова",
                        Price = 15000,
                        Stars = 4
                },
            };
        }

        public static IEnumerable<Reservation>? BuildReservationsList(string username)
        {
            return 
            [
                new()
                {
                    Id = 1,
                    ReservationUid = Guid.NewGuid(),
                    Username = username,
                    PaymentUid = Guid.Empty,
                    HotelId = 1,
                    Status = "PAID",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(2),
                },
                new()
                {
                    Id = 2,
                    ReservationUid = Guid.NewGuid(),
                    Username = username,
                        PaymentUid = Guid.Empty,
                    HotelId = 2,
                    Status = "PAID",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(4),
                }
            ];
        }

        public static Reservation? BuildReservationRequest()
        {
            return new()
            {
                PaymentUid = Guid.Empty,
                HotelId = 2,
                EndDate = DateTime.Now.AddDays(2),
                StartDate = DateTime.Now,
            };
        }

        public static CreateReservationRequest? BuildReservationRequestMessage()
        {
            return new()
            {
                HotelUid = Guid.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
    };
        }

        public static Reservation? BuildReservationResponse()
        {
            return new()
            {
                Id = 1,
                ReservationUid = Guid.Empty,
                Username = "TestUsername",
                PaymentUid = Guid.Empty,
                HotelId = 2,
                Status = "PAID",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
            };
        }

        public static Reservation? BuildReservationByGuid(Guid guid)
        {
            return new()
            {
                Id = 1,
                ReservationUid = guid,
                Username = "TestUsername",
                PaymentUid = Guid.Empty,
                HotelId = 1,
                Status = "PAID",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
            };
        }

            public static Payment? BuildPaymentByUId(Guid guid)
        {
            return new()
            {
                Id = 1,
                PaymentUid = guid,
                Status = "PAID",
                Price = 10000,
            };
        }

        public static Payment? BuildPaymentRequest(int sum)
        {
            return new()
            {
                Id = 1,
                Price = sum,
                PaymentUid = Guid.Empty,
                Status = "PAID",
            };
        }

        public static Payment? BuildPaymentResponse(int sum)
        {
            return new()
            {
                Id = 1,
                Price = sum,
                PaymentUid = Guid.Empty,
                Status = "PAID",
            };
        }

        public static Loyalty? BuildLoyaltyByUsername(string username)
        {
            return new()
            {
                Id = 1,
                Username = username,
                Status = "GOLD",
                ReservationCount = 26,
                Discount = 10,
            };
        }
    }
}
