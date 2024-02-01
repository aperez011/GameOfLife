using GOL.DataAccess;
using GOL.Services;
using GOL.Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Context
builder.Services.AddSingleton<dbGolContext, dbGolContext>();

builder.Services.AddScoped<IGOLInternalServices, GOLInternalServices>();
builder.Services.AddScoped<IGolServices, GolServices>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
