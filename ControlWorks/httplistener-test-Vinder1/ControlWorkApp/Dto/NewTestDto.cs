using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlWorkApp.Dto
{
    public sealed class NewTestDto
    {
        public required string Title { get; init; }
        public string? Description { get; init; }
        public required int AuthorId { get; init; }
    }
}