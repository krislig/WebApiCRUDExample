using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCRUD.Models
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(int id, Product product);
        Task DeleteByIdAsync(int id);
    }
}
