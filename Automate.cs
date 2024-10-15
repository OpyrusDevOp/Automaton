namespace Automaton;

public class Automate
{
    #region Constructors

    public Automate(State[] states, Transition[] transitions)
        {
            _states = new Dictionary<int, State>();
            _entryStates = new Dictionary<int, State>();
            _exitStates = new Dictionary<int, State>();
            _alphabet = [];
            _transitions = [];
    
            foreach (var state in states)
            {
                _states.Add(state.Id, state);
                switch (state.IsEntry)
                {
                    case true when state.IsExit:
                        throw new Exception();
                    case true:
                        _entryStates.Add(state.Id, state);
                        break;
                }
    
                if (state.IsExit)
                    _exitStates.Add(state.Id, state);
            }
    
            foreach (var transition in transitions)
            {
                if (!_alphabet.Contains(transition.Value))
                    _alphabet.Add(transition.Value);
    
                _transitions.Add(transition);
            }
        }
    
    public Automate(State[] states, Transition[] transitions, char[] alphabet)
        {
            _states = new Dictionary<int, State>();
            _entryStates = new Dictionary<int, State>();
            _exitStates = new Dictionary<int, State>();
            _alphabet = [..alphabet];
            _transitions = [..transitions];
    
            foreach (var state in states)
            {
                _states.Add(state.Id, state);
                if(state.IsEntry)
                    _entryStates.Add(state.Id, state);
    
                if (state.IsExit)
                    _exitStates.Add(state.Id, state);
            }
        }

    #endregion

    #region Fields

    private readonly Dictionary<int, State> _states;
    private readonly Dictionary<int, State> _entryStates;
    private readonly Dictionary<int, State> _exitStates;
    private readonly List<Transition> _transitions;
    private readonly List<char> _alphabet;

    #endregion
    
    #region Methods

        public void Display_Alphabet()
        {
            Console.WriteLine($"Alphabet: {{ {string.Join(", ", _alphabet)} }}");
        }
    
        public void  Display_Transition()
        {
            Console.WriteLine("Transitions : ");
            foreach (var transition in _transitions)
            {
                Console.WriteLine(
                    $"({transition.InitialState.Id}) --{transition.Value}--> ({transition.LastState.Id})"
                );
            }
        }
    
        public int? Out_State(int initialState, char value)
        {
            var t = _transitions.FirstOrDefault(t => t.InitialState.Id == initialState && t.Value == value);
    
            return t?.LastState.Id;
        }

        private int[]? Out_States(int[] initialState, char value)
        {
            var transitions = _transitions.FindAll(t =>
                initialState.Contains(t.InitialState.Id) && t.Value == value
            );
    
            if (transitions.Count < 1)
                 return Array.Empty<int>();;
    
            var outStates = new List<int>();
    
            foreach (var transition in transitions)
            {
                var id = transition.LastState.Id;
                if (!outStates.Contains(id))
                    outStates.Add(id);
            }
    
            return outStates.ToArray();
        }

        private bool In_Alphabet(string word)
        {
            return word.All(character => _alphabet.Contains(character));
        }
    
        public bool Is_Recognized(string word)
        {
            if (!In_Alphabet(word))
                return false;
    
            int[]? currentPosition = _entryStates.Keys.ToArray();
    
            foreach (var character in word)
            {
                currentPosition = Out_States(currentPosition, character);
    
                if (currentPosition == null || currentPosition.Length < 1)
                    return false;
            }
    
            foreach (var pos in currentPosition)
            {
                if (_states[pos].IsExit)
                    return true;
            }
    
            return false;
        }

    #endregion

    #region StaticMethods

