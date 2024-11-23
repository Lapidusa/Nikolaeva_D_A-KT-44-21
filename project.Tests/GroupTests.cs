using project.Models;

namespace project.Tests
{
    public class GroupTests
    { 
        [Fact]
        public void isValidGroupName_KT4421_True()
        {
            var testGroup = new Group
            {
                GroupName = "КТ-44-21"
            };
        
            var result = testGroup.isValidGroupName();
        
            Assert.True(result);
        }
    }
}

