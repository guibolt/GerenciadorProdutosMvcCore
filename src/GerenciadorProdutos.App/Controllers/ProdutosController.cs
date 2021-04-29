using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GerenciadorProdutos.App.ViewModels;
using GerenciadorProdutos.Business.Intefaces;
using AutoMapper;
using GerenciadorProdutos.Business.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GerenciadorProdutos.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoService _produtoService;
        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository,
            IProdutoService produtoService, INotificador notificador, IMapper mapper) : base(mapper, notificador)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _produtoService = produtoService;
        }

        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.ObterTodos();
            var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);

            return View(produtosViewModel);
        }
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var imgPreFixo = $"{Guid.NewGuid()}_";
            
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPreFixo))
                return View(produtoViewModel);

            produtoViewModel.Imagem = $"{imgPreFixo}{produtoViewModel.ImagemUpload.FileName}";

            var produto = _mapper.Map<Produto>(produtoViewModel);
            await _produtoService.Adicionar(produto);

            if (!OperacaoValida()) return View(produto);

            return RedirectToAction("Index");

        }
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
            produtoViewModel.Imagem = produtoViewModel.Imagem;

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                    return View(produtoViewModel);
                
                produtoAtualizacao.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            var produto = _mapper.Map<Produto>(produtoViewModel);
            await _produtoService.Atualizar(produto);

            if (!OperacaoValida()) return View(produto);

            return RedirectToAction("Index");
        }

        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            await _produtoService.Remover(id);

            if (!OperacaoValida()) return View(produtoViewModel);

            TempData["Sucesso"] = "Produto excluido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(produto);

            var fornecedores = await _fornecedorRepository.ObterTodos();
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return produtoViewModel;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoViewModel)
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return produtoViewModel;
        }
        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
