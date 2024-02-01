using GOL.Entities.DTOs;

namespace GOL.Utilities.Interfaces
{
    public interface IGolServices
    {
        /// <summary>
        /// Get game information
        /// </summary>
        /// <param name="gameId">GUID:Game ID</param>
        /// <returns>Game status</returns>
        Task<OperationResult<HashSet<GenerationResponseModel>>> GetGameOfLifeGenerations(Guid gameId);

        /// <summary>
        /// Get next generation for the game
        /// </summary>
        /// <returns>next generation state</returns>
        Task<OperationResult<GenerationResponseModel>> GetNextGeneration(StartGameRequest startPositions);

        /// <summary>
        /// Gets x number of generation away for the start
        /// </summary>
        /// <param name="states">int: number of generations requested.</param>
        /// <returns>List: generation info</returns>
        Task<OperationResult<HashSet<GenerationResponseModel>>> GetGenerations(StartGameRequest startBoard, int states);

        /// <summary>
        /// Get the last board generate
        /// </summary>
        /// <param name="startBoard"></param>
        /// <param name="maxAttemptsAllowed"></param>
        /// <returns></returns>
        Task<OperationResult<GenerationResponseModel>> GetFinalBoard(StartGameRequest startBoard, int maxAttemptsAllowed);

        /// <summary>
        /// Allows uploading a new game.
        /// </summary>
        /// <param name="startPositions">These are the live cell positions.</param>
        /// <returns>GUID: Game Id</returns>
        Task<OperationResult<GeneralResponseModel>> StartGameOfLife(StartGameRequest startPositions);

        /// <summary>
        /// Finish a game
        /// </summary>
        /// <param name="gameId">GUID:Game ID</param>
        /// <returns>Game last status</returns>
        Task<OperationResult> EndGameOfLife(Guid gameId);

    }
}
