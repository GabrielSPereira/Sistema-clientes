using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class BeneficiarioModel
    {
        private string cpf;

        /// <summary>
        /// Nome
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        public string CPF
        {
            get { return cpf; }
            set { cpf = FormatarCPF(value); }
        }

        /// <summary>
        /// Método para formatar o CPF removendo caracteres especiais
        /// </summary>
        /// <returns>CPF formatado sem caracteres especiais</returns>
        private string FormatarCPF(string cpf)
        {
            // Remove os caracteres especiais do CPF
            return new string(cpf.Where(char.IsDigit).ToArray());
        }
    }
}