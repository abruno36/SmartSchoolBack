using System;

namespace SmartSchool.WebAPI.V2.Dtos
{
    /// <summary>
    /// Este � o DTO de mudan�a de estado do Aluno (ativo).
    /// </summary>
    public class TrocaEstadoDto
    {
        public bool Estado { get; set; }
    }
}