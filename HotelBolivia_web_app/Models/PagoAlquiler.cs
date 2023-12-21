using HotelBolivia_web_app.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBolivia_web_app.Models
{
    public class PagoAlquiler
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Ci { get; set; }
        [Required]
        public String? NombreCompleto { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        [Required]
        public int Dias { get; set; }
        [Required]
        public float MontoTotal { get; set; }
        
        [Required]
        public int NumFactura { get; set; }

        //Forering Keys
        public int HabtacionId { get; set; }
        public virtual Habitacion? Habitacion { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

    } 
}
