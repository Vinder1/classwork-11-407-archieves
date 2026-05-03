namespace AspNetApp.Exceptions;

/// <summary>
/// Thrown when you are trying to use the star that is missing
/// </summary>
public class StarNotFoundException() : Exception(message: "Star not found") {}