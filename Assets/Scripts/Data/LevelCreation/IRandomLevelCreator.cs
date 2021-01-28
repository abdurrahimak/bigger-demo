namespace BiggerDemo.Data
{
    public interface IRandomLevelCreator
    {
        LevelData GenerateRandomLevel(LevelDifficult levelDifficult);
    }
}
