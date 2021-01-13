using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public async static Task InsertarData(CursosOnlineContext context, UserManager<Usuario> userManager){
            if(!userManager.Users.Any()){
                var usuario = new Usuario {
                    NombreCompleto = "Alejandro Fredes",
                    UserName = "adfredes",
                    Email = "adfredes@gmail.com"
                };
                var result = await userManager.CreateAsync(usuario, "Pa$$w0rd");                
            }
        }
    }
}