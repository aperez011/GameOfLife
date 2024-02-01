using GOL.DataAccess;
using GOL.Entities;
using GOL.Entities.DTOs;
using GOL.Utilities;
using GOL.Utilities.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GOL.Services
{
    public class GOLInternalServices : IGOLInternalServices
    {

        private readonly dbGolContext _ctx;
        private readonly GameOfLifeStateModel _gol = new GameOfLifeStateModel();
        
        //Add to a config file
        private readonly int _canvas_X;
        private readonly int _canvas_Y;
        private readonly int _gen_limit = 5000;

        private int[] axisX;
        private int[] axisY;

        public GOLInternalServices(dbGolContext ctx)
        {

            _canvas_X = 100;
            _canvas_Y = 100;
            _ctx = ctx;
        }

        public async Task AutomaticGame(StartGameModel gameRequest)
        {
            try
            {
                //Save game header to the DB
                var gol = new GameOfLifeHeader
                {
                    GID = gameRequest.Id,
                    StartTime = DateTime.Now,
                    Status = nameof(GOLStatus.Running)
                };

                //Save
                _ = _ctx.Insert(gol);

                //start game
                await this.StartGame(gameRequest);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerationResponseModel> FinalBoard(List<Position> gameRequest, int maxAttemptsAllowed)
        {
            try
            {
                //Set the current generation
                _gol.CurrentGeneration = gameRequest.ToList();

                for (int i = 0; i < maxAttemptsAllowed; i++)
                {
                    var previewsBoard = _gol.CurrentGeneration.ToList();

                    bool newLife = await this.GenerateNextGeneration(_gol.CurrentGeneration);

                    if (!newLife)
                        return new GenerationResponseModel { GenerationNumber = 0, LiveCells = previewsBoard };
                }

                throw new Exception("Max attempts exceeded.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task StartGame(StartGameModel gameRequest)
        {
            try
            {
                //Set the current generation
                _gol.CurrentGeneration = gameRequest.ActiveCells.ToList();

                bool newLife = await this.GenerateNextGeneration(_gol.CurrentGeneration);

                if (!newLife /*|| _gol.CurrentGeneration == _gol.NewGeneration Validate before move to prod.*/)
                {
                    this.FinishGame(gameRequest.Id);
                    Console.WriteLine($"Game '{gameRequest.Id}' has ended.");
                    return;
                }

                var generationNumber = this.AddGeneration(gameRequest.Id, _gol.CurrentGeneration);
                Console.WriteLine($"Game '{gameRequest.Id}' generation #{generationNumber}.");

                if (generationNumber >= _gen_limit)
                {
                    this.FinishGame(gameRequest.Id);
                    Console.WriteLine($"Game '{gameRequest.Id}' has reached the maximum number of generations allowed.");
                    return;
                }

                //Validate if the game have been cancel

                //NOTA: change for a QUEUE, to improve performance
                var isCancel = this.IsCancel(gameRequest.Id);
                if (isCancel)
                    return;

                //Running until all cells dies.
                await this.StartGame(new StartGameModel { Id = gameRequest.Id, ActiveCells = _gol.CurrentGeneration });

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> GenerateNextGeneration(IList<Position> cells)
        {
            List<Position> _currentGeneration = cells.ToList();
            try
            {
                bool newLife = false;

                axisX = cells.Select(c => c.X).ToArray();
                axisY = cells.Select(c => c.Y).ToArray();

                //X
                for (int x = axisX.Min() - 1; x <= axisX.Max() + 1; x++)
                {
                    //Y
                    for (int y = axisY.Min() - 1; y <= axisY.Max() + 1; y++)
                    {

                        int nc = NeighboringStates(x, y, _currentGeneration);

                        //Validating life cells
                        if (_currentGeneration.Any(c => c.X == x && c.Y == y))
                            if (nc > 3 || nc < 2)
                                cells.Remove(_currentGeneration.Single(c => c.X == x && c.Y == y));

                        //Adding new life cells
                        if (nc == 3)
                        {
                            cells.Add(new Position { X = x, Y = y });
                            newLife = true;
                        }

                    }
                }

                return newLife;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _currentGeneration = null;
            }
        }

        public async Task UpdateCurrentGenerationState(IList<Position> currentGeneration)
        {
            List<Position> _currentGeneration = currentGeneration.ToList();
            try
            {
                axisX = currentGeneration.Select(c => c.X).ToArray();
                axisY = currentGeneration.Select(c => c.Y).ToArray();

                //X
                for (int x = axisX.Min() - 1; x <= axisX.Max() + 1; x++)
                {
                    //Y
                    for (int y = axisY.Min() - 1; y <= axisY.Max() + 1; y++)
                    {
                        //validate life
                        if (_currentGeneration.Any(c => c.X == x && c.Y == y))
                        {
                            int nc = NeighboringStates(x, y, _currentGeneration);

                            if (nc > 3 || nc < 2)
                                currentGeneration.Remove(_currentGeneration.Single(c => c.X == x && c.Y == y));
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _currentGeneration = null;
            }
        }

        public int NeighboringStates(int x, int y, IList<Position> cells)
        {
            //Count Neigbor
            //UP
            int countUP = cells.Count(c =>
                                          (
                                            (c.X == ((x - 1) == -1 ? 99 : (x - 1))) &&
                                            (c.Y == ((y - 1) == -1 ? 99 : (y - 1)))
                                          )
                                          ||
                                          (
                                            (c.X == (x == 0 ? 99 : x)) &&
                                            (c.Y == ((y - 1) == -1 ? 99 : y - 1))
                                          )
                                          ||
                                          (
                                            (c.X == ((x + 1) == _canvas_X ? 0 : x + 1)) &&
                                            (c.Y == ((y - 1) == -1 ? 99 : y - 1))
                                          )
                                     );

            //Center
            int countC = cells.Count(c =>
                                        (
                                            (c.X == ((x - 1) == -1 ? 99 : (x - 1))) &&
                                             c.Y == y
                                        )
                                        ||
                                        (
                                            (c.X == (x + 1 == _canvas_X ? 0 : x + 1)) &&
                                             c.Y == y
                                        )
                                    );

            //Down
            int countD = cells.Count(c =>
                                        (
                                            (c.X == ((x - 1) == -1 ? 99 : (x - 1))) &&
                                            (c.Y == ((y + 1) == _canvas_Y ? 0 : y + 1))
                                        )
                                        ||
                                        (
                                            c.X == x &&
                                            (c.Y == ((y + 1) == _canvas_Y ? 0 : y + 1))
                                        )
                                        ||
                                        (
                                            (c.X == (x + 1 == _canvas_X ? 0 : x + 1)) &&
                                            (c.Y == ((y + 1) == _canvas_Y ? 0 : y + 1))
                                        )
                                    );

            return (countUP + countC + countD);
        }

        #region [Internal validation methods]

        private bool IsCancel(Guid gameId)
        {
            var game = _ctx.FindOne<GameOfLifeHeader>(gameId);
            return game.Status == nameof(GOLStatus.Cancel);
        }

        private int AddGeneration(Guid gameId, List<Position> gen)
        {
            var gol = new GameOfLifeGenerations
            {
                GID = Guid.NewGuid(),
                GameId = gameId,
                Live = JsonSerializer.Serialize(gen)
            };

            //Save Generation to the DB.
            var id = _ctx.Insert(gol);
            return id;
        }

        private void FinishGame(Guid gameId)
        {
            var game = _ctx.FindOne<GameOfLifeHeader>(gameId);

            game.EndTime = DateTime.Now;
            game.Status = nameof(GOLStatus.Finish);

            _ctx.Update(game);
        }

        #endregion [Internal validation methods]
    }
}