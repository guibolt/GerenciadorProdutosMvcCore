using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GerenciadorProdutos.App.ViewModels;

namespace GerenciadorProdutos.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GerenciadorProdutos.App.ViewModels.EnderecoViewModel> EnderecoViewModel { get; set; }
    }
}
