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

    public void RemoveState(State state)
    {
        if (!States.Contains(state))
            return;

        Transitions.RemoveWhere(t => t.startState == state || t.endState == state);

        States.Remove(state);
    }
    #endregion

    #region methods

    /// <summary> Deduce all the possible transitions where we can read the letter from a start state</summary>
    /// <param name="letter"> value to read in the transition</param>
    /// <param name="startState"> the state to start from </param>
    /// <returns> States ended to when reading the letter</returns>
    public State[] futureStates(State startState, char letter)
    {
        var states = Transitions
            .Where(t => t.startState == startState && t.value == letter)
            .Select(t => t.endState)
            .ToArray();

        return states;
    }

    ///<summary> Check accessible states from a states collection that read a value </summary>
    /// <param name="startStates"> states to check accessible states of </param>
    /// <param name="letter"> Value to read when transiting to the accessible states </param>
    ///<returns> Returns accessible states collection </returns>
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

    ///<summary> Check accessible states from a states collection </summary>
    /// <param name="startStates"> states to check accessible states of </param>
    ///<returns> Returns accessible states collection </returns>
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

    ///<summary> Check precedessors of a group of states </summary>
    /// <param name="startStates"> states to check predecessors of </param>
    ///<returns> Returns predecessor States collection </returns>
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

    /// <summary> Determine if the automate recognize a word </summary>
    /// <param name="word"> The word to recognize </param>
    /// <returns> Returns true if the word is recognised, if not false </returns>
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

        // Step 2.2 : remove useless states.
        var usefulStates = accessibleStates.ToHashSet();
        usefulStates.RemoveWhere(s => notCoaccessibleStates.Contains(s));

        // Step 3 : remove transition related to useless states
        var usefulTransitions = Transitions;

        usefulTransitions.RemoveWhere(t =>
            !usefulStates.Contains(t.startState) || !usefulStates.Contains(t.endState)
        );

        // Step 4 : set the automate
        var trimmedAutomaton = new Automate();

        trimmedAutomaton.AddTransitions(usefulTransitions.ToArray());

        return trimmedAutomaton;
    }

    /// <summary> Determinze the current automate </summary>
    /// <returns> Returns a determinised automate of this automate </returns>
    public Automate Determinize()
    {
        var nextQueue = new Queue<Det_StatesGroup>();

        // Create the super entry state (formed of all initial states)
        var superInitState = new Det_StatesGroup(GetInitStates());
        // Queue it to the super states queue
        nextQueue.Enqueue(superInitState);
        // Group states already visited
        var proceeded = new List<Det_StatesGroup>();
        // Formed transitions
        var det_Transitions = new HashSet<Det_Transtion>();
        do
        {
            Det_StatesGroup? currentStateGroup;
            var dequeued = nextQueue.TryDequeue(out currentStateGroup);

            if (!dequeued || currentStateGroup != null)
                continue;
            //Check for transtion possible with each letter of the automate alphabet
            foreach (var letter in Alphabet)
            {
                var currentStates = currentStateGroup!.states.ToArray();
                var outStates = futureStates(currentStates, letter);

                // if no transition possible skip
                if (outStates.Length < 1)
                    continue;

                // Create state group
                var stateGroup = new Det_StatesGroup(outStates);
                // if it hasn't been visited, add it to the queue
                if (!proceeded.Contains(stateGroup))
                    nextQueue.Enqueue(stateGroup);

                // Create the transtion
                var transition = new Det_Transtion()
                {
                    startState = currentStateGroup,
                    value = letter,
                    endState = stateGroup,
                };

                det_Transitions.Add(transition);
                proceeded.Add(stateGroup);
            }
        } while (nextQueue.Count > 0);

        // Reformat transitions to standard
        var transitions = det_Transitions.Select(t => t.ToTransition()).ToArray();

        // create the automate
        var aut = new Automate();
        aut.AddTransitions(transitions);
        return aut;
    }

    /// <summary> Get the automate recognizing the intersection language of this automate and the second </summary>
    /// <param name="automate2"> the second automate </param>
    /// <returns> Returns the intersection automate </returns>
    public Automate Intersection(Automate automate2)
    {
        var statePairs = new List<StatePair>();
        var interStates = new List<State>();

        // get second automate's states
        var automate2States = automate2.GetStates();
        // form pairs of state between the states collection of the two automates
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
        // Set transitions
        for (var i = 0; i < statePairs.Count - 1; i++)
        {
            var paireA = statePairs[i];

            for (var j = i; j < statePairs.Count; j++)
            {
                var paireB = statePairs[j];

                // Check transition pair firstStates (transitions in first automate)
                // and second States (transition in second automate)
                // read the same value
                var transition = Transitions.FirstOrDefault(t =>
                    t.startState == paireA.firstState && t.endState == paireB.firstState
                );

                if (transition != null)
                {
                    var transitionB = aut2Transition.FirstOrDefault(t =>
                        t.startState == paireA.secondState && t.endState == paireB.secondState
                    );

                    // if they read the same value, create transtion between the first pair to the second reading
                    // the corresponding value
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
                if (paireA == paireB)
                    continue;
                // do the same thing for second state pair to first one
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
        // create the intersection from the transitions formed
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
        var accessibleStates = new HashSet<State>(outStates);
        do
        {
            outStates = futureStates(outStates);

            if (outStates.Length == 0)
                break;

            outStates = outStates.Where(s => !accessibleStates.Contains(s)).ToArray();

            foreach (var state in outStates)
                accessibleStates.Add(state);
        } while (outStates.Length > 0);

        return accessibleStates.ToArray();
    }

    private State[] GetNotCoaccessibleStates(State[] accessibleStates)
    {
        var outStates = accessibleStates.Where(s => s.isExit && !s.isEntry).ToArray();
        var coaccessibleStates = accessibleStates.ToList();
        var proceeded = new HashSet<State>();

        do
        {
            outStates = PredecessorStates(outStates);

            if (outStates.Length == 0)
                break;

            outStates = outStates.Where(s => !proceeded.Contains(s)).ToArray();

            foreach (var state in outStates)
            {
                proceeded.Add(state);

                var contained = coaccessibleStates.Remove(state);

                if (!contained)
                    coaccessibleStates.Add(state);
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

    public String PrintTransitions()
    {
        string message = "";

        foreach (var transition in Transitions)
            message += $"{transition.Print()}\r\n";

        return message;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Automate autB)
            return false;

        var stateEqual = States.SetEquals(autB.GetStates());

        var transitionsEqual = Transitions.SetEquals(autB.GetTransition());

        var alphabetEqual = Alphabet.SetEquals(autB.GetAlphabet());

        return stateEqual && transitionsEqual && alphabetEqual;
    }

    public override int GetHashCode()
    {
        return Transitions.GetHashCode();
    }
    #endregion
}
