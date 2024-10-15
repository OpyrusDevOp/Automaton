using Automaton;

// Create first automaton
var a = new Automate(
    [
        new State(0, isEntry: true, isExit: false),
        new State(1, isEntry: false, isExit: true)
    ],
    [
        new Transition(new State(0, true), new State(1, false, true), 'a'),
        new Transition(new State(1, false, true), new State(0, true), 'b')
    ],
    [ 'a', 'b' ]
);

// Create second automaton
var b = new Automate(
    [
        new State(0, isEntry: true, isExit: false),
        new State(1, isEntry: false, isExit: true)
    ],
    [
        new Transition(new State(0, true), new State(1, false, true), 'x'),
        new Transition(new State(1, false, true), new State(0, true), 'y')
    ],
    [ 'x', 'y']
);

// Perform union
var unionAutomate = Automate.Union(a, b);

unionAutomate.Display_Alphabet();
unionAutomate.Display_Transition();