﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GerenciadorProdutos.App.ViewModels;
using GerenciadorProdutos.Business.Intefaces;
using AutoMapper;
using GerenciadorProdutos.Business.Models;

namespace GerenciadorProdutos.App.Controllers
{
    public class FornecedoresController : BaseController
    {

        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService,
            IMapper mapper, INotificador notificador) : base(mapper,notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
        }

        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            var fornecedoresViewModel = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return View(fornecedoresViewModel);
        }

        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [Route("novo-fornecedor")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Route("novo-fornecedor")]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction("Index");
        }

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(await ObterFornecedorProdutosEndereco(id));

            return View(fornecedorViewModel);
        }
        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            await _fornecedorService.Remover(id);
            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));
        }
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();


            return PartialView("_DetalhesEndereco", fornecedor);
        }
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            var enderecoMapped = _mapper.Map<Endereco>(fornecedorViewModel.Endereco);
            await _fornecedorService.AtualizarEndereco(enderecoMapped);

            if (!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });
            return Json(new { success = true, url });
        }
        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            var fornecedores = await _fornecedorRepository.ObterFornecedorEndereco(id);
            var fornecedoresViewModel = _mapper.Map<FornecedorViewModel>(fornecedores);

            return fornecedoresViewModel;
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            var fornecedores = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
            var fornecedoresViewModel = _mapper.Map<FornecedorViewModel>(fornecedores);

            return fornecedoresViewModel;
        }
    }
}
