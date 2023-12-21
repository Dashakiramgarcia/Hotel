using HotelBolivia_web_app.Dtos;
using System.ComponentModel.DataAnnotations;
namespace HotelBolivia_web_app.Models
{
    public class Usuario
    {
 
        [Key]
        public int Id { get; set; }
        [Required]
        public String? Email { get; set; }
        [Required]
        public String? Password { get; set; }
        [Required]
        public String? NombreCompleto { get; set; }
        [Required]
        public RolEnum Rol { get; set; }

        ///realaciones a muchos
        public virtual List<PagoAlquiler>? PagoAlquilers { get; set; }

    }
}
