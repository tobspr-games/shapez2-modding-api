namespace ShapezShifter
{
    public interface IGameScenarioExtender : IExtender
    {
        GameScenario ExtendGameScenario(GameScenario gameScenario);
    }
}