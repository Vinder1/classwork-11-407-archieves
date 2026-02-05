using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlWorkApp.Entities
{
    public sealed class Answer
    {
        public required int Id { get; init; }
        public required int QuestionId { get; init; }
        public required int StudentId { get; init; }
        public required string GivenAnswer { get; init; } = null!;
        public required bool IsCorrect { get; init; }
    }
}