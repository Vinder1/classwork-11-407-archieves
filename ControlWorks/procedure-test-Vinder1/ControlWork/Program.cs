namespace ControlWork;

class Program
{
    static void Main(string[] args)
    {
        //Вариант 2


        Console.WriteLine("Task 1");
        var args1 = new string[] {
            "", "hgfg", "edfv", "qwertoiuytr"
        }; // expected: hgfg qwertoiuytr
        PrintArray("T1 args", args1);
        PrintArray("T1 ans", FindWords(args1));
        //_ = FindWords(new string[0]);
        //_ = FindWords(null);

        args1 = new string[] {
            "baaad", "bbbbbBBBBBBBbbbbbbbb", "sdcghnjumjuymfdgb", "p"
        };
        PrintArray("T2 args", args1);
        PrintArray("T2 ans", FindWords(args1));

        Console.WriteLine("\n\nTask 2");
        var nums1 = new int[] {
            0, 1, 2, 3, 9, 8, 7, 6, 5, 5, 5, 5
        };
        var nums2 = new int[] {
            5, 16, 1, 3, 7, 19, 0
        };
        PrintArray("args (n1)", nums1);
        PrintArray("args (n2)", nums2);
        PrintArray("ans", Intersection(nums1, nums2));
        // nums1 = new int[] {
        //     1, 2, 3, 9, 8, 6, 5, 5, 5, 5
        // };
        // nums2 = new int[] {
        //     5, 16, 1, 3, 7, 19, 0
        // };
        // PrintArray("args (n1)", nums1);
        // PrintArray("args (n2)", nums2);
        // PrintArray("ans", Intersection(nums1, nums2));

        Console.WriteLine("\n\nTask 3");
        var args3 = new string[] {"aaa@lol", "aa.a@lol", "aa.a+bbb@lol", "aa.a+bbb+ccc@lol"};
        PrintArray("args", args3);
        Console.WriteLine($"ans: {GetCountUniqueEmails(args3)}");
        args3 = new string[] {"aboba@email.co+m", "aboba@email.com", "ABOBA@email.com"};
        PrintArray("args", args3);
        Console.WriteLine($"ans: {GetCountUniqueEmails(args3)}");
        // args3 = new string[] {"aboba@ema@il.co+m", "aboba@email.com", "ABOBA@email.com"};
        // PrintArray("args", args3);
        // Console.WriteLine($"ans: {GetCountUniqueEmails(args3)}");
    }

    static void PrintArray(string message, string[] arr)
    {
        Console.Write($"{message} | ");
        foreach (var s in arr)
            Console.Write($"{s}, ");
        Console.WriteLine();
    }

    static void PrintArray(string message, int[] arr)
    {
        Console.Write($"{message} | ");
        foreach (var s in arr)
            Console.Write($"{s}, ");
        Console.WriteLine();
    }

    ///Task 1
    static string[] FindWords(string[] words) 
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
    static int[] Intersection(int[] nums1, int[] nums2)
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
    static int GetCountUniqueEmails(string[] emails)
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
