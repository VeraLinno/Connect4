namespace MenuSystem;

public class Menu
{
    private List<MenuItem> MenuItems { get; set; } = new ();

    public void AddMenuItems(List<MenuItem> items)
    {
        // check for validity
        //add

        foreach (var item in items)
        {
            // control (dict)
            
            MenuItems.Add(item);
        }
    }

    public void Run() // or public string run() , then you need to return smth
    {
        var menuIsDone = false;
        do
        {
            // display menu
            // get user input
            // validate 
            // execute choice
            // loop back to top
            
            DisplayMenu();
            Console.Write("Please enter your choice: ");
            var userInput = Console.ReadLine();

            if (userInput == "X")
            {
                menuIsDone = true;
            }
                
        } while (!menuIsDone);
        
    }

    private void DisplayMenu()
    {
        foreach (var item in MenuItems)
        {
            Console.WriteLine(item);
        }
    }
}