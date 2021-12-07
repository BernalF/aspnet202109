using System;
using Xunit;

namespace WA90.Tests
{
    public class Hello_Tests
    {
        [Fact]
        public void SayHelloWorld()
        {
            // Arrange
            bool varBool;

            // Act
            varBool = true;

            // Assert
            Assert.True(varBool, "No es verdarero");
        }
    }
}
