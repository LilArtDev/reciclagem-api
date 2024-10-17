﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reciclagem.api.Services;
using Reciclagem.api.Models;
using Reciclagem.api.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;


namespace Reciclagem.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CidadaoController : ControllerBase
    {
        private readonly ICidadaoService _cidadaoService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public CidadaoController(ICidadaoService cidadaoService, IMapper mapper, IEmailService emailService)
        {
            _cidadaoService = cidadaoService;
            _mapper = mapper;
            _emailService = emailService;
        }


        ////busca por todos os cidadãos cadastrados
        //[HttpGet]
        ////caso precise fazer algum teste no código remova esse [Authorize]
        //[Authorize(Roles = "operador, analista, diretor")]
        //public ActionResult<IEnumerable<CidadaoViewModel>> Get()
        //{
        //    var lista = _cidadaoService.ListarCidadaos();
        //    var viewModelList = _mapper.Map<IEnumerable<CidadaoViewModel>>(lista);

        //    if (viewModelList == null)
        //    {
        //        return NoContent();
        //    }
        //    return Ok(viewModelList);
        //}


        //busca por um único cidadão
        [HttpGet("{id}")]
        [Authorize(Roles = "analista, diretor")]
        public ActionResult<CidadaoViewModel> Get([FromRoute] int id)
        {
            var model = _cidadaoService.ObterCidadaoPorId(id);


            if (model == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = _mapper.Map<CidadaoViewModel>(model);
                return Ok(viewModel);
            }
        }

        //Cria um cidadão
        [HttpPost]
        [Authorize(Roles = "analista, diretor")]
        public ActionResult Post([FromBody] CidadaoViewModel viewModel)
        {
            var model = _mapper.Map<CidadaoModel>(viewModel); 
            _cidadaoService.CriarCidadao(model); 

            return CreatedAtAction(nameof(Get), new { id = model.CidadaoId }, model);
        }

        //Deleta um cidadão
        [HttpDelete("{id}")]
        [Authorize(Roles = "analista, diretor")]
        public ActionResult Delete([FromRoute] int id)
        {
            _cidadaoService.DeletarCidadao(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<CidadaoPaginacaoViewModel>> Get([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            var cidadaos = _cidadaoService.ListarCidadaos(pagina, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<CidadaoViewModel>>(cidadaos);

            var viewModel = new CidadaoPaginacaoViewModel
            {
                //Pagina = pagina,
                CurrentPage = pagina,
                PageSize = tamanho
            };

            if (viewModelList == null)
            {
                return NoContent();
            }
            return Ok(viewModel);
        }
        [HttpPost("{id}/enviar-alerta")]
        [Authorize(Roles = "analista, diretor")]
        public ActionResult EnviarAlerta([FromRoute] int id, [FromBody] AlertaViewModel alerta)
        {
            var cidadao = _cidadaoService.ObterCidadaoPorId(id);

            if (cidadao == null)
            {
                return NotFound();
            }

            string assunto = alerta.Assunto;
            string mensagem = alerta.Mensagem;

            try
            {
                _emailService.EnviarEmail(cidadao.Email, assunto, mensagem);
                return Ok("E-mail de alerta enviado com sucesso.");
            }
            catch (Exception ex)
            {
                // Trate o erro conforme necessário
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao enviar e-mail: {ex.Message}");
            }
        }

    }
}