    public static Automate Intersection(Automate a, Automate b)
    {
        var intersectingStates = new Dictionary<int, IntersecState>();
        var id = 0;
        
        var alphabet = a._alphabet.Intersect(b._alphabet).ToArray();
        var transitions = new List<Transition>();
        var states = new List<State>();
        
        
        var aIntersetions = a._transitions.FindAll(t => alphabet.Contains(t.Value));
        var bIntersetions = b._transitions.FindAll(t => alphabet.Contains(t.Value));
        
        foreach (var state in from aState in a._states.Values from bState in b._states.Values select new IntersecState
                 {
                     AutoAState = aState,
                     AutoBState = bState,
                     IsEntry = aState.IsEntry && bState.IsEntry,
                     IsExit = aState.IsExit && bState.IsExit
                 })
        {
            intersectingStates.Add(id, state);
            states.Add(State.FromIntersecState(state, id));
            id++;
        }

        for (var i = 0; i < intersectingStates.Count-1; i++)
        {
            for (var j = i+1; j < intersectingStates.Count; j++)
            {
                var currentState = intersectingStates[i];
                var comparedState = intersectingStates[j];

                var aTransition = aIntersetions.FirstOrDefault(t =>
                    t.InitialState == currentState.AutoAState && t.LastState == comparedState.AutoAState);
                
                var bTransition = bIntersetions.FirstOrDefault(t =>
                    t.InitialState == currentState.AutoBState && t.LastState == comparedState.AutoBState);

                if (aTransition != null && bTransition != null && aTransition.Value == bTransition.Value)
                {
                    var initialState = states.First(s => s.Id == i);
                    var lastState = states.First(s => s.Id == j);

                    transitions.Add(
                        new Transition(initialState, lastState, aTransition.Value)
                    );
                }
                var reverseATransition = aIntersetions.FirstOrDefault(t =>
                    t.LastState == currentState.AutoAState && t.InitialState == comparedState.AutoAState);
                
                var reverseBTransition = bIntersetions.FirstOrDefault(t =>
                    t.LastState == currentState.AutoBState && t.InitialState == comparedState.AutoBState);
                
                if(reverseATransition == null || reverseBTransition == null || reverseATransition.Value != reverseBTransition.Value) continue;
                    
                var reverseInitialState = states.First(s => s.Id == j);
                var reverseLastState = states.First(s => s.Id == i);

                transitions.Add(
                    new Transition(reverseInitialState, reverseLastState, reverseATransition.Value)
                );
                    
            }
        }
        var oldStateArray  = states.ToArray();
        
        foreach (var state in oldStateArray)
        {
            if(transitions.Any(t => t.InitialState == state || t.LastState == state)) continue;

            states.Remove(state);
        }
        return new Automate(states.ToArray(), transitions.ToArray(), alphabet);
    }
    
    
    public static Automate Union(Automate a, Automate b)
{
    var intersectingStates = new Dictionary<int, IntersecState>();
    var id = 0;
    var alphabet = a._alphabet.Union(b._alphabet).ToArray();
    var transitions = new HashSet<Transition>();
    var states = new List<State>();

    foreach (var state in from aState in a._states.Values from bState in b._states.Values select new IntersecState
    {
        Id = id,
        AutoAState = aState,
        AutoBState = bState,
        IsEntry = aState.IsEntry || bState.IsEntry,
        IsExit = aState.IsExit || bState.IsExit
    })
    {
        intersectingStates.Add(id, state);
        states.Add(State.FromIntersecState(state, id));
        id++;
    }

    void AddTransition(IntersecState currentState, IntersecState comparedState)
    {
        var initialState = states.First(s => s.Id == currentState.Id);
        var lastState = states.First(s => s.Id == comparedState.Id);

        if (currentState.AutoAState.Id != comparedState.AutoAState.Id)
        {
            var aTransition = a._transitions.FirstOrDefault(t => t.InitialState == currentState.AutoAState && t.LastState == comparedState.AutoAState);
            if (aTransition != null)
            {
                transitions.Add(new Transition(initialState, lastState, aTransition.Value));
            }
        }

        if (currentState.AutoBState.Id != comparedState.AutoBState.Id)
        {
            var bTransition = b._transitions.FirstOrDefault(t => t.InitialState == currentState.AutoBState && t.LastState == comparedState.AutoBState);
            if (bTransition != null)
            {
                transitions.Add(new Transition(initialState, lastState, bTransition.Value));
            }
        }
    }

    for (var i = 0; i < intersectingStates.Count - 1; i++)
    {
        for (var j = i + 1; j < intersectingStates.Count; j++)
        {
            AddTransition(intersectingStates[i], intersectingStates[j]);
        }
    }

    return new Automate(states.ToArray(), transitions.ToArray(), alphabet);
}

    
    #endregion
    
    
}
