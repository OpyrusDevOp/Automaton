namespace Automaton;

public class Automate
{
    #region Constructors

    public Automate(State[] states, Transition[] transitions)
        {
            States = new Dictionary<int, State>();
            EntryStates = new Dictionary<int, State>();
            ExitStates = new Dictionary<int, State>();
            Alphabet = [];
            Transitions = [];
    
            foreach (var state in states)
            {
                States.Add(state.Id, state);
                switch (state.IsEntry)
                {
                    case true when state.IsExit:
                        throw new Exception();
                    case true:
                        EntryStates.Add(state.Id, state);
                        break;
                }
    
                if (state.IsExit)
                    ExitStates.Add(state.Id, state);
            }
    
            foreach (var transition in transitions)
            {
                if (!Alphabet.Contains(transition.Value))
                    Alphabet.Add(transition.Value);
    
                Transitions.Add(transition);
            }
        }
    
    public Automate(State[] states, Transition[] transitions, char[] alphabet)
        {
            States = new Dictionary<int, State>();
            EntryStates = new Dictionary<int, State>();
            ExitStates = new Dictionary<int, State>();
            Alphabet = [..alphabet];
            Transitions = [..transitions];
    
            foreach (var state in states)
            {
                States.Add(state.Id, state);
                switch (state.IsEntry)
                {
                    case true when state.IsExit:
                        throw new Exception();
                    case true:
                        EntryStates.Add(state.Id, state);
                        break;
                }
    
                if (state.IsExit)
                    ExitStates.Add(state.Id, state);
            }
        }

    #endregion

    #region Fields

    private readonly Dictionary<int, State> States;
    private readonly Dictionary<int, State> EntryStates;
    private readonly Dictionary<int, State> ExitStates;
    private readonly List<Transition> Transitions;
    private readonly List<char> Alphabet;

    #endregion
    
    #region Methods

        public void Display_Alphabet()
        {
            Console.Write("Alphabet : { ");
    
            foreach (var letter in Alphabet)
            {
                Console.Write($"{letter}, ");
            }
    
            Console.WriteLine("}");
        }
    
        public void Display_Transition()
        {
            Console.WriteLine("Transitions : ");
            foreach (var transition in Transitions)
            {
                Console.WriteLine(
                    $"({transition.InitialState.Id}) --{transition.Value}--> ({transition.LastState.Id})"
                );
            }
        }
    
        public int? Out_State(int initialState, char value)
        {
            var t = Transitions.FirstOrDefault(t => t.InitialState.Id == initialState && t.Value == value);
    
            return t?.LastState.Id;
        }

        private int[]? Out_States(int[] initialState, char value)
        {
            var transitions = Transitions.FindAll(t =>
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
            return word.All(character => Alphabet.Contains(character));
        }
    
        public bool Is_Recognized(string word)
        {
            if (!In_Alphabet(word))
                return false;
    
            int[]? currentPosition = EntryStates.Keys.ToArray();
    
            foreach (var character in word)
            {
                currentPosition = Out_States(currentPosition, character);
    
                if (currentPosition == null || currentPosition.Length < 1)
                    return false;
            }
    
            foreach (var pos in currentPosition)
            {
                if (States[pos].IsExit)
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
        
        var alphabet = a.Alphabet.Intersect(b.Alphabet).ToArray();
        var transitions = new List<Transition>();
        var states = new List<State>();
        
        
        var aIntersetions = a.Transitions.FindAll(t => alphabet.Contains(t.Value));
        var bIntersetions = b.Transitions.FindAll(t => alphabet.Contains(t.Value));
        
        foreach (var state in from aState in a.States.Values from bState in b.States.Values select new IntersecState
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
    
    #endregion
    
    
}
