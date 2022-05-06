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
    public class UsersAddressBusinessEngine
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersAddressBusinessEngine(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IResult Add(UsersAddressVM address)
        {
            try
            {
                UsersAddress newAddress =
                    _mapper.Map<UsersAddressVM, UsersAddress>(address);
                return _unitOfWork.UsersAddressRepo.Add(newAddress) ?
                        new SuccessResult("Adres Eklendi") :
                        new ErrorResult("Adres Eklenemedi");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IDataResult<ICollection<UsersAddressVM>> GetAll(string userId)
        {
            try
            {
                if (userId != null)
                {
                    var userAddressList = _unitOfWork
                        .UsersAddressRepo.GetAll(x => x.UserId == userId);
                    var result = _mapper.Map<IQueryable<UsersAddress>, ICollection<UsersAddressVM>>(userAddressList);

                    return new SuccessDataResult<ICollection<UsersAddressVM>>(result, $"{result.Count} address has been found");

                }
                else
                {
                    throw new Exception("userId is null so that could not able to find useraddress!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
