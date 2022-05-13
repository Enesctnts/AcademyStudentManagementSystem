using ASMSBusinessLayer.ContractsBLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Controllers
{
    public class NeighbourhoodController : Controller
    {
        private readonly INeighbourhoodBusinessEngine _neighbourhoodEngine;

        public NeighbourhoodController(INeighbourhoodBusinessEngine neighbourhoodEngine)
        {
            _neighbourhoodEngine = neighbourhoodEngine;
        }

        public JsonResult GetDistrictNeigbourhoods(int districtId)
        {
            try
            {
                var data = _neighbourhoodEngine.GetNeighbourhoodsOfDistrict(districtId).Data;
                return Json(new { isSuccess = true,data });
            }
            catch (Exception ex)
            {

                //ex loglanacak
                return Json(new { isSuccess = false });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
