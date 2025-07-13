using ControlWork;

namespace ControlWorkTests;

public static class ControlWorkTestTask1
{
    private const string FirstRow = "qwertyiop";
    private const string SecondRow = "asdfghjkl";
    private const string ThirdRow = "zxcvbnm";
    
    [Fact]
    public static void AllWordsTypedWithOnlyFirstRow()
    {
        //Aggregate
        string[] input = ["qwertyu", "ytr", "o"];
        string[] expectedResult = ["qwertyu", "ytr", "o"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void AllWordsTypedWithOnlySecondRow()
    {
        //Aggregate
        string[] input = ["jhgfghj", "ffffffff", "afdfdffg"];
        string[] expectedResult = ["jhgfghj", "ffffffff", "afdfdffg"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
        
    [Fact]
    public static void AllWordsTypedWithOnlyThirdRow()
    {
        //Aggregate
        string[] input = ["zxvvbbxv", "zxcbbbb", "nbvc"];
        string[] expectedResult = ["zxvvbbxv", "zxcbbbb", "nbvc"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void OnlyCorrectWords()
    {
        //Aggregate
        string[] input = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc"];
        string[] expectedResult = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void ThreeCorrectTwoIncorrectWords()
    {
        //Aggregate
        string[] input = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc", "aboba", "qaz"];
        string[] expectedResult = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void IncorrectWordsTypedWithNotOnlyLetters()
    {
        //Aggregate
        string[] input = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc", "qwety444", "[]';/."];
        string[] expectedResult = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc"];
        
        //Act    
        var ans = Program.FindWords(input);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void NullArrayThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => Program.FindWords(null));
    }
    
    [Fact]
    public static void EmptyArrayThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => Program.FindWords([]));
    }
    
    [Fact]
    public static void EmptyStringDoesntThrowExceptionAndIsIncorrect()
    {
        Assert.Empty(Program.FindWords([""]));
    }
    
    [Fact]
    public static void CheckThatUpperCasePresenceDoesntMatter()
    {
        //Aggregate
        string[] input = ["qertyUIUytr", "jhgfDFGh", "xcVBNbvc"];
        string[] expectedResult = ["qertyuiuytr", "jhgfdfgh", "xcvbnbvc"];
        
        //Act    
        var ans = Program.FindWords(input);
        for (var i = 0; i < ans.Length; i++)
        {
            ans[i] = ans[i].ToLower();
        }
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    private static void CheckEqualArrays(string[] first, string[] second)
    {
        Array.Sort(first);
        Array.Sort(second);
        
        Assert.Equal(first.Length, second.Length);
        for (var i = 0; i < second.Length; i++)
        {
            Assert.Equal(second[i], first[i]);
        }
    }
}