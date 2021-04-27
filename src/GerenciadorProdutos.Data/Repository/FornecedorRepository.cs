using GerenciadorProdutos.Business.Intefaces;
using GerenciadorProdutos.Business.Models;
using GerenciadorProdutos.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GerenciadorProdutos.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(GerenciadorContext context) : base(context)
        {
        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id) => await Db.Fornecedores.AsNoTracking()
                                                                                .Include(c => c.Endereco)
                                                                                .FirstOrDefaultAsync(c => c.Id == id);


        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id) => await Db.Fornecedores.AsNoTracking()
                                                                                        .Include(c => c.Produtos)
                                                                                        .Include(c => c.Endereco).FirstOrDefaultAsync(c => c.Id == id);
    }
}
