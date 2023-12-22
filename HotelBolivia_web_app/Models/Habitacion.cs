using HotelBolivia_web_app.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBolivia_web_app.Models
{
    public class Habitacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Numero { get; set; }
        public string? Foto { get; set; }
        [Required]
        public TipoHabitacionesEnum Habitaciones { get; set; }

        [NotMapped]
        [Display(Name = "Cargar Foto")]
        public IFormFile? FotoFile { get; set; }

        ///realaciones a muchos
        public virtual List<PagoAlquiler>? PagoAlquilers { get; set; }
    }
}
