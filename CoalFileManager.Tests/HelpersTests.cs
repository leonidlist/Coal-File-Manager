using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coal.Draw;

namespace CoalFileManager.Tests {
    [TestClass]
    public class HelpersTests {
        [TestMethod]
        public void MultiplySpace_SpaceAnd5_5SpacesReturned() {
            //arrange
            string space = " ";
            int multiplier = 5;
            string expected = "      ";
            //act
            string actual = space.MultiplySpace(multiplier);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetTopBorder_6_TopBorderReturned() {
            //arrange
            int width = 6;
            string expected = "╔════╗";
            //act
            string actual = Borders.GetTopBorder(width);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetMiddleBorder_8_MiddleBorderReturned() {
            //arrange
            int width = 8;
            string expected = "║      ║";
            //act
            string actual = Borders.GetMiddleBorder(width);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetBottomBorder_10_BottomBorderReturned() {
            //arrange
            int width = 10;
            string expected = "╚════════╝";
            //act
            string actual = Borders.GetBottomBorder(width);
            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
