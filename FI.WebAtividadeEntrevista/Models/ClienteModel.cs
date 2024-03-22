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
    public class ClienteModel
    {
        private string cpf;
        private string cep;

        public long Id { get; set; }
        
        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "CEP inválido. Use o formato 00000-000")]
        public string CEP
        {
            get { return cep; }
            set { cep = FormatarSomenteNumeros(value); }
        }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Required]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        public string CPF
        {
            get { return cpf; }
            set { cpf = FormatarSomenteNumeros(value); }
        }

        /// <summary>
        /// Lista de beneficiários
        /// </summary>
        public List<BeneficiarioModel> Beneficiarios { get; set; }

        /// <summary>
        /// Método para formatar string removendo caracteres especiais
        /// </summary>
        /// <returns>String formatada sem caracteres especiais</returns>
        private string FormatarSomenteNumeros(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
        }

    }
}