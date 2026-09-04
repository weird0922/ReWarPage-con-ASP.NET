# ReWear

Marketplace web de ropa de segunda mano. Los usuarios pueden publicar prendas usadas y explorar las que están disponibles para darles una segunda vida.

**Stack:** ASP.NET Core MVC (.NET 8) + Entity Framework Core 8 + SQL Server.

## Cómo ejecutar el proyecto

```bash
dotnet run --project ReWearWeb
```

No hace falta correr ningún comando de EF Core a mano. Al arrancar, `Program.cs` llama a `Database.Migrate()`, que crea la base de datos y aplica las migraciones automáticamente si todavía no existen — incluidos los datos de prueba. Solo se necesita tener accesible el servidor indicado en `ConnectionStrings:DefaultConnection` de `ReWearWeb/appsettings.json` (por defecto, una instancia local de SQL Server con autenticación de Windows).

Rutas principales:
- `/` — página de inicio.
- `/Prendas` — listado de prendas disponibles (vista de solo lectura).

## Estructura del proyecto

- **Models/** — clases de entidad (`Prenda`, `Categoria`, `Usuario`) con sus propiedades y validaciones.
- **Data/** — `AppDbContext`, que expone los `DbSet` y configura relaciones, índices y datos semilla con Fluent API.
- **Migrations/** — migraciones de EF Core generadas a partir del modelo en código.
- **Controllers/** — lógica de las páginas (por ahora, solo lectura de datos).
- **Views/** — plantillas Razor que renderizan lo que devuelven los controllers.

## Justificación del enfoque: Code First vs. Database First vs. Model First

EF Core ofrece tres enfoques para relacionar el modelo de datos con la base de datos. Antes de empezar a codear se evaluaron los tres:

### Code First (elegido)

El modelo de clases y la configuración de EF son la fuente principal: el equipo define entidades y relaciones en código, y usa migraciones para crear o evolucionar el esquema de la base de datos.

- **Ventajas:** cambios versionables con Git; migraciones reproducibles; integración natural con clases C# y pruebas.
- **Limitaciones:** una migración incorrecta puede generar riesgos en producción; exige disciplina para revisar el SQL generado y coordinación con el DBA.
- **Conviene cuando:** el proyecto es nuevo, el equipo controla el esquema y hay un pipeline de despliegue automatizado.

### Database First (descartado)

La base de datos existente es la fuente principal. EF Core hace ingeniería inversa (scaffolding) sobre un esquema ya creado para generar el `DbContext` y las clases de entidad.

- **Ventajas:** permite trabajar sobre una base madura sin rediseñarla; aprovecha objetos y relaciones ya existentes.
- **Limitaciones:** el re-scaffolding puede sobrescribir código generado; no todos los conceptos de un modelo orientado a objetos se deducen del esquema.
- **Conviene cuando:** hay sistemas heredados, bases administradas por un DBA, o integraciones con esquemas compartidos.
- **Por qué se descartó:** ReWear arrancó como proyecto nuevo, sin ninguna base de datos preexistente de la cual partir — no había nada que "hacer scaffolding".

### Model First clásico (descartado)

Se diseña primero un modelo conceptual visual, y a partir de ese modelo se genera tanto la base de datos como el código. Estuvo asociado a Entity Framework 6 y a archivos EDMX.

- **Ventajas:** facilita discutir un modelo conceptual visual antes de implementar.
- **Limitaciones:** no es un flujo nativo de EF Core moderno; puede generar una falsa expectativa si se confunde con el EDMX de EF6.
- **Conviene cuando:** se usa como técnica de análisis conceptual, o con tecnologías que sí soportan ese flujo (EF6 clásico).
- **Por qué se descartó:** el proyecto usa EF Core 8, donde este flujo ya no existe de forma nativa.

### Conclusión

Se eligió **Code First** porque el proyecto arrancó de cero (sin base de datos previa, descartando Database First), el equipo necesita controlar y versionar el esquema junto con el código en el mismo repositorio Git (algo que Model First no ofrece en EF Core moderno), y el flujo `entidades en C# → migración → base de datos` es el que mejor se integra con .NET 8 y con las herramientas de EF Core 8 usadas en el proyecto (`Microsoft.EntityFrameworkCore.SqlServer` y `Microsoft.EntityFrameworkCore.Design`).
