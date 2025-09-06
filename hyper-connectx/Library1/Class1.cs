namespace Library1;

public static class FizzBuzzLogic
{
    public static void FizzBuzzInstructions()
    {
        Console.WriteLine("Fizz buzz is a group word game for children to teach them about division." +
                          "[1] Players take turns to count incrementally, replacing any number divisible by three with the word \"fizz\"," +
                          " and any number divisible by five with the word \"buzz\", " +
                          "and any number divisible by both three and five with the word \"fizzbuzz\".");
    }

    public static int GetFizzBuzzCount() 
    {
        Console.Write("Enter a number to start: ");
        var userInput = Console.ReadLine();
        
        var countTo = 100;

        if (userInput != null)
        {
            if(!int.TryParse(userInput,  out countTo))
            {
                countTo = 100;
            }
        }
        else
        {
            Console.WriteLine("Please enter a number greater than zero.");
        }
        return countTo;
    }

    public static void FizzBuzz(int number)
    {
        for (int i = 1; i <= number; i++)
        {
            if (i % 3 == 0)
            {
                Console.Write("fizz");

                if (i % 5 == 0)
                {
                    Console.Write("buzz");
                }

                Console.Write(", ");
                continue;
            }

            if (i % 5 == 0)
            {
                Console.Write("buzz, ");
                continue;
            }

            Console.Write(i + ", ");
        }
    }
}