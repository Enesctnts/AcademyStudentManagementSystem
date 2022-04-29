using ASMSBusinessLayer.ContractsBLL;
using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Models;
using ASMSEntityLayer.ResultModels;
using ASMSEntityLayer.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSBusinessLayer.ImplementationsBLL
{
    public class CityBusinessEngine : ICityBusinessEngine
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;

        public CityBusinessEngine(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public IResult Add(CityVM city)
        {
            try
            {
                City newCity = _mapper.Map<CityVM,City>(city);
                var insertresult=_unitofWork.CityRepo.Add(newCity);
                return insertresult ? 
                    new SuccessResult("İl eklendi") : 
                    new ErrorResult("İl eklemede hata oluştu,Tekrar deneyiniz");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IResult Delete(CityVM city)
        {
            throw new NotImplementedException();
        }

        public IDataResult<ICollection<CityVM>> GetAll()
        {
            try
            {

                //select* from Cities inner join Distircts
                var cities = _unitofWork.CityRepo.GetAll(x => !x.IsDeleted, includeEntities: "Districts");
                ICollection<CityVM> allCities = _mapper.Map<IQueryable<City>, ICollection<CityVM>>(cities);

                return new SuccessDataResult<ICollection<CityVM>>(allCities, $"{allCities.Count} adet il listelendi");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IDataResult<CityVM> GetById(int city)
        {
            throw new NotImplementedException();
        }

        public IDataResult<ICollection<CityVM>> GetFirstOrDefault()
        {
            throw new NotImplementedException();
        }

        public IResult Update(CityVM city)
        {
            throw new NotImplementedException();
        }
    }
}
