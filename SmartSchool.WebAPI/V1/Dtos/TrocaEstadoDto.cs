using System;

namespace SmartSchool.WebAPI.V1.Dtos
{
    /// <summary>
    /// Este � o DTO de mudan�a de estado do Aluno (ativo).
    /// </summary>
    public class TrocaEstadoDto
    {
        public bool Estado { get; set; }
    }
}