using GOL.DataAccess;
using GOL.Entities;
using GOL.Entities.DTOs;
using GOL.Utilities;
using GOL.Utilities.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace GOL.Services
{
    public class GolServices : IGolServices
    {
        private readonly IGOLInternalServices _gameServices;
        private readonly dbGolContext _ctx;


        public GolServices(IGOLInternalServices gameServices, dbGolContext ctx)
        {
            _gameServices = gameServices;
            _ctx = ctx;
        }


        public async Task<OperationResult<HashSet<GenerationResponseModel>>> GetGameOfLifeGenerations(Guid gameId)
        {
            try
            {
                var generations = _ctx.FindBy<GameOfLifeGenerations>(c=> c.GameId == gameId);

                var response = generations.Select(c => new GenerationResponseModel
                {
                   GameId = gameId,
                   GenerationNumber = c.Id,
                   LiveCells = JsonSerializer.Deserialize<HashSet<Position>>(c.Live) ?? new HashSet<Position>()
                }).ToHashSet();

                return OperationResult<HashSet<GenerationResponseModel>>.Success(response);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<OperationResult<HashSet<GenerationResponseModel>>> GetGenerations(StartGameRequest startBoard, int states)
        {
            HashSet<Position> NewGeneration = startBoard.InitialPositions;
            HashSet<GenerationResponseModel> generations = new HashSet<GenerationResponseModel>();


            try
            {
                for (int i = 0; i < states; i++)
                {
                    var nexLife = await _gameServices.GenerateNextGeneration(NewGeneration);
                    if (!nexLife)
                        return OperationResult<HashSet<GenerationResponseModel>>.Success(generations);

                    generations.Add(new GenerationResponseModel { GenerationNumber = i + 1, LiveCells = NewGeneration });
                }

                return OperationResult<HashSet<GenerationResponseModel>>.Success(generations);

            }
            catch (Exception ex)
            {
                return OperationResult<HashSet<GenerationResponseModel>>.Fail((int)HttpStatusCode.NotFound, ex.Message);
            }
            finally
            {
                NewGeneration = null;
                generations = null;
            }
        }

        public async Task<OperationResult<GenerationResponseModel>> GetFinalBoard(StartGameRequest startBoard, int maxAttemptsAllowed)
        {
            try
            {
                var finalBoard = await _gameServices.FinalBoard(startBoard.InitialPositions, maxAttemptsAllowed);
                return OperationResult<GenerationResponseModel>.Success(finalBoard);

            }
            catch (Exception ex)
            {
                return OperationResult<GenerationResponseModel>.Fail((int)HttpStatusCode.NotFound, ex.Message); ;
            }
        }

        public async Task<OperationResult<GenerationResponseModel>> GetNextGeneration(StartGameRequest startPositions)
        {
            HashSet<Position> NewGeneration = startPositions.InitialPositions;

            try
            {

                var nextGenerarion = await _gameServices.GenerateNextGeneration(NewGeneration);

                return OperationResult<GenerationResponseModel>.Success(new GenerationResponseModel { GenerationNumber = 0, LiveCells = NewGeneration });

            }
            catch (Exception ex)
            {
                return OperationResult<GenerationResponseModel>.Fail((int)HttpStatusCode.NotFound, ex.Message); ;
            }
            finally
            {
                NewGeneration = null;
            }
        }

        public async Task<OperationResult<GeneralResponseModel>> StartGameOfLife(StartGameRequest gameRequest)
        {
            try
            {
                //Building the game
                var game = new StartGameModel
                {
                    Id = Guid.NewGuid(),
                    ActiveCells = gameRequest.InitialPositions
                };

                //Add the new thread
                //new Thread(() =>
                //{
                //Thread.CurrentThread.Name = id.ToString();
                //Thread.CurrentThread.IsBackground = true;

                Console.WriteLine($"Game '{game.Id}' has started");
                _ = _gameServices.AutomaticGame(game);

                //}).Start();

                return OperationResult<GeneralResponseModel>.Success(new GeneralResponseModel { Id = game.Id });

            }
            catch (Exception ex)
            {
                return OperationResult<GeneralResponseModel>.Fail((int)HttpStatusCode.NotFound, ex.Message); ;
            }
        }

        public async Task<OperationResult> EndGameOfLife(Guid gameId)
        {
            try
            {
                var game = _ctx.FindOne<GameOfLifeHeader>(gameId);

                game.EndTime = DateTime.Now;
                game.Status = nameof(GOLStatus.Cancel);

                _ = _ctx.Update(game);

                return OperationResult<GeneralResponseModel>.Success();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}