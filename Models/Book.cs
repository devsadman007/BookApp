using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(50)]
        public string Author { get; set; }

        [Required, Range(0, 1000)]
        public decimal Price { get; set; }
    }
}
