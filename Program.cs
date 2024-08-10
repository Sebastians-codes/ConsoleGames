using ConsoleGames;

FyraIRad fyraIRad = new();
FemIRad femIRad = new();

int choice = GetChoice();
Console.Clear();
if (choice == 1) {
    fyraIRad.Play();
    return;
}
femIRad.Play();
return;

int GetChoice() {
    Console.WriteLine("Välj mellan vanlig fyra i rad och 5 i rad med special regler.");
    Console.WriteLine("1 - Fyra i rad.\n2 - Fem i rad.");
    Console.Write("-> ");
    if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int num) && num is > 0 and < 3)
        return num;
    Console.Clear();
    Console.WriteLine("felaktig input försök igen.");
    return GetChoice();
}