using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace DB
{
    public class PresupuestoContext: DbContext
    {
        public PresupuestoContext(DbContextOptions<PresupuestoContext> options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Ahorro> Ahorros { get; set; }
        public DbSet<MetaAhorro> MetaAhorros { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<FuenteIngreso> FuenteIngresos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<CategoriaGasto> CategoriaGastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne<Rol>()
                .WithMany()
                .HasForeignKey(x => x.id_rol);

            //relacion Ahorro MetaAhorro
            modelBuilder.Entity<Ahorro>()
                .HasOne<MetaAhorro>()
                .WithMany()
                .HasForeignKey(x => x.id_meta);
            //relacio Ahorro Usuario
            modelBuilder.Entity<Ahorro>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.id_usuario);

            //relacion Ingreso FuenteIngreso
            modelBuilder.Entity<Ingreso>()
                .HasOne<FuenteIngreso>()
                .WithMany()
                .HasForeignKey(x => x.id_fuente);

            //relcion Ingreso Usuario
            modelBuilder.Entity<Ingreso>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.id_usuario);

            //relacion Gasto CategoriaGasto
            modelBuilder.Entity<Gasto>()
                .HasOne<CategoriaGasto>()
                .WithMany()
                .HasForeignKey(x => x.id_categoria);

            //relacion Gasto Usuairo
            modelBuilder.Entity<Gasto>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.id_usuario);
        }

    }
}