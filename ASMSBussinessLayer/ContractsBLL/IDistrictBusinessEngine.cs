using ASMSEntityLayer.ResultModels;
using ASMSEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSBusinessLayer.ContractsBLL
{
    public interface IDistrictBusinessEngine
    {

        // Buraya ekleme silme güncelleme vb metot imzaları 
        // yazılabilir

        // Biz şuanda sadece ihtiyacımız olanları yazalım

        /// <summary>
        /// This method will return all distrcits.
        /// Bu metot tüm ilçeleri getirir
        /// </summary>
        /// <returns></returns>
        IDataResult<ICollection<DistrictVM>> GetAllDistricts();
        /// <summary>
        /// This method will give districts of a city
        /// Örneğin, istanbu'un ilçeleri
        /// </summary>
        /// <param name="cityId">Buraya byte türünde değer gönderiniz</param>
        /// <returns></returns>
        IDataResult<ICollection<DistrictVM>> GetDistrictsOfCity(byte cityId);

    }
}
