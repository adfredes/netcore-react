using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace Persistencia
{
    public class CursosOnlineContext: IdentityDbContext<Usuario>
    {
        private const string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=CursosOnline;Integrated Security=True";
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        

        public CursosOnlineContext(DbContextOptions options): base(options){

        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.CursoId, ci.InstructorId});            
            // modelBuilder.Entity<CursoInstructor>()
            //     .HasOne<Instructor>(ci => ci.Instructor)
            //     .WithMany(i => i.CursoLink)
            //     .HasForeignKey( ci => ci.InstructorId);
        }
    }
}