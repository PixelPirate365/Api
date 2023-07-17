using web_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IPeopleService, PeopleService>();

builder.Services.AddCors(x => x.AddDefaultPolicy(
    opts => opts.WithOrigins("http://127.0.0.1:5500", "http://localhost:7096")
    .WithHeaders("Content-Type")
    .WithMethods("POST")
  )
) ;

var app = builder.Build();



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
