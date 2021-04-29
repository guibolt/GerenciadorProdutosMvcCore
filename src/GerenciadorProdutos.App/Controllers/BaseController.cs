using AutoMapper;
using GerenciadorProdutos.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProdutos.App.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IMapper _mapper;
        private readonly INotificador _notificador;

        protected BaseController(IMapper mapper, INotificador notificador)
        {
            _mapper = mapper;
            _notificador = notificador;
        }
        protected bool OperacaoValida() => !_notificador.TemNotificacao();
    }
}
