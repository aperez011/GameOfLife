using GOL.Entities.DTOs;

namespace GOL.Utilities.Interfaces
{
    public interface IGOLInternalServices
    {
        Task AutomaticGame(StartGameModel gameRequest);

        Task<GenerationResponseModel> FinalBoard(HashSet<Position> gameRequest, int maxAttemptsAllowed);

        Task StartGame(StartGameModel gameRequest);

        int NeighboringStates(int x, int y, HashSet<Position> currentCells);

        Task<bool> GenerateNextGeneration(HashSet<Position> lastGeneration);

        Task UpdateCurrentGenerationState(HashSet<Position> currentGeneration);
    }
}
