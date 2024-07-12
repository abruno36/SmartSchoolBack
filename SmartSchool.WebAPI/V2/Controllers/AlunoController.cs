using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V2.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.WebAPI.V2.Controllers
{
    /// <summary>
    /// Versão 2.0 do meu controlador de Alunos
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlunoController : ControllerBase
    {
        public readonly IRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Método responsável para retornar todos os alunos - com parâmetros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            var alunos = await _repo.GetAllAlunosAsync(pageParams, true);

            var alunosResult = _mapper.Map<IEnumerable<AlunoDto>>(alunos);

            Response.AddPagination(alunos.CurrentPage, alunos.PageSize, alunos.TotalCount, alunos.TotalPages);

            return Ok(alunosResult);
        }

        /// <summary>
        /// Método responsável para retornar todos os alunos ativos - com professores
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllAlunos")]
        public IActionResult GetAllAlunos()
        {
            var alunos = _repo.GetAllAlunos(true, true);
            return Ok(alunos);
        }

        /// <summary>
        /// Método responsável por retornar aluno por disciplina.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ByDisciplina/{id}")]
        public async Task<IActionResult> GetByDisciplinaId(int id)
        {
            var result = await _repo.GetAllAlunosByDisciplinaIdAsync(id, false);
            return Ok(result);
        }

        /// <summary>
        /// Método responsável por retornar apenas um Aluno por meio do Código ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // api/aluno
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest($"Aluno(a) {id} não foi encontrado(a)");

            var alunoDto = _mapper.Map<AlunoRegistrarDto>(aluno);

            return Ok(alunoDto);
        }

        /// <summary>
        /// Método responsável por inserir um novo aluno.
        /// </summary>
        /// <returns></returns>
        // api/aluno
        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest($"Aluno(a) {aluno.Nome} não cadastrado");
        }

        /// <summary>
        /// Método responsável por alterar um aluno.
        /// </summary>
        /// <returns></returns>
        // api/aluno
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest($"Aluno(a) {id} não encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest($"Aluno(a) {aluno.Nome} não Atualizado");
        }

        /// <summary>
        /// Método responsável por alterar o estado(ativo) de um aluno.
        /// </summary>
        /// <returns></returns>
        // api/aluno/{id}/trocarEstado
        [HttpPatch("{id}/trocarEstado")]
        public IActionResult trocarEstado(int id, TrocaEstadoDto trocaEstado)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest($"Aluno(a) {id} não encontrado");

            aluno.Ativo = trocaEstado.Estado;

            aluno.DataFim = aluno.Ativo == false ? DateTime.Now : (DateTime?)null;

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                var msn = aluno.Ativo ? "ativado(a)" : "desativado(a)";
                return Ok(new { message = $"Aluno(a) {aluno.Nome}, {msn} com sucesso!" });
            }

            return BadRequest($"Aluno(a) {aluno.Nome} não Atualizado");
        }

        /// <summary>
        /// Método responsável por deletar um aluno.
        /// </summary>
        /// <returns></returns>
        // api/aluno
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest($"Aluno(a) {id} não encontrado(a)");

            _repo.Delete(aluno);
            if (_repo.SaveChanges())
            {
                return Ok($"Aluno(a) {id} deletado com sucesso");
            }

            return BadRequest($"Aluno(a) {aluno.Nome} não deletado(a)");
        }
    }
}