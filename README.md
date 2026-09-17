# SpaFlow — Sistema de gestión para SPA

Starter comercial construido para **ASP.NET Core 10 + C# + Razor Pages + Entity Framework Core + SQL Server + ASP.NET Core Identity**.

La solución está pensada para evolucionar desde un SPA de una sola sucursal hasta un producto revendible con varias sucursales y múltiples clientes/negocios.

## Qué incluye actualmente

- Autenticación con ASP.NET Core Identity.
- Roles preparados: `SuperAdmin`, `Admin`, `Manager`, `Receptionist`, `Therapist`, `Cashier`.
- Entidad de negocio (`SpaBusiness`) y sucursales.
- Dashboard con citas, clientes, ingresos y alertas de stock.
- Agenda de reservas.
- Alta de reservas internas.
- Reserva web pública para clientes.
- Validación de choques de horario por profesional y cabina.
- Clientes y ficha básica.
- Servicios, categorías, duración, precio y comisión.
- Profesionales, especialidad y servicios asignables.
- Cabinas/salas.
- Inventario, productos y movimientos de stock.
- Ventas y pagos modelados.
- Gastos modelados.
- Membresías y gift cards modeladas.
- Reportes base.
- Configuración del negocio.
- Auditoría modelada.
- Diseño responsive.

## Tecnologías

- .NET 10
- ASP.NET Core Razor Pages
- C#
- Entity Framework Core 10
- SQL Server / SQL Server Express
- ASP.NET Core Identity
- HTML + CSS + JavaScript

## Requisitos locales

1. Visual Studio 2022/2025 o VS Code.
2. SDK de .NET 10.
3. SQL Server 2022/2025 o SQL Server Express.
4. Opcional: SQL Server Management Studio.

## Configuración de base de datos

Por defecto usa:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SpaFlowDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Si tu instancia es distinta, cambia `SpaFlow.Web/appsettings.json`.

Ejemplos:

```text
Server=.;Database=SpaFlowDb;Trusted_Connection=True;TrustServerCertificate=True
Server=localhost;Database=SpaFlowDb;User Id=sa;Password=TU_CLAVE;TrustServerCertificate=True
```

## Primer arranque

Desde la carpeta de la solución:

```bash
dotnet restore
dotnet run --project SpaFlow.Web
```

El inicializador crea la base automáticamente cuando no existen migraciones. Para un despliegue comercial, usa migraciones:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project SpaFlow.Web
dotnet ef database update --project SpaFlow.Web
```

> Si ya arrancaste con `EnsureCreated` durante desarrollo y luego decides adoptar migraciones, lo más limpio es eliminar la base de desarrollo y recrearla con la primera migración.

## Credencial inicial de DEMO

- Correo: `admin@spaflow.local`
- Contraseña: `ChangeMe123!`

**Cámbiala antes de publicar el sistema.** También debes mover las credenciales de producción a variables de entorno, User Secrets o un gestor de secretos.

## URLs principales

- `/` — portada.
- `/Identity/Account/Login` — acceso.
- `/Dashboard` — panel administrativo.
- `/Appointments` — agenda.
- `/Appointments/Create` — nueva reserva.
- `/Clients` — clientes.
- `/Services` — servicios.
- `/Employees` — personal.
- `/Inventory` — inventario.
- `/Reports` — reportes.
- `/Settings` — configuración.
- `/Booking` — reserva pública.

## Flujo recomendado del SPA

1. Recepción registra o busca al cliente.
2. Selecciona tratamiento, profesional, cabina y horario.
3. El sistema valida que no exista un cruce.
4. Reserva queda `Pending` o `Confirmed`.
5. Al llegar: `CheckedIn`.
6. Al iniciar: `InService`.
7. Al finalizar: `Completed`.
8. Se registra pago y, si corresponde, consumo de inventario y comisión.
9. Dashboard y reportes se actualizan.

## Para venderlo a varios SPA

No cambies el código para cada cliente. Conserva `SpaBusiness` como entidad raíz y configura:

- nombre comercial;
- logo;
- paleta visual;
- sucursales;
- servicios;
- profesionales;
- horarios;
- moneda;
- políticas de reserva;
- mensajes y recordatorios.

Esto permite convertir SpaFlow en un SaaS o en una instalación independiente por cliente.

## Próximas funciones prioritarias

Ver `docs/PLAN_PRODUCTO.md`.
