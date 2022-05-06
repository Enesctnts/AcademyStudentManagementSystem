using ASMSDataAccessLayer.ContactsDAL;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMSBusinessLayer.ContractsBLL;
using ASMSEntityLayer.ViewModels;
using ASMSEntityLayer.ResultModels;
using ASMSEntityLayer.Models;

namespace ASMSBusinessLayer.ImplementationsBLL
{
    public class NeighbourhoodBusinessEngine : INeighbourhoodBusinessEngine
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        public NeighbourhoodBusinessEngine(IUnitOfWork unitofWork,
            IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }


        public IDataResult<ICollection<NeighbourhoodVM>> GetNeighbourhoodsOfDistrict(int districtId)
        {
            try
            {
                if (districtId > 0)
                {
                    var neighbourhoods = _unitofWork.NeighbourhoodRepo.
                            GetAll(x => x.DistrictId == districtId);
                    ICollection<NeighbourhoodVM> result =
                        _mapper.Map<IQueryable<Neighbourhood>, ICollection<NeighbourhoodVM>>(neighbourhoods);

                    return new SuccessDataResult<ICollection<NeighbourhoodVM>>(result);

                }
                else
                {
                    throw new Exception("DistrictId düzgün gelmedi!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
