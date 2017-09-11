using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCRUD.Controllers;
using WebApiCRUD.Models;
using Xunit;

namespace WebApiCRUD.Tests
{
    public class ProductControllerTests
    {
        static List<Product> SampleProducts = new List<Product>()
        {
            new Product(){Id = "1", Name = "Phone", Description = "Great phone", Price = 2000.0m},
            new Product(){Id = "2", Name = "TV", Description = "Samsung", Price = 5000.0m},
            new Product(){Id = "15", Name = "Pencil", Description = "Black Pencil", Price = 1.99m},
            new Product(){Id = "88", Name = "Glass", Description = "Glass", Price = 15.0m},
            new Product(){Id = "222", Name = "Office Desk", Description = "Wooden office desk", Price = 1500.0m},
            new Product(){Id = "823749", Name = "Keyboard", Description = "Ergonomic keyboard", Price = 399.99m}
        };

        [Fact]
        public async void Get_GetAllProducts_Ok()
        {
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Product>>(SampleProducts));

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.GetAsync();

            Assert.IsAssignableFrom<OkObjectResult>(response);
            OkObjectResult result = response as OkObjectResult;
            Assert.NotNull(result);
            Assert.True(result.StatusCode == 200);
            var products = result.Value as IEnumerable<Product>;

            Assert.NotNull(products);
            Assert.NotEmpty(products);
            Assert.True(products.Count() == SampleProducts.Count);
        }

        [Fact]
        public async void Post_AddProduct_Ok()
        {
            var mockRepository = new Mock<IProductRepository>();

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.PostAsync(new Product() { Id = "123", Name = "Test", Description = "Test Description", Price = 11.90m });

            Assert.IsAssignableFrom<OkResult>(response);
            var result = response as OkResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async void Post_AddProduct_BadRequest()
        {
            var mockRepository = new Mock<IProductRepository>();

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.PostAsync(null);

            Assert.IsAssignableFrom<BadRequestResult>(response);
            var result = response as BadRequestResult;
            Assert.NotNull(result);
            Assert.True(result.StatusCode == 400);
        }

        [Fact]
        public async void Get_GetProductById_Ok()
        {
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(99)).Returns(Task.FromResult<Product>(new Product() { Id = "99" }));

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.GetAsync(99);

            Assert.IsAssignableFrom<OkObjectResult>(response);
            OkObjectResult result = response as OkObjectResult;
            Assert.True(result.StatusCode == 200);
            Assert.NotNull(result);

            var product = result.Value as Product;

            Assert.NotNull(product);
            Assert.True(product.Id == "99");
        }

        [Fact]
        public async void Get_GetProductById_NotFound()
        {
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(99)).Returns(Task.FromResult<Product>(null));

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.GetAsync(99);

            Assert.IsAssignableFrom<NotFoundResult>(response);
            NotFoundResult result = response as NotFoundResult;
            Assert.True(result.StatusCode == 404);
        }

        [Fact]
        public async void Put_Update_Ok()
        {
            var mockRepository = new Mock<IProductRepository>();

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.PutAsync(99, new Product() { Id = "99", Name = "new name", Description = "new description", Price = 10.0m });

            Assert.IsAssignableFrom<OkResult>(response);
            OkResult result = response as OkResult;
            Assert.True(result.StatusCode == 200);
        }

        [Fact]
        public async void Delete_Remove_Ok()
        {
            var mockRepository = new Mock<IProductRepository>();

            var productsController = new ProductsController(mockRepository.Object);

            var response = await productsController.DeleteAsync(99);

            Assert.IsAssignableFrom<OkResult>(response);
            OkResult result = response as OkResult;
            Assert.True(result.StatusCode == 200);
        }
    }
}
