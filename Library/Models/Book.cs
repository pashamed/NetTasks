using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Введите количество")]
        [Display(Name = "Количество страниц")]
        public int PageNum { get; set; }
        [Display(Name = "Жанр")]
        public Genre Genre { get; set; }
        [JsonIgnore]
        [ForeignKey("AuthorID")]
        public Author? Author { get; set; }
    }
}
