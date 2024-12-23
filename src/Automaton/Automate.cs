namespace Automaton;

public class Automate
{
    #region fields
    private HashSet<State> States = new();
    private HashSet<Transition> Transitions = new();
    private HashSet<char> Alphabet = new();
    #endregion

    #region getters
    // Getters
    public State[] GetInitStates() => States.Where(s => s.isEntry).ToArray();

    public State[] GetStates() => States.ToArray();

    public State[] GetExitStates() => States.Where(s => s.isExit).ToArray();

    public Transition[] GetTransition() => Transitions.ToArray();

    public char[] GetAlphabet() => Alphabet.ToArray();
    #endregion

    #region modifiers
    // Setters
    public void AddState(State state)
    {
        States.Add(state);
    }

    public void AddStates(State[] states)
    {
        foreach (var state in states)
            States.Add(state);
    }

    public void AddTransition(Transition transition)
    {
        States.Add(transition.startState);
        States.Add(transition.endState);
        Alphabet.Add(transition.value);
        Transitions.Add(transition);
    }

    public void AddTransition(State startState, char value, State endState)
    {
        States.Add(startState);
        States.Add(endState);
        Alphabet.Add(value);

        var transition = new Transition()
        {
            startState = startState,
            value = value,
            endState = endState,
        };

        Transitions.Add(transition);
    }

    public void AddTransitions(Transition[] transitions)
    {
        foreach (var transition in transitions)
        {
            States.Add(transition.startState);
            States.Add(transition.endState);
            Alphabet.Add(transition.value);
            Transitions.Add(transition);
        }
    }

    public void AddLetter(char letter) => Alphabet.Add(letter);

    public void AddLetters(char[] letters)
    {
        foreach (var letter in letters)
            Alphabet.Add(letter);
    }
    #endregion

    #region methods

    /// <summary> Deduce all the possible transitions where we can read the letter from a start state</summary>
    /// <param name="letter"> value to read in the transition</param>
    /// <param name="startState"> the state to start from </param>
    ///
    /// <returns> States ended to when reading the letter</returns>
    public State[] futureStates(State startState, char letter)
    {
        var states = Transitions
            .Where(t => t.startState == startState && t.value == letter)
            .Select(t => t.endState)
            .ToArray();

        return states;
    }

    public State[] futureStates(State[] startStates, char letter)
    {
        var states = new List<State>();

        foreach (var startState in startStates)
        {
            var result = Transitions
                .Where(t => t.startState == startState && t.value == letter)
                .Select(t => t.endState)
                .ToArray();

            if (result.Length > 0)
                states.AddRange(result);
        }
        return states.ToArray();
    }

    public State[] futureStates(State[] startStates)
    {
        var states = new List<State>();

        foreach (var startState in startStates)
        {
            var result = Transitions
                .Where(t => t.startState == startState)
                .Select(t => t.endState)
                .ToArray();

            if (result.Length > 0)
                states.AddRange(result);
        }
        return states.ToArray();
    }

    public State[] PredecessorStates(State[] startStates)
    {
        var states = new List<State>();

        foreach (var startState in startStates)
        {
            var result = Transitions
                .Where(t => t.endState == startState)
                .Select(t => t.startState)
                .ToArray();

            if (result.Length > 0)
                states.AddRange(result);
        }
        return states.ToArray();
    }

    public bool Recognition(string word)
    {
        var outStates = GetInitStates();

        foreach (var letter in word)
        {
            if (!Alphabet.Contains(letter))
                return false;

            outStates = futureStates(outStates, letter);

            if (outStates.Length == 0)
                return false;
        }

        return outStates.Any(s => s.isExit);
    }

    /// <summary> Reduce the automate by removing useless states </summary>
    /// <returns> Trimmed automate </returns>
    public Automate Trim()
    {
        // Step 1 : get accessible states
        var accessibleStates = GetAccessibleStates();
        // Step 2 : get state accessible states that are not coaccessible (useless states)
        var notCoaccessibleStates = GetNotCoaccessibleStates(accessibleStates);

        var usefulStates = new List<State>(accessibleStates.Except(notCoaccessibleStates));

        usefulStates.AddRange(GetInitStates());
        usefulStates.AddRange(GetExitStates());

        // Step 3 : remove transition related to useless states
        var usefulTransitions = Transitions.ToList();

        usefulTransitions.RemoveAll(t =>
            !usefulStates.Contains(t.startState) || !usefulStates.Contains(t.endState)
        );

        // Step 4 : set the automate
        var trimmedAutomaton = new Automate();

        trimmedAutomaton.AddTransitions(usefulTransitions.ToArray());

        return trimmedAutomaton;
    }

