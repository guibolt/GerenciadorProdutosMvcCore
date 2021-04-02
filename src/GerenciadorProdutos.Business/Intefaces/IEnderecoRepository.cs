using GerenciadorProdutos.Business.Models;
using System;
using System.Threading.Tasks;

namespace GerenciadorProdutos.Business.Intefaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
