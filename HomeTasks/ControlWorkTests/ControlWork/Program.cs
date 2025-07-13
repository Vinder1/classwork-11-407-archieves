namespace ControlWork;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Variant 2");
    }
    
    
    
    ///Task 1
    public static string[] FindWords(string[] words) 
    {
        if (words == null || words.Length == 0)
            throw new NotSupportedException("Array is incorrect");

        string[] answer = new string[words.Length];
        int answerLength = 0;

        foreach (var s in words)
        {
            if (GoodWord(s)) 
                answer[answerLength++] = s;
        }

        string[] fixedAnswer = new string[answerLength];
        for (int i = 0; i < answerLength; i++)
            fixedAnswer[i] = answer[i];
        return fixedAnswer;
    }

    static bool GoodWord(string word)
    {
        word = word.ToLower();
        if (word == "")
            return false;
        return GoodWord(word, "qwertyuiop")
               || GoodWord(word, "asdfghjkl")
               || GoodWord(word, "zxcvbnm");
    }

    static bool GoodWord(string word, string keyboard)
    {
        foreach (var letter in keyboard)
            word = word.Replace(letter.ToString(), "");
        return word == "";
    }
    
    
    
    ///Task 2
    public static int[] Intersection(int[] nums1, int[] nums2)
    {
        int minLength = Math.Min(nums1.Length, nums2.Length);
        int[] answer = new int[minLength];
        int answerLength = 0;

        bool nums1Contains7Dividable = false;
        foreach (var num in nums1)
        {
            if (num % 7 == 0)
                nums1Contains7Dividable = true;
            
            if (nums2.Contains(num) && !CorrectContains(answer, answerLength, num))
                answer[answerLength++] = num;
        }

        if (!nums1Contains7Dividable)
            throw new Exception("No nums dividable by 7");

        int[] fixedAnswer = new int[answerLength];
        for (int i = 0; i < answerLength; i++)
            fixedAnswer[i] = answer[i];
        return fixedAnswer;
    }

    static bool CorrectContains(int[] array, int length, int num)
    {
        //Normal .Contains check whole array, but we need to check
        // only in prefix with given length
        bool contains = false;
        for (int i = 0; i < length; i++)
        {
            if (array[i] == num)
                contains = true;
        }
        return contains;
    }
    
    
    
    ///Task 3
    public static int GetCountUniqueEmails(string[] emails)
    {
        string[] shortEmails = GetShortEmails(emails);
        //PrintArray("T3", shortEmails);
        string[] ans = new string[shortEmails.Length];
        int answerLength = 0;
        foreach (var email in shortEmails)
        {
            if (!ans.Contains(email))
                ans[answerLength++] = email;
        }
        return answerLength;
    }

    static string[] GetShortEmails(string[] emails)
    {
        string[] ans = new string[emails.Length];
        for (int i = 0; i < emails.Length; i++)
            ans[i] = GetShort(emails[i]);
        return ans;
    }

    static string GetShort(string email)
    {
        string[] parts = email.Split("@");
        if (parts.Length != 2)
            throw new Exception($"{email} : That is not an email");

        parts[0] = parts[0].Replace(".", "");
        if (parts[0].Contains("+"))
            parts[0] = parts[0].Substring(0, parts[0].IndexOf("+"));
        return $"{parts[0]}@{parts[1]}";
    }
}
