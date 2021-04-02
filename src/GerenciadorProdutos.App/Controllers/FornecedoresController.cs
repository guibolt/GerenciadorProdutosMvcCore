using System;
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

        public FornecedoresController(IFornecedorRepository fornecedorRepository ,IMapper mapper) : base(mapper)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            var fornecedoresViewModel = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return View(fornecedoresViewModel);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.Adicionar(fornecedor);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.Atualizar(fornecedor);

            return View(fornecedorViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            await _fornecedorRepository.Remover(id);

            return RedirectToAction(nameof(Index));
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
