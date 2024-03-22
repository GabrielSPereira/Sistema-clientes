using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.Clientes.DaoCliente cli = new DAL.Clientes.DaoCliente();
            var idCliente = cli.Incluir(cliente);
            DAL.Beneficiarios.DaoBeneficiario ben = new DAL.Beneficiarios.DaoBeneficiario();
            foreach (var beneficiario in cliente.Beneficiarios)
            {
                beneficiario.IdCliente = idCliente;
                ben.Incluir(beneficiario);
            }

            return idCliente;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.Clientes.DaoCliente cli = new DAL.Clientes.DaoCliente();
            cli.Alterar(cliente);

            DAL.Beneficiarios.DaoBeneficiario ben = new DAL.Beneficiarios.DaoBeneficiario();

            var idsBeneficiariosPorClienteNoBanco = ben.Listar(cliente.Id).Select(x => x.Id).ToHashSet();
            var idsBeneficiarios = cliente.Beneficiarios.Select(x => x.Id).ToHashSet();
            var idsBeneficiariosParaExcluir = idsBeneficiariosPorClienteNoBanco.Except(idsBeneficiarios);

            foreach(var id in idsBeneficiariosParaExcluir)
            {
                ben.Excluir(id);
            }

            foreach (var beneficiario in cliente.Beneficiarios)
            {
                if (beneficiario.Id > 0)
                {
                    ben.Alterar(beneficiario.Id, beneficiario);
                }
                else
                {
                    beneficiario.IdCliente = cliente.Id;
                    ben.Incluir(beneficiario);
                }
            }
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.Clientes.DaoCliente cli = new DAL.Clientes.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Clientes.DaoCliente cli = new DAL.Clientes.DaoCliente();

            new DaoBeneficiario().ExcluirBeneficiariosDoCliente(id);

            new DAL.Clientes.DaoCliente().Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.Clientes.DaoCliente cli = new DAL.Clientes.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }
    }
}
