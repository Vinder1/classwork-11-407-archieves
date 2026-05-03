using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskTracker.Api.Domain.Entities;
using TaskTracker.Api.Dtos.Boards;
using TaskTracker.Api.Repositories.Interfaces;
using TaskTracker.Api.Services.Interfaces;

namespace TaskTracker.Api.Services.Implementations;

public class BoardService(IBoardRepository boardRepository, IMapper mapper) : IBoardService
{
    private readonly IBoardRepository _boardRepository = boardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<BoardResponse> CreateAsync(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Name = request.Name,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _boardRepository.AddAsync(board, cancellationToken);
        await _boardRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BoardResponse>(board);
    }

    public async Task<List<BoardResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var boards = await _boardRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<BoardResponse>>(boards);
    }
    
    public async Task<List<BoardResponse>> GetAllForProjectAsync(int projectId, CancellationToken cancellationToken)
    {
        var boards = await _boardRepository.GetAllForProjectAsync(projectId, cancellationToken);
        return _mapper.Map<List<BoardResponse>>(boards);
    }
}