using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.Models
{
    [Serializable]
    public class Author
    {
        public int AuthorID { get; set; }
        [Required]
        [Display(Name ="Имя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Отчество")]
        public string? FamilyName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата Рождения")]
        public DateTime DoB { get; set; }
        public ICollection<Book>? Books { get; set; }       
    }
}
