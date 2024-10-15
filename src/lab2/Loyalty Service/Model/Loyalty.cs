namespace Loyalty_Service
{
    public class Loyalty
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int ReservationCount { get; set; }
        public int Discount { get; set; }
    }
}
