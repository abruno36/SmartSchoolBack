using System;

namespace SmartSchool.WebAPI.V2.Dtos
{
    /// <summary>
    /// Este é o DTO de mudança de estado do Aluno (ativo).
    /// </summary>
    public class TrocaEstadoDto
    {
        public bool Estado { get; set; }
    }
}