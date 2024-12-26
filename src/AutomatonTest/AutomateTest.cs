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

    [Fact]
    public void Intersection1()
    {
        Automate GetAutomate1()
        {
            Automate aut = new();
            var q0 = new State()
            {
                id = "q0",
                isEntry = true,
                isExit = true,
            };
            var q1 = new State() { id = "q1" };
            var q2 = new State() { id = "q2", isExit = true };

            Transition[] transitions =
            {
                new Transition()
                {
                    startState = q0,
                    value = 'b',
                    endState = q0,
                },
                new Transition()
                {
                    startState = q0,
                    value = 'a',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'b',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'a',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'a',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'b',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'a',
                    endState = q1,
                },
            };

            aut.AddTransitions(transitions);
            return aut;
        }

        Automate GetAutomate2()
        {
            Automate aut = new();
            var q0 = new State()
            {
                id = "q0",
                isEntry = true,
                isExit = true,
            };
            var q1 = new State() { id = "q1" };
            var q3 = new State() { id = "q3", isExit = true };
            var q2 = new State() { id = "q2" };
            Transition[] transitions =
            {
                new Transition()
                {
                    startState = q0,
                    value = 'b',
                    endState = q0,
                },
                new Transition()
                {
                    startState = q0,
                    value = 'a',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'b',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'a',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'b',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'a',
                    endState = q3,
                },
                new Transition()
                {
                    startState = q3,
                    value = 'b',
                    endState = q3,
                },
                new Transition()
                {
                    startState = q3,
                    value = 'a',
                    endState = q1,
                },
            };

            aut.AddTransitions(transitions);
            return aut;
        }

        Automate GetIntersectionAutomate()
        {
            Automate aut = new();
            var q0 = new State()
            {
                id = "q0",
                isEntry = true,
                isExit = true,
            };
            var q1 = new State() { id = "q1" };
            var q2 = new State() { id = "q2" };
            var q3 = new State() { id = "q3" };
            var q4 = new State() { id = "q4" };
            var q5 = new State() { id = "q5" };
            var q6 = new State() { id = "q6", isExit = true };
            Transition[] transitions =
            {
                new Transition()
                {
                    startState = q0,
                    value = 'b',
                    endState = q0,
                },
                new Transition()
                {
                    startState = q0,
                    value = 'a',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'b',
                    endState = q1,
                },
                new Transition()
                {
                    startState = q1,
                    value = 'a',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'b',
                    endState = q2,
                },
                new Transition()
                {
                    startState = q2,
                    value = 'a',
                    endState = q3,
                },
                new Transition()
                {
                    startState = q3,
                    value = 'b',
                    endState = q3,
                },
                new Transition()
                {
                    startState = q3,
                    value = 'a',
                    endState = q4,
                },
                new Transition()
                {
                    startState = q4,
                    value = 'b',
                    endState = q4,
                },
                new Transition()
                {
                    startState = q4,
                    value = 'a',
                    endState = q5,
                },
                new Transition()
                {
                    startState = q5,
                    value = 'b',
                    endState = q5,
                },
                new Transition()
                {
                    startState = q5,
                    value = 'a',
                    endState = q6,
                },
                new Transition()
                {
                    startState = q6,
                    value = 'b',
                    endState = q6,
                },
                new Transition()
                {
                    startState = q6,
                    value = 'a',
                    endState = q6,
                },
            };

            aut.AddTransitions(transitions);
            return aut;
        }

        Automate aut1 = GetAutomate1();
        Automate aut2 = GetAutomate2();

        Automate intersection = aut1.Intersection(aut2).Trim();

        var expected = GetIntersectionAutomate();

        Console.WriteLine("Expected : ");
        Console.WriteLine(expected.PrintTransitions());

        Console.WriteLine("Result : ");
        Console.WriteLine(intersection.PrintTransitions());

        Assert.Equivalent(expected, intersection, true);
    }

    [Fact]
    public void DeterminisationTest()
    {
        Automate GetNonDet()
        {
            Automate automate = new();

            // Déclaration des états
            State state1 = new() { id = "1", isEntry = true };

            State state2 = new() { id = "2", isExit = true };

            State state3 = new() { id = "3" };

            State state4 = new() { id = "4" };

            // Ajout des transitions
            automate.AddTransition(state1, 'a', state1);
            automate.AddTransition(state1, 'a', state3);
            automate.AddTransition(state1, 'b', state2);

            automate.AddTransition(state2, 'b', state1);
            automate.AddTransition(state2, 'b', state4);

            automate.AddTransition(state3, 'a', state4);
            automate.AddTransition(state3, 'b', state2);

            automate.AddTransition(state4, 'a', state4);
            automate.AddTransition(state4, 'b', state2);
            return automate;
        }

        Automate GetExpectedAut()
        {
            Automate automate = new();

            // Déclaration des états
            State state1 = new() { id = "1", isEntry = true };

            State state2 = new() { id = "2", isExit = true };

            State state13 = new() { id = "1 3" };

            State state14 = new() { id = "1 4" };
            State state134 = new() { id = "1 3 4" };

            automate.AddTransition(state1, 'b', state2);
            automate.AddTransition(state1, 'a', state13);
            automate.AddTransition(state2, 'b', state14);
            automate.AddTransition(state13, 'b', state2);
            automate.AddTransition(state13, 'a', state134);
            automate.AddTransition(state134, 'a', state134);
            automate.AddTransition(state134, 'b', state2);
            automate.AddTransition(state14, 'a', state134);
            automate.AddTransition(state14, 'b', state2);

            return automate;
        }

        var expected = GetExpectedAut();

        Console.WriteLine("Expected  (DeterminisationTest) : ");
        Console.WriteLine(expected.PrintTransitions());

        var nonDetAut = GetNonDet();
        var result = nonDetAut.Determinize();

        Console.WriteLine("Result (DeterminisationTest) : ");
        Console.WriteLine(expected.PrintTransitions());

        Assert.Equivalent(expected, result, true);
    }
}
