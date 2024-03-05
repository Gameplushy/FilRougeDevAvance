using ConnectionToLife.GameOfLife;

namespace TestsUnitaires
{
    public class GameOfLifeTests
    {
        [Fact]
        public void ValidString_CallingNewBoard_SetsCorrectRules()
        {
            Board b = new Board("23A3D");

            Assert.Equal([3], b.BirthRules);
            Assert.Equal([2,3], b.SurviveRules);
        }

        [Fact]
        public void InvalidString_CallingNewBoard_ThrowsException()
        {
            Assert.Throws<FormatException>(()=>new Board("oops"));
        }
    }
}