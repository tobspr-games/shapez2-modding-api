namespace ShapezShifter.Hijack
{
    public interface IGameScenarioRewirer : IRewirer
    {
        GameScenario ModifyGameScenario(GameScenario gameScenario);
    }
}