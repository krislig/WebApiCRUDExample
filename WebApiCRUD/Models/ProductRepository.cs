using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCRUD.Models
{
    public class ProductRepository : IProductRepository
    {
        private DocumentDBRepository<Product> _documentDbRepository;

        public ProductRepository(DocumentDBRepository<Product> documentDBRepository)
        {
            _documentDbRepository = documentDBRepository;
        }

        public async Task AddAsync(Product product)
        {
            await _documentDbRepository.CreateItemAsync(product);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _documentDbRepository.DeleteItemAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _documentDbRepository.GetItemsAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _documentDbRepository.GetItemAsync(id);
        }

        public async Task UpdateAsync(int id, Product product)
        {
            await _documentDbRepository.UpdateItemAsync(id, product);
        }
    }
}
