using System.ComponentModel.DataAnnotations;

namespace MyStore.DTO
{
    public class CheckOutDto
    {
        [Required(ErrorMessage = "Address Required")]
        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }
}
