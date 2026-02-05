namespace ControlWorkApp.Dto
{
    public class NewQuestionDto
    {
        public required int TestId { get; init; }
        public required string Text { get; init; } = null!;
        public required string CorrectAnswer { get; init; } = null!;
    }
}