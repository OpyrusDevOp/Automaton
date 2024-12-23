using Automaton;

namespace AutomatonTest;

public class AutomateTest
{
    Automate automate = new();

    public AutomateTest()
    {
        State firstState =
            new()
            {
                id = "0",
                isEntry = true,
                isExit = true,
            };

        State secondState =
            new()
            {
                id = "1",
                isEntry = false,
                isExit = false,
            };

        automate.AddTransition(firstState, 'b', firstState);
        automate.AddTransition(firstState, 'a', secondState);
        automate.AddTransition(secondState, 'b', secondState);
        automate.AddTransition(secondState, 'a', firstState);
    }

    [Fact]
    public void CorrectAlphabet()
    {
        var answer = "{ b, a, }";

        Assert.Equivalent(answer, automate.PrintAlphabet());
    }

    [Fact]
    public void WordRecognition() => Assert.True(automate.Recognition("aa"));

    [Fact]
    public void WordRecognition2() => Assert.True(automate.Recognition("baba"));

    [Fact]
    public void WordRecognition3() => Assert.False(automate.Recognition("aacs"));
}
