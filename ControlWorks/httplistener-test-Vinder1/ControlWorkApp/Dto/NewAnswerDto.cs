namespace ControlWorkApp.Dto
{
    public sealed class NewAnswerDto
    {
        public required int QuestionId { get; init; }
        public required int StudentId { get; init; }
        public required string GivenAnswer { get; init; } = null!;
        public bool IsCorrect { get; set; }
    }
}