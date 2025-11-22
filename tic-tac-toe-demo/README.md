README.md

# EF

## Tooling

~~~sh
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
~~~

## Migrations

~~~sh
// run from the solution folder

// add new migration, creates the app.db file
dotnet ef migrations --startup-project WebApp --project DAL add Initial

// apply migration
dotnet ef database update --startup-project WebApp --project DAL

// drop the database
dotnet ef database drop --startup-project WebApp --project DAL
~~~

# Asp.net

## Tooling

~~~sh
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool update --global dotnet-aspnet-codegenerator
~~~

Install from nuget:
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer

Packages in WebApp
~~~xml
    <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="10.0.0"/>
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0"/>
    <PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="10.0.0"/>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0"/>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0"/>
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="9.0.0" />
</ItemGroup>
~~~

## Scaffolding
~~~sh
// run from inside WebApp folder
cd WebApp

dotnet aspnet-codegenerator razorpage -m Category -dc AppDbContext -udl -outDir Pages/Categories --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Priority -dc AppDbContext -udl -outDir Pages/Priorities --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m ToDo -dc AppDbContext -udl -outDir Pages/ToDos --referenceScriptLibraries
  
// -m - Name of the model class
// -dc - Data context class
// -udl - Use default layout
// -outDir - Where to generate the output
// --referenceScriptLibraries - Add validation scripts on Edit and Create pages
~~~

## Update _Layuot.cshtml

Add links to scaffolded pages

~~~html
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home</a>
</li>
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-page="/ToDos/Index">ToDos</a>
</li>
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-page="/Priorities/Index">Priorities</a>
</li>
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-page="/Categories/Index">Categories</a>
</li>
~~~

Fix the UI:

// to get the correct order
~~~cshtml
public async Task OnGetAsync()
    {
        Category = await _context
             .Categories
             .OrderBy(p => p.SortValue)
             .ThenBy(p =>p.CategoryName)
             .ToListAsync();
    }
~~~
