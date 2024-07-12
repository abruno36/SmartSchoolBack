using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V1.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.WebAPI.V1.Controllers
{
    /// <summary>
    /// Versão 1.0 do meu controlador de Professores
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        public ProfessorController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Método responsável para retornar todos os professores ativos 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParamsProf pageParams)
        {
            var professor = await _repo.GetAllProfessoresAsync(pageParams);

            Response.AddPagination(professor.CurrentPage, professor.PageSize, professor.TotalCount, professor.TotalPages);

            return Ok(professor);
        }

        /// <summary>
        /// Método responsável por retornar apenas um único ProfessorDTO.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new ProfessorRegistrarDto());
        }
        /// <summary>
        /// Método responsável por retornar apenas um Professor por meio do Código ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // api/Professor
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var Professor = _repo.GetProfessorById(id, true);
            if (Professor == null) return BadRequest($"Professor(a) {id} não foi encontrado(a)");

            var professorDto = _mapper.Map<ProfessorDto>(Professor);

            return Ok(professorDto);
        }

        /// <summary>
        /// Método responsável por retornar apenas por aluno ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // api/Professor
        [HttpGet("byaluno/{alunoId}")]
        public IActionResult GetByAlunoId(int alunoId)
        {
            var Professores = _repo.GetProfessoresByAlunoId(alunoId, true);
            if (Professores == null) return BadRequest("Professores não encontrados");

            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(Professores));
        }

        /// <summary>
        /// Método responsável por retornar todos os professores por disciplinas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // api/Professor
        [HttpGet("bydisciplina/{disciplinaId}")]
        public IActionResult GetAllProfessoresDisciplinaId(int disciplinaId)
        {
            var Professores = _repo.GetAllProfessoresByDisciplinaId(disciplinaId, true);
            if (Professores == null) return BadRequest("Professores não encontrados");

            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(Professores));
        }

        /// <summary>
        /// Método responsável por inserir um novo professor.
        /// </summary>
        /// <returns></returns>
        // api/Professor
        [HttpPost]
        public IActionResult Post(ProfessorRegistrarDto model)
        {
            var prof = _mapper.Map<Professor>(model);

            _repo.Add(prof);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
            }

            return BadRequest($"Professor(a) {model.Nome} não foi encontrado(a)");
        }

        /// <summary>
        /// Método responsável por alterar um professor.
        /// </summary>
        /// <returns></returns>
        // api/Professor
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProfessorRegistrarDto model)
        {
            var prof = _repo.GetProfessorById(id, false);
            if (prof == null) return BadRequest($"Professor(a) {id} não foi encontrado(a)");

            _mapper.Map(model, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
            }

            return BadRequest($"Professor(a) {id} não foi atualizado(a)");
        }

        /// <summary>
        /// Método responsável por alterar um professor.
        /// </summary>
        /// <returns></returns>
        // api/Professor
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistrarDto model)
        {
            var prof = _repo.GetProfessorById(id, false);
            if (prof == null) return BadRequest($"Professor(a) {id} não foi encontrado(a)");

            _mapper.Map(model, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
            }

            return BadRequest($"Professor(a) {id} não foi atualizado(a)");
        }

        /// <summary>
        /// Método responsável por deletar um professor.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prof = _repo.GetProfessorById(id, false);
            if (prof == null) return BadRequest($"Professor(a) {id} não foi encontrado(a)");

            _repo.Delete(prof);
            if (_repo.SaveChanges())
            {
                return Ok($"professor(a) {prof.Id} deletado(a)");
            }

            return BadRequest($"Professoe(a) {prof.Nome} não deletado(a)");
        }
    }
}