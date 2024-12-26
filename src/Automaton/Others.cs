public class State
{
    public required string id { get; init; }
    public bool isEntry { get; set; } = false;
    public bool isExit { get; set; } = false;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not State stateB)
            return false;
        return id == stateB.id;
    }

    public override string ToString()
    {
        var stateString = string.Empty;
        if (isEntry)
            stateString += "->";

        stateString += isExit ? $"(({id}))" : $"({id})";
        return stateString;
    }

    public override int GetHashCode() => id.GetHashCode();
}

public record StatePair
{
    public required State firstState;
    public required State secondState;

    public string Id() => $"{firstState.id} {secondState.id}";
}

public class Det_StatesGroup
{
    public HashSet<State> states { get; private set; } = new();
    public bool isExit { get; set; } = true;
    public bool isEntry { get; set; } = true;

    public Det_StatesGroup() { }

    public Det_StatesGroup(State state) => AddState(state);

    public Det_StatesGroup(State[] states) => AddState(states);

    public void AddState(State state)
    {
        var added = states.Add(state);

        if (!added)
            return;

        isEntry &= state.isEntry;
        isExit |= state.isExit;
    }

    public void AddState(State[] states)
    {
        foreach (var state in states)
        {
            var added = this.states.Add(state);

            if (!added)
                return;

            isEntry &= state.isEntry;
            isExit |= state.isExit;
        }
    }

    public string Id()
    {
        var id = string.Empty;

        foreach (var state in states)
            id += $"{state.id} ";

        return id.Trim();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Det_StatesGroup stateGroupB)
            return false;

        var hasSameStates = stateGroupB.states == states;
        var areBothEntry = stateGroupB.isEntry == isEntry;
        var areBothExit = stateGroupB.isExit == isExit;
        return hasSameStates && areBothExit && areBothEntry;
    }

    public override int GetHashCode()
    {
        return states.GetHashCode();
    }

    public static State ToState(Det_StatesGroup group) =>
        new()
        {
            id = group.Id(),
            isEntry = group.isEntry,
            isExit = group.isExit,
        };
}

public class Det_Transtion
{
    public required Det_StatesGroup startState { get; set; }
    public char value { get; set; }
    public required Det_StatesGroup endState { get; set; }

    public Transition ToTransition() =>
        new()
        {
            startState = Det_StatesGroup.ToState(startState),
            value = value,
            endState = Det_StatesGroup.ToState(endState),
        };

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Det_Transtion transtionB)
            return false;
        return startState.Equals(transtionB.startState)
            && value == transtionB.value
            && endState.Equals(transtionB.endState);
    }

    public override int GetHashCode() =>
        startState.GetHashCode() + value.GetHashCode() + endState.GetHashCode();
}

public class Transition
{
    public required State startState { get; set; }
    public char value { get; set; }
    public required State endState { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Transition transition)
            return false;

        return startState == transition.startState
            && value == transition.value
            && endState == transition.endState;
    }

    public override int GetHashCode() =>
        startState.GetHashCode() + value.GetHashCode() + endState.GetHashCode();

    public string Print() => $"{startState.ToString()} --{value}--> {endState.ToString()}";
}
