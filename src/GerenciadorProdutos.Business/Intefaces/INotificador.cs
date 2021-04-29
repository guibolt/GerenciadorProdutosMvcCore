using GerenciadorProdutos.Business.Notifications;
using System.Collections.Generic;

namespace GerenciadorProdutos.Business.Intefaces
{

    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
