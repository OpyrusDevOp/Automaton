# Automaton

## Description

The **Automaton** project aims to develop an automaton that determines if a word belongs to a given language. It also includes the construction of an automaton that recognizes the intersection of two languages based on two distinct automatons.

## Features

- [x] **State and Transition Display**: Displays the states and transitions of the automaton.
- [x] **Language Recognition**: Checks if a word belongs to a language defined by an automaton.
- [x] **Automaton Product**: Generates an automaton that recognizes the intersection of two languages from two distinct automatons.

### Future Features

- [ ] **Union of Languages**: Implement the creation of an automaton that recognizes the union of two languages.
- [ ] **Complete an Automaton**: Add a sink state if necessary to complete the automaton.
- [ ] **Complement of a Language**: From a deterministic automaton, return an automaton that recognizes the complement of the language.
- [ ] **Non-Deterministic to Deterministic Conversion**: Convert a non-deterministic automaton into a deterministic one that recognizes the same language.
- [ ] **Check for Emptiness**: Determine if the language recognized by an automaton is empty.
- [ ] **Check for Infinity**: Determine if the language recognized by an automaton is infinite.

## Installation

1. Clone the repository

2. Open the project in your preferred integrated development environment (IDE).

3. Build and run the project.

## Usage

### 1. Create an Automaton

To create an automaton, define its states and transitions. For example:

```csharp
State[] states =
{
    new State(1, true),  // Entry state
    new State(2),
    new State(3, false, true),  // Exit state
};

Transition[] transitions =
{
    new Transition(states[0], states[1], 'a'),
    new Transition(states[1], states[2], 'b'),
};

Automate automate = new Automate(states, transitions);
```

### 2. Check a Word

Use the `Is_Recognized` method to check if a word belongs to the automaton:

```csharp
var word = "ab";
bool recognized = automate.Is_Recognized(word);
Console.WriteLine(recognized ? $"{word} belongs to the language" : $"{word} does not belong to the language");
```

### 3. Intersection of Two Automatons

To create an automaton that recognizes the intersection of two languages, use the `Intersection` method:

```csharp
Automate intersectedAutomate = Automate.Intersection(automateA, automateB);
```

## Contribution

Contributions are welcome! Please submit a pull request or open an issue to discuss improvements.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

## Acknowledgments

- [Automata and Languages](https://en.wikipedia.org/wiki/Finite-state_machine) - Inspiration source for automaton theory.
- Thanks to all the contributors who helped move this project forward!
