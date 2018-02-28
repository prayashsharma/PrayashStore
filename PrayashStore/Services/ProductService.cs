using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrayashStore.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _productRepository = UnitOfWork.GetRepository<Product>();
        }

        public IRepository<Product> ProductRepository
        {
            get { return _productRepository; }
        }

        public void AddProduct(Product product)
        {
            ProductRepository.Add(product);
            UnitOfWork.Complete();
        }

        public void EditProduct(Product product)
        {
            ProductRepository.Edit(product, product.Id);
            UnitOfWork.Complete();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return ProductRepository.GetAll();
        }

        public IEnumerable<Product> GetAllProductsByCategoryId(int categoryId)
        {
            return ProductRepository.Find(x => x.CategoryId == categoryId);
        }

        public IEnumerable<Product> GetAllProductsByCategoryIdAndGender(int categoryId, string gender)
        {
            IEnumerable<Product> products = new List<Product>();
            if (!String.IsNullOrWhiteSpace(gender))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                return ProductRepository.Find(p => p.CategoryId == categoryId && p.Gender == genderEnum);
            }
            return products;
        }

        public IEnumerable<Product> GetAllProductsByCategoryName(string categoryName)
        {
            return ProductRepository.Find(x => x.Category.Name == categoryName);
        }

        public IEnumerable<Product> GetAllProductsByCategoryNameAndGender(string categoryName, string gender)
        {
            IEnumerable<Product> products = new List<Product>();
            if (!String.IsNullOrWhiteSpace(gender))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                return ProductRepository.Find(p => p.Category.Name == categoryName && p.Gender == genderEnum);
            }
            return products;
        }

        public IEnumerable<Product> GetAllProductsByGender(string gender)
        {
            IEnumerable<Product> products = new List<Product>();
            if (!String.IsNullOrWhiteSpace(gender))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                products = ProductRepository.Find(p => p.Gender == genderEnum);
            }
            return products;
        }

        public Product GetProductById(int productId)
        {
            return ProductRepository.Get(productId);
        }

        public void RemoveProduct(Product product)
        {
            ProductRepository.Remove(product);
            UnitOfWork.Complete();
        }

        public Product GetProductWithCategory(int productId)
        {
            return ProductRepository.Include(c => c.Category).SingleOrDefault(x => x.Id == productId);
        }
    }
}