# Bolsa_Empleos_Capacitaciones-IA
Bolsa de Empleos con capacitaciones integradas usando inteligencia artificial, para jóvenes inexpertos  


# Como añadir nuevas tablas a la base de datos usando migraciones 
    1: Ir a la carpeta src: 
    cd C:\Users\user\Desktop\Bolsa_Empleos_Capacitaciones-IA\src
    2: Crear nueva migración:
    dotnet ef migrations add NombreMigracion --project BolsaEmpleos.Infrastructure\BolsaEmpleos.Infrastructure.csproj --startup-project BolsaEmpleos.API\BolsaEmpleos.API.csproj --context BolsaEmpleosDbContext --output-dir Persistence\Migrations
    3: Aplicarla a PostgreSQL:
    dotnet ef database update --project BolsaEmpleos.Infrastructure\BolsaEmpleos.Infrastructure.csproj --startup-project BolsaEmpleos.API\BolsaEmpleos.API.csproj --context BolsaEmpleosDbContextdotne

# Como levantar el Frontend y el backend 
Frontend: 
    1: cd frontend 
    2: nmp install
    3: npm run dev
    4: localhost 5173

Backend:
    1: cd src 
    2: dotnet run --project BolsaEmpleos.API\BolsaEmpleos.API.csproj
    3: localhost 5002 

