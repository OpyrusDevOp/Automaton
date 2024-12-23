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
        return id;
    }

    public override int GetHashCode() => id.GetHashCode();
}

public record StatePair
{
    public required State firstState;
    public required State secondState;

    public string Id() => $"{firstState.id} {secondState.id}";
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

    public string Print() => $"({startState.ToString()}) --{value}--> ({endState.ToString()})";
}
