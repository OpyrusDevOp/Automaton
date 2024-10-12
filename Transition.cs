namespace Test;

public class Transition(State initialState, State lastState, char value)
{
    private bool Equals(Transition other)
    {
        return InitialState.Equals(other.InitialState) && LastState.Equals(other.LastState) && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Transition)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InitialState, LastState, Value);
    }

    public static bool operator ==(Transition? left, Transition? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Transition? left, Transition? right)
    {
        return !Equals(left, right);
    }

    public readonly State InitialState = initialState;
    public readonly State LastState = lastState;
    public readonly char Value = value;
}