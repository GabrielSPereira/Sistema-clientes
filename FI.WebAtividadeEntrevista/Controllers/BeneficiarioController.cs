using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        [HttpGet]
        public JsonResult ListaPorCliente(long idCliente)
        {
            try
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().ListaPorCliente(idCliente);

                return Json(new { Result = "OK", Records = beneficiarios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}