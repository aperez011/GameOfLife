using GOL.Entities.DTOs;

namespace GOL.Utilities.Interfaces
{
    public interface IGOLInternalServices
    {
        Task AutomaticGame(StartGameModel gameRequest);

        Task<GenerationResponseModel> FinalBoard(List<Position> gameRequest, int maxAttemptsAllowed);

        Task StartGame(StartGameModel gameRequest);

        int NeighboringStates(int x, int y, IList<Position> currentCells);

        Task<bool> GenerateNextGeneration(IList<Position> lastGeneration);

        Task UpdateCurrentGenerationState(IList<Position> currentGeneration);
    }
}
