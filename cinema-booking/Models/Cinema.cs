using cinema_booking.DataContext.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace cinema_booking.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();


    }
}

