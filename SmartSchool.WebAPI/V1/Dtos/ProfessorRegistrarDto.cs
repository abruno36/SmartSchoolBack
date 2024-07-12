using System;

namespace SmartSchool.WebAPI.V1.Dtos
{
    /// <summary>
    /// Este é o DTO de Professor para Registrar.
    /// </summary>
    public class ProfessorRegistrarDto
    {
        /// <summary>
        /// Identificador e chave do Banco.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Registro do Professor, para outros negócios na Instituição.
        /// </summary>
        public int Registro { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;
    }
}