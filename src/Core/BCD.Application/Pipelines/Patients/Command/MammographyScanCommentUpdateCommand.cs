using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Pipelines.Patients.Command
{
    public record MammographyScanCommentUpdateCommand : IRequest<ResultDto<int>>
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class MammographyScanCommentUpdateCommandHandler(IBCDContext bCDContext)
        : IRequestHandler<MammographyScanCommentUpdateCommand, ResultDto<int>>
    {
        public async Task<ResultDto<int>> Handle(
            MammographyScanCommentUpdateCommand request, CancellationToken cancellationToken)
        {
            var mammographyScan = await bCDContext.MammographyScans
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (mammographyScan is null)
            {
                return ResultDto<int>.Failure("MammographyScan record not found");
            }

            mammographyScan.Comment = request.Comment;

            await bCDContext.SaveChangesAsync(cancellationToken);

            return ResultDto<int>.Ok(mammographyScan.Id, "Comment updated successfully");
        }
    }
}
