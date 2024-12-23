using Automaton;

var s1 = new State { id = "s1", isEntry = true };
var s2 = new State { id = "s2" };
var s3 = new State { id = "s3", isExit = true };
var s4 = new State { id = "s4" }; // État inutile

var automate = new Automate();
automate.AddTransition(s1, 'a', s2);
automate.AddTransition(s2, 'b', s3);
automate.AddTransition(s4, 'c', s4); // Transition inutile

Console.WriteLine("Avant trim :");
Console.WriteLine(automate.PrintTransition());

var trimmed = automate.Trim();

Console.WriteLine("Après trim :");
Console.WriteLine(trimmed.PrintTransition());
