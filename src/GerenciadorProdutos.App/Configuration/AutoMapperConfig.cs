using AutoMapper;
using GerenciadorProdutos.App.ViewModels;
using GerenciadorProdutos.Business.Models;

namespace GerenciadorProdutos.App.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();

        }
    }
}
