using Back_Repuestos.Data;
using Back_Repuestos.Dato_semilla;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(9, 3, 0))));

//Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAngular",
        policy => policy
            .WithOrigins("http://localhost:4200") //Url del angular ojo
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//correo
builder.Services.Configure<ConfCorreo>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IEmailService, CorreoService>();




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    }); ;

builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DatoSemilla.SeedAdminAsync(db, app.Configuration);
}
using (var scope = app.Services.CreateScope())

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsAngular");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
