using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Products
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Display(Name = "product name")]
        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(50, ErrorMessage = "max length for {0} has {1} char")]
        public string Name { get; set; }

        [Display(Name = "product amount")]
        [Required(ErrorMessage = "{0} is required")]
        public int Amount { get; set; }

        [Display(Name = "product description")]
        [Required(ErrorMessage = "{0} is required")]
        public string Description { get; set; }
    }
}