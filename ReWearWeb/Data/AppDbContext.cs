using Microsoft.EntityFrameworkCore;
using ReWearWeb.Models;

namespace ReWearWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Prenda> Prendas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prenda>(entity =>
        {
            entity.HasOne(p => p.Vendedor)
                  .WithMany(u => u.PrendasEnVenta)
                  .HasForeignKey(p => p.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Categoria)
                  .WithMany(c => c.Prendas)
                  .HasForeignKey(p => p.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(p => p.Precio)
                  .HasPrecision(10, 2);

            entity.HasIndex(p => p.CategoriaId);
            entity.HasIndex(p => p.UsuarioId);
            entity.HasIndex(p => p.EstaDisponible);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.FechaRegistro).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasIndex(c => c.Nombre).IsUnique();

            entity.HasData(
                new Categoria { Id = 1, Nombre = "Camisas", Descripcion = "Camisas casuales y formales" },
                new Categoria { Id = 2, Nombre = "Pantalones", Descripcion = "Jeans, pantalones casuales y formales" },
                new Categoria { Id = 3, Nombre = "Vestidos", Descripcion = "Vestidos de todo tipo" },
                new Categoria { Id = 4, Nombre = "Zapatos", Descripcion = "Zapatillas, zapatos formales y casuales" }
            );
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "María",
                    Apellidos = "García López",
                    Email = "maria@rewear.pe",
                    Password = "Rewear2024",
                    Telefono = "987654321",
                    FechaRegistro = new DateTime(2024, 1, 15)
                },
                new Usuario
                {
                    Id = 2,
                    Nombre = "Carlos",
                    Apellidos = "Ramírez Paz",
                    Email = "carlos@rewear.pe",
                    Password = "Rewear2024",
                    Telefono = "912345678",
                    FechaRegistro = new DateTime(2024, 2, 10)
                },
                new Usuario
                {
                    Id = 3,
                    Nombre = "Ana",
                    Apellidos = "Torres Quispe",
                    Email = "ana@rewear.pe",
                    Password = "Rewear2024",
                    Telefono = "955111222",
                    FechaRegistro = new DateTime(2024, 3, 5)
                }
            );
        });

        modelBuilder.Entity<Prenda>(entity =>
        {
            entity.HasData(
                new Prenda
                {
                    Id = 1,
                    Titulo = "Camisa blanca de algodón",
                    Descripcion = "Camisa blanca de manga larga, talla M, pocas veces usada. Ideal para trabajo o eventos casuales.",
                    Precio = 35.00m,
                    Talla = "M",
                    Estado = "Usado - Bueno",
                    Marca = "Zara",
                    Color = "Blanco",
                    FechaPublicacion = new DateTime(2024, 6, 1),
                    EstaDisponible = true,
                    UsuarioId = 1,
                    CategoriaId = 1
                },
                new Prenda
                {
                    Id = 2,
                    Titulo = "Jeans azul clásico",
                    Descripcion = "Jeans azul oscuro, talla 32, corte recto. Muy cómodos y sin desgaste visible.",
                    Precio = 55.00m,
                    Talla = "32",
                    Estado = "Usado - Excelente",
                    Marca = "Levi's",
                    Color = "Azul oscuro",
                    FechaPublicacion = new DateTime(2024, 6, 5),
                    EstaDisponible = true,
                    UsuarioId = 2,
                    CategoriaId = 2
                },
                new Prenda
                {
                    Id = 3,
                    Titulo = "Vestido floral de verano",
                    Descripcion = "Vestido corto con estampado floral, material liviano. Perfecto para el verano.",
                    Precio = 48.00m,
                    Talla = "S",
                    Estado = "Nuevo con etiqueta",
                    Marca = "Mango",
                    Color = "Estampado",
                    FechaPublicacion = new DateTime(2024, 6, 10),
                    EstaDisponible = true,
                    UsuarioId = 3,
                    CategoriaId = 3
                },
                new Prenda
                {
                    Id = 4,
                    Titulo = "Zapatillas blancas casuales",
                    Descripcion = "Zapatillas blancas de cuero sintético, talla 40. Usadas solo un par de veces.",
                    Precio = 80.00m,
                    Talla = "40",
                    Estado = "Usado - Muy bueno",
                    Marca = "Nike",
                    Color = "Blanco",
                    FechaPublicacion = new DateTime(2024, 6, 12),
                    EstaDisponible = true,
                    UsuarioId = 1,
                    CategoriaId = 4
                },
                new Prenda
                {
                    Id = 5,
                    Titulo = "Camisa a cuadros estilo vintage",
                    Descripcion = "Camisa de franela a cuadros rojos y negros, estilo vintage, talla L.",
                    Precio = 42.00m,
                    Talla = "L",
                    Estado = "Usado - Bueno",
                    Marca = "Hollister",
                    Color = "Rojo/Negro",
                    FechaPublicacion = new DateTime(2024, 6, 14),
                    EstaDisponible = true,
                    UsuarioId = 2,
                    CategoriaId = 1
                }
            );
        });
    }
}
