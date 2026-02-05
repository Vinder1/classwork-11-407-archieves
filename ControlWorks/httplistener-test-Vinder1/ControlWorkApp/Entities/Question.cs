namespace ControlWorkApp.Entities
{
    public sealed class Question
    {
        public required int Id { get; init; }
        public required int TestId { get; init; }
        public required string Text { get; init; } = null!;
        public required string CorrectAnswer { get; init; } = null!;
    }
}