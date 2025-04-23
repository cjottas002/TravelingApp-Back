# 🌍 TravelingApp

**TravelingApp** es una solución completa de gestión de viajes que incluye:

- ✨ Frontend Android (Kotlin + XML + MVVM)
- 💼 Backend en .NET 8 (C#) con arquitectura limpia y CQRS
- 🗄️ Persistencia en base de datos SQL Server con Entity Framework Core
- 🐳 Contenerización con Docker y Docker Compose

---

## 📱 Frontend (Android)

El frontend está desarrollado en **Kotlin** siguiendo buenas prácticas de arquitectura moderna:

- ✅ **MVVM** (Model-View-ViewModel)
- ✅ **Diseño con XML** (layouts tradicionales)
- ✅ **Jetpack Libraries** (LiveData, Navigation, ViewBinding, Lifecycle)
- ✅ **Retrofit** para llamadas HTTP
- ✅ **Dagger Hilt** para inyección de dependencias
- ✅ **Modularización** para mantener una arquitectura escalable

### 📁 Módulos principales

- `core`: configuración global (Retrofit, interceptores, etc.)
- `data`: modelos de red y DTOs
- `domain`: entidades para la ui, implementacion de repositorios, servicios
- `ui`: vistas XML, viewmodels y lógica de presentación

---

## 🧠 Backend (.NET)

El backend está desarrollado con **.NET 8** en C#, aplicando principios de arquitectura limpia y CQRS.

### 🛠️ Tecnologías y patrones usados

- **CQRS** con `MediatR` para separar lectura y escritura
- **AutoMapper** para mapear entre entidades y DTOs
- **Entity Framework Core** para acceso a datos
- **FluentValidation** para validaciones limpias de comandos
- **JWT** para autenticación segura
- **ASP.NET Core Identity** para gestión de usuarios y roles

### 📁 Proyectos y estructura

- `TravelingApp.Application`: lógica de negocio, comandos, handlers, DTOs
- `TravelingApp.Infrastructure`: servicios, base de datos, Identity, migraciones
- `TravelingApp.Domain`: entidades y contratos
- `TravelingApp.UI`: API ASP.NET Core

---

## 🗃️ Base de Datos

- **SQL Server** como motor de base de datos
- Migraciones gestionadas mediante **EF Core**
- Esquema configurado en `"Travel"` usando `modelBuilder.HasDefaultSchema("Travel")`
- Autogeneración de tablas Identity (`AspNetUsers`, `AspNetRoles`, etc.)

---

## 🐳 Contenerización con Docker

El backend está empaquetado en una **imagen Docker** para facilitar el despliegue en diferentes entornos.

- Se expone en el puerto `5000` por defecto

---

## 🌐 Sugerencia: usar NGINX como proxy para el backend

Para entornos de desarrollo o despliegue, puedes configurar un servidor **NGINX** como **reverse proxy** que redirija peticiones HTTP hacia tu backend en `localhost:5000`.

Esto facilita:

- Acceso externo desde dispositivos Android físicos
- Pruebas cruzadas entre redes
- Simulación de entorno real

### 🔧 Ejemplo básico de configuración NGINX

```nginx
server {
    listen 80;
    server_name traveling.local;

    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
