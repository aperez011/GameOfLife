using GOL.DataAccess;
using GOL.Entities;
using GOL.Entities.DTOs;
using GOL.Services;
using GOL.Utilities;
using GOL.Utilities.Interfaces;
using LiteDB;
using System.Text.Json;

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

Task.Run(() =>
{
    //Starting unfinish game
    using (var ctx = new LiteDatabase(@"GOL.db"))
    {
        //GOLInternalServices service = new GOLInternalServices(_ctx);

        //Get unfinish game
        var games = ctx.GetCollection<GameOfLifeHeader>(typeof(GameOfLifeHeader).Name).Find(c => c.Status == nameof(GOLStatus.Running));

        if (games.Any())
        {
            foreach (var game in games)
            {
                var lastGeneration = ctx.GetCollection<GameOfLifeGenerations>(typeof(GameOfLifeGenerations).Name).Find(c => c.GameId == game.GID);

                //service.StartGame(new StartGameModel { Id = game.GID, ActiveCells = System.Text.Json.JsonSerializer.Deserialize<HashSet<Position>>(lastGeneration.MaxBy(c => c.Id)?.Generation) ?? new HashSet<Position>() });
                Console.WriteLine("Restart");
            }
        }
    }
});

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
