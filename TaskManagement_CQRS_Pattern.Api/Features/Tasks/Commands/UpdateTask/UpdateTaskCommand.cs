using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement_CQRS_Pattern.Db.AppDbContextModels;

namespace TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(int id, UpdateTaskDto updateTaskDto) : IRequest<string>;

public class UpdateTaskHandler: IRequestHandler<UpdateTaskCommand, string>
{
    private readonly AppDbContext _appDbContext;

    public UpdateTaskHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> Handle(UpdateTaskCommand request, CancellationToken ct) 
    {
        var item = await _appDbContext.TaskItems.FirstOrDefaultAsync(x => x.Id == request.id, ct);

        if (item is null) return "No item data found.";

        if(!string.IsNullOrEmpty(request.updateTaskDto.Title)){
            item.Title = request.updateTaskDto.Title;
        }

        if (!string.IsNullOrEmpty(request.updateTaskDto.Description))
        {
            item.Description = request.updateTaskDto.Description;
        }

        if(request.updateTaskDto.Iscompleted is not null)
        {
            item.Iscompleted = request.updateTaskDto.Iscompleted.Value;
        }

        int result = await _appDbContext.SaveChangesAsync(ct);
        var message = result > 0 ? "Update successful." : "Update failed.";

        return message;
    }
}

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; } 
    public bool? Iscompleted { get; set; }
}


