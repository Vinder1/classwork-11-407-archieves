using ControlWork;

namespace ControlWorkTests;

public static class ControlWorkTestTask3
{
    [Fact]
    public static void ThreeRegularUniqueEmails()
    {
        //Aggregate
        string[] input = ["aboba@boba", "qwerty@uiop", "zxc@banana"];
        int expectedResult = 3;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsWithDotesAllSameWithoutDotes()
    {
        //Aggregate
        string[] input = ["aboba@boba", "ab.oba@boba", "a.b.o.b.a@boba"];
        int expectedResult = 1;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsWithPlusAllSameWithoutPartAfterPlus()
    {
        //Aggregate
        string[] input = ["aboba@boba", "aboba+biba@boba", "aboba+1234@boba"];
        int expectedResult = 1;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsWithPlusAllSameWithoutPlusButDifferentWithoutPartsAfterPlus()
    {
        //Aggregate
        string[] input = ["aboba@boba", "a+boba@boba", "ab+oba@boba"];
        int expectedResult = 3;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsSameLocalDifferentDomainMustBeUnique()
    {
        //Aggregate
        string[] input = ["local@domain1", "local@domain2", "local@domain3"];
        int expectedResult = 3;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsDifferentLocalSameDomainMustBeUnique()
    {
        //Aggregate
        string[] input = ["local1@domain", "local2@domain", "local3@domain"];
        int expectedResult = 3;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeEmailsSameLocal_ButDotesAndPlusesInDomainMakeDomainsSameWithLocalPartRules_MustNotBeSame()
    {
        //Aggregate
        string[] input = ["local@domain", "local@do+main", "local@domain+naimod"];
        int expectedResult = 3;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
    
    [Fact]
    public static void ThreeSameBecauseOfPlusesCheckThatSecondPlusDoesntDoAnything()
    {
        //Aggregate
        string[] input = ["local@domain", "local+bebebe@domain", "local+bebebe+bababa@domain"];
        int expectedResult = 1;
        
        //Act
        var ans = Program.GetCountUniqueEmails(input);
        
        //Assert
        Assert.Equal(expectedResult, ans);
    }
}