using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiário</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario ben = new DAL.Beneficiarios.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiários
        /// </summary>
        public List<DML.Beneficiario> ListaPorCliente(long idCliente)
        {
            DAL.Beneficiarios.DaoBeneficiario ben = new DAL.Beneficiarios.DaoBeneficiario();
            return ben.Listar(idCliente);
        }
    }
}
