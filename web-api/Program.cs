using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using web_api.Controllers;
using web_api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var securityScheme = new OpenApiSecurityScheme() {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Enter your token here",
};
var securityReq = new OpenApiSecurityRequirement() {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    options.OperationFilter<AuthenticationRequirementOperationFilter>();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "web-api.xml"));
});

//MyContext is scoped
builder.Services.AddDbContext<MyContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
);
var origins = builder.Configuration
    .GetSection("Cors")
    .Get<string[]>();

builder.Services.AddCors(x => x.AddDefaultPolicy(
    opts => opts
        .WithOrigins(origins)
        //.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
  )
);
//Jwt || Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateAudience = false,
            //ValidAudience = "domain.com",
            ValidateIssuer = true,
            ValidIssuer = "web-api",  //builder.Configuration["Jwt:Issuer"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthController.TOKEN_SECRET))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Note: Run "update-database" on application load
await using (var scope = app.Services.CreateAsyncScope()) {
    using var context = scope.ServiceProvider.GetService<MyContext>();
    await context.Database.MigrateAsync();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers()
    .RequireAuthorization();

app.Run();
