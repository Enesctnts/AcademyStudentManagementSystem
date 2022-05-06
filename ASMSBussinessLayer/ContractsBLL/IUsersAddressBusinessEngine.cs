using ASMSEntityLayer.ResultModels;
using ASMSEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSBusinessLayer.ContractsBLL
{
    public interface IUsersAddressBusinessEngine
    {
        // Ekleme *
        // Düzenleme
        // Silme
        // Listeleme *
        IResult Add(UsersAddressVM address);
        IDataResult<ICollection<UsersAddressVM>> GetAll(string userId);

    }
}
