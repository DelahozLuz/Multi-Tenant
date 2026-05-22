# Multi-Tenant

Aplicación SaaS multi-tenant con backend en .NET 9 y frontend en Angular 19.

## Arquitectura

```
┌─────────────┐     ┌──────────────┐     ┌────────────┐
│  Frontend   │────▶│   Backend    │────▶│ SQL Server │
│  Angular 19 │     │  .NET 9 API  │     │   2022     │
│  puerto 8080│     │  puerto 5000 │     │  puerto1433│
└─────────────┘     └──────────────┘     └────────────┘
       │                    │
       │  /api/*            │
       └──── Nginx ─────────┘
       (reverse proxy)
```

```

## Inicio rápido con Docker

```bash
# 1. Clonar el repositorio
git clone https://github.com/DelahozLuz/Multi-Tenant.git
cd Multi-Tenant

2.  **Construir y levantar el entorno completo:**
    Ejecuta el siguiente comando para descargar las imágenes, construir los contenedores y desplegar la aplicación junto con la base de datos:

    ```bash
    # Opción A: En segundo plano (recomendado)
    docker compose up -d --build

    # Opción B: En primer plano 
    docker compose up --build

⚠️ Importante: Este proceso asegura que el entorno sea consistente, ya que cada vez que ejecutas el comando se asegura de levantar la base de datos y aplicar los datos de prueba, garantizando que siempre cuentes con la información necesaria para trabajar.
```

### Credenciales de prueba

```
Email:    admin@gmail.com
Password: admin123
```
```
Email:    test1@ejemplo.com
Password: admin123
```

## Ejecutar en desarrollo (sin Docker)

### Backend

```bash
cd backend

# Restaurar paquetes
dotnet restore o dar click en la solucion dde proyecto

# Aplicar migraciones (requiere SQL Server local)
dotnet ef database update --project src/Infrastructure --startup-project src/API

# Ejecutar
dotnet run --project src/API --launch-profile https
```

La API estará en `https://localhost:7125` y Swagger en `https://localhost:7125/swagger`.

### Frontend

```bash
cd frontend

# Instalar dependencias
npm install

# Ejecutar (apunta a https://localhost:7125/api)
ng serve
```

# El frontend estará en `http://localhost:4200`.


## Datos semilla

Al iniciar, el backend crea la base de datos y la puebla automáticamente con:

### Usuarios

| Email              | Contraseña |
|--------------------|------------|
| admin@gmail.com    | `admin123` |
| test1@ejemplo.com  | `admin123` |

### Credenciales de prueba

```
Email:    admin@gmail.com
Password: admin123
```

## Estructura del proyecto

```
Multi-Tenant/

├── docker-compose.yml         # Orquestación Docker
│
├── backend/
│   ├── Dockerfile
│   ├── SaaSMultiTenant.sln
│   └── src/
│       ├── API/               # Web API (.NET 9)
│       │   ├── Controllers/   # Endpoints REST
│       │   ├── Program.cs     # Punto de entrada
│       │   └── appsettings.json
│       ├── Application/       # Lógica de negocio
│       ├── Domain/            # Entidades del dominio
│       └── Infrastructure/    # Datos (EF Core, repositorios)
│           ├── Data/          # AppDbContext + seed data
│           └── Migrations/    # Migraciones de EF Core
│
├── frontend/
│   ├── Dockerfile
│   ├── nginx.conf             # Configuración de Nginx 
│   └── src/
│       └── app/
│           ├── core/          # Servicios, interceptors, modelos
│           ├── features/      # Componentes de negocio
│           ├── login/         # Login
│           └── projects/      # Gestión de proyectos
│
└── database/                  # Scripts SQL (opcional)
```

## API endpoints

| Método | Ruta                    | Descripción              |
|--------|-------------------------|--------------------------|
| POST   | `/api/Auth/login`       | Iniciar sesión           |
| POST   | `/api/Auth/token`       | Obtener JWT por workspace|
| GET    | `/api/Projects`         | Listar proyectos         |
| POST   | `/api/Projects`         | Crear proyecto           |
| PUT    | `/api/Projects/{id}`    | Actualizar proyecto      |
| DELETE | `/api/Projects/{id}`    | Eliminar proyecto        |