    public Automate Intersection(Automate automate2)
    {
        var statePairs = new List<StatePair>();
        var interStates = new List<State>();

        var automate2States = automate2.GetStates();

        foreach (var state in States)
        {
            foreach (var aut2State in automate2States)
            {
                var statePair = new StatePair() { firstState = state, secondState = aut2State };
                var intersectionState = CreateIntersectionState(statePair);
                statePairs.Add(statePair);
                interStates.Add(intersectionState);
            }
        }

        var aut2Transition = automate2.GetTransition();
        var interTransitions = new HashSet<Transition>();
        for (var i = 0; i < statePairs.Count + 1; i++)
        {
            var paireA = statePairs[i];
            for (var j = i + 1; j < statePairs.Count; j++)
            {
                var paireB = statePairs[j];

                var transition = Transitions.FirstOrDefault(t =>
                    t.startState == paireA.firstState && t.endState == paireB.firstState
                );

                if (transition != null)
                {
                    var transitionB = aut2Transition.FirstOrDefault(t =>
                        t.startState == paireA.secondState && t.endState == paireB.secondState
                    );

                    if (transitionB != null && transition.value == transitionB.value)
                    {
                        var interTransition = new Transition()
                        {
                            startState = interStates[i],
                            value = transition.value,
                            endState = interStates[j],
                        };

                        interTransitions.Add(interTransition);
                    }
                }

                var inversedTransition = Transitions.FirstOrDefault(t =>
                    t.startState == paireB.firstState && t.endState == paireA.firstState
                );

                if (inversedTransition == null)
                    continue;

                var inversedTransitionB = aut2Transition.FirstOrDefault(t =>
                    t.startState == paireB.secondState && t.endState == paireA.secondState
                );

                if (
                    inversedTransitionB != null
                    && inversedTransition.value == inversedTransitionB.value
                )
                {
                    var interTransition = new Transition()
                    {
                        startState = interStates[j],
                        value = inversedTransition.value,
                        endState = interStates[i],
                    };

                    interTransitions.Add(interTransition);
                }
            }
        }

        var intersection = new Automate();
        intersection.AddTransitions(interTransitions.ToArray());
        return intersection;
    }
    #endregion
    #region private_helpers

    private State CreateIntersectionState(StatePair statePair) =>
        new()
        {
            id = statePair.Id(),
            isEntry = statePair.firstState.isEntry && statePair.secondState.isEntry,
            isExit = statePair.firstState.isExit && statePair.secondState.isExit,
        };

    private State[] GetAccessibleStates()
    {
        var outStates = GetInitStates();
        var accessibleStates = new HashSet<State>();
        do
        {
            outStates = futureStates(outStates);

            if (outStates.Length == 0)
                break;

            outStates = outStates
                .Where(s => !s.isEntry && !s.isExit && !accessibleStates.Contains(s))
                .ToArray();

            foreach (var state in outStates)
                accessibleStates.Add(state);
        } while (outStates.Length > 0);

        return accessibleStates.ToArray();
    }

    private State[] GetNotCoaccessibleStates(State[] accessibleStates)
    {
        var outStates = GetExitStates();
        var coaccessibleStates = accessibleStates.ToList();
        var proceeded = new HashSet<State>();

        foreach (var state in coaccessibleStates)
            System.Console.WriteLine(state.id);
        do
        {
            outStates = PredecessorStates(outStates);

            if (outStates.Length == 0)
                break;

            outStates = outStates
                .Where(s => !s.isEntry && !s.isExit && !proceeded.Contains(s))
                .ToArray();

            foreach (var state in outStates)
            {
                proceeded.Add(state);
                coaccessibleStates.Remove(state);
            }
        } while (outStates.Length > 0);
        return coaccessibleStates.ToArray();
    }
    #endregion
    #region helpers

    public string PrintAlphabet()
    {
        string alphabet = "{ ";

        foreach (var letter in Alphabet)
            alphabet += $"{letter}, ";

        alphabet += "}";

        return alphabet;
    }

    public String PrintTransition()
    {
        string message = "";

        foreach (var transition in Transitions)
            message += $"{transition.Print()}\r\n";

        return message;
    }
    #endregion
}
