using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProdutos.App.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IMapper _mapper;

        protected BaseController(IMapper mapper) => _mapper = mapper;
    }
}
