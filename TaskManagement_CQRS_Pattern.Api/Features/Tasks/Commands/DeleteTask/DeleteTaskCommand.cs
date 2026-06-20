using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement_CQRS_Pattern.Db.AppDbContextModels;

namespace TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(int id) : IRequest<string>;

public class DeleteTaskHanlder: IRequestHandler<DeleteTaskCommand, string>
{
    private readonly AppDbContext _appDbContext;

    public DeleteTaskHanlder(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var item = await _appDbContext.TaskItems.FirstOrDefaultAsync(x => x.Id == request.id, ct);
        
        if (item is null) return "No item found";
        
        _appDbContext.TaskItems.Remove(item);
        var result = await _appDbContext.SaveChangesAsync(ct);

        var message = result > 0 ? "Delete successful" : "Delete failed";

        return message;
    }
}