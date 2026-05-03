using TaskTracker.Api.Dtos.Boards;

namespace TaskTracker.Api.Services.Interfaces;

public interface IBoardService
{
    Task<List<BoardResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<BoardResponse>> GetAllForProjectAsync(int projectId, CancellationToken cancellationToken);
    Task<BoardResponse> CreateAsync(CreateBoardRequest request, CancellationToken cancellationToken);
}