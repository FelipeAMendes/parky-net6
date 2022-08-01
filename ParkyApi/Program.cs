using ParkyApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwagger();
builder.Services.AddDbContext();
builder.Services.AddServices();
builder.Services.AddAutoMapper();
builder.Services.AddJwtToken();

var app = builder.Build();

app.ExecuteMigrations();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
