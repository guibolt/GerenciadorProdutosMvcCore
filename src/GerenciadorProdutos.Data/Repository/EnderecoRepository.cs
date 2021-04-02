using GerenciadorProdutos.Business.Intefaces;
using GerenciadorProdutos.Business.Models;
using GerenciadorProdutos.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace GerenciadorProdutos.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(GerenciadorContext context) : base(context) { }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId) => await Db.Enderecos.AsNoTracking()
                                                                                    .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        
    }
}
