namespace Automaton;

public class State(int id, bool isEntry = false, bool isExit = false)
{
    

    public readonly int Id = id;
    public readonly bool IsEntry = isEntry;
    public readonly bool IsExit = isExit;
    

    public override int GetHashCode()
    {
        return Id;
    }

    public static bool operator ==(State? left, State? right)
    {
        if (left is null || right is null) return false;
        return left.Id == right.Id && left.IsEntry == right.IsEntry && left.IsExit == right.IsExit;
    }

    public static bool operator !=(State? left, State? right)
    {
        if (left is null || right is null) return false;
        return left.Id != right.Id || left.IsEntry != right.IsEntry || left.IsExit != right.IsExit;
    }

    public static State FromIntersecState(IntersecState intersecState, int id)
    {
        return new State(id, intersecState.IsEntry, intersecState.IsExit);
    }
}


public record IntersecState
{
    public int Id;
    public required State AutoAState { get; set; }
    public required State AutoBState { get; set; }
    public required bool IsEntry;
    public required bool IsExit;
}