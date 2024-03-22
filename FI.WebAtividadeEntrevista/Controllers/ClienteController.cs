using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Net;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            if (!ModelStateIsValid())
            {
                return BadRequestResponse();
            }

            try
            {
                var cliente = CreateClienteFromModel(model);
                model.Id = new BoCliente().Incluir(cliente);
                return Json("Cadastro efetuado com sucesso");
            }
            catch (CPFDuplicadoException ex)
            {
                return BadRequestResponse(ex.Message);
            }
        }

        private static List<Beneficiario> GeraListaBeneficiarios(ClienteModel model)
        {
            if (model.Beneficiarios is null || !model.Beneficiarios.Any())
            {
                return new List<Beneficiario>();
            }

            List<Beneficiario> beneficiarios = new List<Beneficiario>();

            foreach (var beneficiarioModel in model.Beneficiarios)
            {
                long id = beneficiarioModel.Id.StartsWith("temp_") ? 0 : long.Parse(beneficiarioModel.Id);

                Beneficiario beneficiario = new Beneficiario
                {
                    Nome = beneficiarioModel.Nome,
                    CPF = beneficiarioModel.CPF,
                    Id = id,
                };

                beneficiarios.Add(beneficiario);
            }

            return beneficiarios;
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            if (!ModelStateIsValid())
            {
                return BadRequestResponse();
            }

            try
            {
                var cliente = CreateClienteFromModel(model);
                new BoCliente().Alterar(cliente);
                return Json("Cadastro alterado com sucesso");
            }
            catch (CPFDuplicadoException ex)
            {
                return BadRequestResponse(ex.Message);
            }
        }

        private bool ModelStateIsValid()
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return false;
            }

            return true;
        }

        private JsonResult BadRequestResponse(string errorMessage = null)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(errorMessage != null ? string.Join(Environment.NewLine, errorMessage) : string.Join(Environment.NewLine, errors));
        }

        private Cliente CreateClienteFromModel(ClienteModel model)
        {
            var beneficiarios = GeraListaBeneficiarios(model);
            return new Cliente
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = model.CPF,
                Beneficiarios = beneficiarios
            };
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            Cliente cliente = new BoCliente().Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpDelete]
        public JsonResult Excluir(long id)
        {
            try
            {
                new BoCliente().Excluir(id);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}