using System.Collections.Generic;

namespace WebApiCRUD.Models
{
    public class ProductRepository : IProductRepository
    {
        private DocumentDBRepository<Product> _documentDbRepository;

        public ProductRepository(DocumentDBRepository<Product> documentDBRepository)
        {
            _documentDbRepository = documentDBRepository;
        }

        public void Add(Product product)
        {
            _documentDbRepository.CreateItemAsync(product).Wait();
        }

        public void DeleteById(int id)
        {
            _documentDbRepository.DeleteItemAsync(id).Wait();
        }

        public IEnumerable<Product> GetAll()
        {
            return _documentDbRepository.GetItemsAsync().Result;
        }

        public Product GetById(int id)
        {
            return _documentDbRepository.GetItemAsync(id).Result;
        }

        public void Update(int id, Product product)
        {
            _documentDbRepository.UpdateItemAsync(id, product).Wait();
        }
    }
}
