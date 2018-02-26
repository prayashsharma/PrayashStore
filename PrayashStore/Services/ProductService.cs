using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System;
using System.Collections.Generic;

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

        public IEnumerable<Product> GetAllProducts()
        {
            return ProductRepository.GetAll();
        }

        public IEnumerable<Product> GetAllProductsByCategoryId()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProductsByCategoryId(int id)
        {
            return ProductRepository.Find(x => x.CategoryId == id);
        }

        public IEnumerable<Product> GetAllProductsByCategoryIdAndGender(int id, string gender)
        {
            IEnumerable<Product> products = new List<Product>();
            if (!String.IsNullOrWhiteSpace(gender))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                return ProductRepository.Find(p => p.CategoryId == id && p.Gender == genderEnum);
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

        public Product GetProductById(int id)
        {
            return ProductRepository.Get(id);
        }
    }
}