namespace cinema_booking.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }=string.Empty;
        public bool IsReserved { get; set; }=false;

    }
}
