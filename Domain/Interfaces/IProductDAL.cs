
using Domain.Aggregates.Products;

namespace Domain.Interfaces
{
    public interface IProductDAL: IGenericEntityDAL<ProductEntity, int>
    {
    }
}
