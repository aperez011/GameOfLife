using GOL.Entities;
using GOL.Entities.DTOs;
using GOL.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.CompilerServices;

namespace GOL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GOLController : ControllerBase
    {
        private readonly IGolServices _golServices;
        public GOLController(IGolServices golServices)
        {
            _golServices = golServices;
        }

        [HttpGet]
        [Route("{gameId}/Generations")]
        [ProducesResponseType(typeof(IList<GameOfLifeGenerations>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGameGenerations([FromRoute] Guid gameId)
        {
            var result = await _golServices.GetGameOfLifeGenerations(gameId);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok();
        }

        [HttpPost]
        [Route("FinalState")]
        [ProducesResponseType(typeof(IList<GameOfLifeGenerations>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFinalBoard([FromBody] StartGameRequest startBoard, [FromRoute] int maxAttempsAllowed)
        {
            var result = await _golServices.GetFinalBoard(startBoard, maxAttempsAllowed);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("Generations/{numberGenerations}")]
        [ProducesResponseType(typeof(IList<GameOfLifeGenerations>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGenerations([FromBody] StartGameRequest startBoard, [FromRoute] int numberGenerations)
        {
            var result = await _golServices.GetGenerations(startBoard, numberGenerations);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("Generation/Next")]
        [ProducesResponseType(typeof(GenerationResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNextGeneration([FromBody] StartGameRequest startBoard)
        {
            var result = await _golServices.GetNextGeneration(startBoard);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("Start/Game")]
        [ProducesResponseType(typeof(GeneralResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> StartNewGame([FromBody] StartGameRequest startBoard)
        {
            var result = await _golServices.StartGameOfLife(startBoard);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("End/{gameId}")]
        [ProducesResponseType(typeof(IList<GameOfLifeGenerations>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EndGame([FromRoute] Guid gameId)
        {
            var result = await _golServices.EndGameOfLife(gameId);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok();
        }

        [HttpPost("Test")]
        [ProducesResponseType(typeof(IList<GameOfLifeGenerations>), (int)HttpStatusCode.OK)]
        public async IAsyncEnumerable<HashSet<Position>> GOLTest([FromBody] StartGameRequest startBoard,
                                                 [EnumeratorCancellation] CancellationToken token)
        {
            HashSet<Position> _positions = startBoard.InitialPositions;

            while (!token.IsCancellationRequested)
            {
                var tt = await _golServices.GetNextGeneration(new StartGameRequest { InitialPositions = _positions });

                startBoard.InitialPositions = tt.Data.LiveCells;

                yield return tt.Data.LiveCells;
            }

            /*
            var result = await _golServices.GetNextGeneration(startBoard);
            if (!result.IsSuccess)
                return StatusCode(result.statusCode, result.Message);

            return Ok();*/
        }

    }
}
