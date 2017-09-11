using System.Collections.Generic;

namespace WebApiCRUD.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(int id, Product product);
        void DeleteById(int id);
    }
}
