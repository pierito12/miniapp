# Todo Task API — Mini Aplicación Web

**Curso:** Calidad y Pruebas de Software | **Evaluación:** T3

## Descripción

Mini API REST en ASP.NET Core 8 para gestión de **tareas de iteración**, con pipeline CI/CD en Jenkins.

## Flujo: VS Code → GitHub → Jenkins

```
VS Code (desarrollo) → git push → GitHub (repositorio) → Jenkins (CI/CD)
```

## Estructura del Proyecto

```
TodoApp/
├── TodoApi/                      ← Proyecto principal (Web API)
│   ├── Controllers/
│   │   └── TasksController.cs    ← CRUD completo de tareas
│   ├── Models/
│   │   └── TaskItem.cs           ← Modelo de tarea
│   ├── Program.cs                ← Configuración y middleware
│   └── TodoApi.csproj
├── TodoApi.Tests/                ← Proyecto de pruebas (xUnit)
│   ├── TasksControllerTests.cs   ← 11 pruebas unitarias
│   └── TodoApi.Tests.csproj
├── TodoApp.sln                   ← Solución .NET
└── Jenkinsfile                   ← Pipeline declarativo Jenkins
```

## Comandos .NET

```bash
# 1. Restaurar dependencias
dotnet restore TodoApp.sln

# 2. Compilar
dotnet build TodoApp.sln --configuration Release

# 3. Ejecutar pruebas
dotnet test TodoApp.sln --configuration Release

# 4. Publicar
dotnet publish TodoApi/TodoApi.csproj --configuration Release --output publish

# 5. Ejecutar localmente
dotnet run --project TodoApi
```

## Endpoints de la API

| Método | Ruta                       | Descripción                    |
|--------|----------------------------|--------------------------------|
| GET    | /api/tasks                 | Obtener todas las tareas       |
| GET    | /api/tasks/{id}            | Obtener tarea por ID           |
| GET    | /api/tasks/pending         | Obtener tareas pendientes      |
| POST   | /api/tasks                 | Crear nueva tarea              |
| PUT    | /api/tasks/{id}            | Actualizar tarea               |
| PATCH  | /api/tasks/{id}/complete   | Marcar tarea como completada   |
| DELETE | /api/tasks/{id}            | Eliminar tarea                 |

## Swagger UI

Acceder en: `http://localhost:5000/swagger`

## Etapas del Pipeline Jenkins

1. **Clonar Repositorio** — `checkout scm`
2. **Restaurar Dependencias** — `dotnet restore`
3. **Compilar (Build)** — `dotnet build --configuration Release`
4. **Ejecutar Pruebas** — `dotnet test` (11 pruebas xUnit)
5. **Publicar Aplicación** — `dotnet publish --output publish/`
6. **Archivar Artefactos** — `archiveArtifacts`

## Listado de Tareas de Iteración (Sprint 1)

| # | Tarea | Prioridad | Estado |
|---|-------|-----------|--------|
| 1 | Diseñar base de datos | Alta | ✅ Completado |
| 2 | Implementar autenticación JWT | Alta | ✅ Completado |
| 3 | Desarrollar módulo de pagos | Media | 🔄 En progreso |
| 4 | Crear panel de administración | Media | 🔄 En progreso |
| 5 | Pruebas de integración | Baja | ⏳ Pendiente |
