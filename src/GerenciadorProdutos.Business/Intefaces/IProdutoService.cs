using GerenciadorProdutos.Business.Models;
using System;
using System.Threading.Tasks;

namespace GerenciadorProdutos.Business.Intefaces
{
    public interface IProdutoService : IDisposable
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(Guid id);
    }
}
