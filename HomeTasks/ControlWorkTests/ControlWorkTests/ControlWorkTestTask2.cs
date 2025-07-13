using ControlWork;

namespace ControlWorkTests;

public static class ControlWorkTestTask2
{
    [Fact]
    public static void CheckSequencesWith2Common()
    {
        //Aggregate
        int[] first = [1, 2, 3, 7];
        int[] second = [3, 4, 5, 2];
        int[] expectedResult = [2, 3];
        
        //Act    
        var ans = Program.Intersection(first, second);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void CheckSequencesWithNoCommon()
    {
        //Aggregate
        int[] first = [1, 2, 3, 7];
        int[] second = [10, 11, 12, 13];
        int[] expectedResult = [];
        
        //Act    
        var ans = Program.Intersection(first, second);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void CheckSecondSequenceIsSubsequenceOfFirst()
    {
        //Aggregate
        int[] first = [1, 2, 3, 7];
        int[] second = [1, 2, 3];
        
        //Act    
        var ans = Program.Intersection(first, second);
        
        //Assert
        CheckEqualArrays(ans, second);
    }
    
    [Fact]
    public static void CheckSequencesWith2SameCommonMustBeOnlyOneInAnswer()
    {
        //Aggregate
        int[] first = [1, 2, 2, 3, 7];
        int[] second = [2, 5, 2];
        int[] expectedResult = [2];
        
        //Act    
        var ans = Program.Intersection(first, second);
        
        //Assert
        CheckEqualArrays(ans, expectedResult);
    }
    
    [Fact]
    public static void CheckFirstSequenceDoesntHave7()
    {
        //Aggregate
        int[] first = [1, 2, 3];
        int[] second = [2, 4];
        
        //Act + Assert
        Assert.Throws<Exception>(() => Program.Intersection(first, second));
    }
    
    private static void CheckEqualArrays(int[] first, int[] second)
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