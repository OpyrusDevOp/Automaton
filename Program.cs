// See https://aka.ms/new-console-template for more information

using Automaton;

/*State[] states =
[
    new(1, true),
    new(2),
    new(3),
    new(4, false, true),
    
];

Transition[] transitions =
[
    new(states[0], states[0], 'a'),
    new(states[0], states[0], 'b'),
    new(states[0], states[0], 'c'),
    new(states[0], states[1], 'a'),
    new(states[1], states[2], 'b'),
    new(states[2], states[3], 'c'),
    new(states[3], states[3], 'a'),
    new(states[3], states[3], 'b'),
    new(states[3], states[3], 'c')
];

Automate automate = new(states, transitions);

automate.Display_Alphabet();

automate.Display_Transition();

var word = Console.ReadLine();

if (word == null)
    return;

var recognized = automate.Is_Recognized(word);

Console.WriteLine(recognized ? $"{word} appartient au langage" : $"{word} n'appartient pas au langage");*/


// Automate A
State[] statesA = {
    new(1, true), // Entry state
    new(2, false, true),
    new(3, true) // Exit state
};

Transition[] transitionsA =
[
    new(statesA[0], statesA[1], 'a'),
    new(statesA[1], statesA[2], 'a'),
    new(statesA[2], statesA[1], 'b'),
    new(statesA[2], statesA[2], 'b')
];

// Automate B
State[] statesB =
[
    new(4, true),
    new(5, false, true) // Exit state
];

Transition[] transitionsB =
[
    new(statesB[0], statesB[0], 'a'),
    new(statesB[0], statesB[1], 'b'),
    new(statesB[1], statesB[0], 'a')
];

// Create the automata
var automataA = new Automate(statesA, transitionsA);
var automataB = new Automate(statesB, transitionsB);

// Get intersection
var intersectedAutomate = Automate.Intersection(automataA, automataB);

// Verify results
intersectedAutomate.Display_Alphabet();
intersectedAutomate.Display_Transition();
