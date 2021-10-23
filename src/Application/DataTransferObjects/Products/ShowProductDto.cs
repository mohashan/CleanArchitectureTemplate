using Application.DataTransferObjects.Common;
using Domain.Entities;

namespace Application.DataTransferObjects.Products
{
    public class ShowProductDto : BaseDto<ShowProductDto, Product>
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}