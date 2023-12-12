namespace AoC.Library.Runner;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomExampleAttribute : Attribute
{
    public string Input { get; }

    public string AnswerOne { get; }

    public string AnswerTwo { get; }

    public CustomExampleAttribute(string input, object answerOne, object answerTwo)
    {
        Input = input;
        AnswerOne = answerOne?.ToString() ?? string.Empty;
        AnswerTwo = answerTwo?.ToString() ?? string.Empty;
    }

    public CustomExampleAttribute(string input, object answer) : this(input, answer, answer) { }
}