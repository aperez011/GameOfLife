using GOL.DataAccess;
using GOL.Entities;
using GOL.Entities.DTOs;
using GOL.Utilities;
using GOL.Utilities.Interfaces;
using System.Collections.Generic;
using System.Net;
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

        public async Task<OperationResult> GetFinalState(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<GameResponseModel>> GetGameOfLifeInfo(Guid gameId)
        {
            try
            {
                var ins = new GameOfLifeGenerations
                {
                    Id = 0,
                    GID = Guid.NewGuid(),
                    Generation = 2,
                    Live = ""
                };

                //var test = _ctx.Insert(ins);

                var getTest = _ctx.FindAll<GameOfLifeGenerations>().ToList();


                var one = _ctx.FindOne<GameOfLifeGenerations>(Guid.Parse("3f11ac95-cab6-4a6f-8632-a9ffd867dba0"));

                return OperationResult<GameResponseModel>.Success();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<OperationResult<IList<GenerationResponseModel>>> GetGenerations(StartGameRequest startBoard, int states)
        {
            List<Position> NewGeneration = startBoard.InitialPositions;
            List<GenerationResponseModel> generations = new List<GenerationResponseModel>();


            try
            {
                for (int i = 0; i < states; i++)
                {
                    var nexLife = await _gameServices.GenerateNextGeneration(NewGeneration);
                    if (!nexLife)
                        return OperationResult<IList<GenerationResponseModel>>.Success(generations);

                    generations.Add(new GenerationResponseModel { GenerationNumber = i + 1, LiveCells = NewGeneration.ToList() });
                }

                return OperationResult<IList<GenerationResponseModel>>.Success(generations);

            }
            catch (Exception ex)
            {
                return OperationResult<IList<GenerationResponseModel>>.Fail((int)HttpStatusCode.NotFound, ex.Message);
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
            List<Position> NewGeneration = startPositions.InitialPositions;

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
                _gameServices.AutomaticGame(game);

                //}).Start();

                return OperationResult<GeneralResponseModel>.Success(new GeneralResponseModel { Id = game.Id });

            }
            catch (Exception ex)
            {
                return OperationResult<GeneralResponseModel>.Fail((int)HttpStatusCode.NotFound, ex.Message); ;
            }
        }

        public async Task<OperationResult<GameResponseModel>> EndGameOfLife(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}