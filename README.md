# Automaton Library

This is a C# implementation of an **Automaton**, a computational structure used in formal language theory, for recognizing and manipulating languages. The library supports various operations such as word recognition, determinization, and intersection of languages.

## Features

- **State Management**
  - Add, remove, and query states in the automaton.
- **Transitions**
  - Add transitions between states using symbols from the defined alphabet.
- **Alphabet**
  - Add and manage symbols used in transitions.
- **Recognition**
  - Determine if the automaton recognizes a specific word.
- **Trim the automaton** by removing unreachable and unnecessary states.
- **Determinisation**
  - Convert a non-deterministic automaton to a deterministic one.
- **Intersection**
  - Compute the intersection of the languages recognized by two automatons.
- **Utility Functions**
  - Display the alphabet, transitions, and other information about the automaton.

## Usage

### Class Overview

- `Automate`: The main class for creating and manipulating automatons.
- `State`: Represents a state in the automaton with attributes like `id`, `isEntry`, and `isExit`.
- `Transition`: Represents a transition between two states with a specific value.
- `Det_StatesGroup`: A helper class for managing grouped states during determinisation.
- `StatePair`: Represents pairs of states used in the intersection of languages.

### Basic Example

#### Creating an Automaton

```csharp
var automaton = new Automate();

// Add states
var state1 = new State { id = "q0", isEntry = true };
var state2 = new State { id = "q1", isExit = true };

automaton.AddStates(new[] { state1, state2 });

// Add transitions
automaton.AddTransition(state1, 'a', state2);

// Add letters to the alphabet
automaton.AddLetter('a');

// Check if a word is recognized
bool isRecognized = automaton.Recognition("a"); // Returns true
```

#### Determinisation

```csharp
var deterministicAutomaton = automaton.Determinize();
```

#### Language Intersection

```csharp
var secondAutomaton = new Automate();
// Add states and transitions to the second automaton...
var intersectionAutomaton = automaton.Intersection(secondAutomaton);
```

## Methods

### State and Transition Management

- `AddState(State state)`
- `AddTransition(State startState, char value, State endState)`
- `RemoveState(State state)`

### Recognition and Validation

- `bool Recognition(string word)`: Checks if the automaton recognizes a word.

### Automaton Operations

- `Automate Trim()`: Removes unreachable or unnecessary states.
- `Automate Determinize()`: Converts the automaton into a deterministic version.
- `Automate Intersection(Automate automate2)`: Computes the intersection of two automatons.

### Helper Methods

- `string PrintAlphabet()`: Returns a string representation of the automaton's alphabet.
- `string PrintTransitions()`: Returns a string representation of the automaton's transitions.

## Dependencies

This library has no external dependencies and works with standard .NET libraries.

## Future Enhancements

- Support for union, complement and completion operations.
- Visual representation of the automaton.

## License

This project is licensed under the MIT License.
