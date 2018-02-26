using PrayashStore.UOW.Interfaces;

namespace PrayashStore.Services
{
    public class BaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }
    }
}