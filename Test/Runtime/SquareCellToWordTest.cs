// using NUnit.Framework;
// using UnityTest;
// using UnityEngine;
// using VitoBarra.GeneralUtility.FeatureFullValue;
// using VitoBarra.GridSystem.Framework;
// using VitoBarra.GridSystem.Square;
// namespace Test.Runtime
// {
//     [TestFixture]
//     public class SquareCellToWordTest
//     {
//
//
//         [Test]
//         [TestCase(0.5f, 0.5f)]
//         [TestCase(1.5f, 1.5f)]
//         [TestCase(4.5f, 1.5f)]
//         public void SquareCellToWordTest_PositionToCell_CellToPosition_SamePosition(float x, float y)
//         {
//             SquareGridToWord squareGridToWord = new SquareGridToWord(new Vector2Hook(0,0), 1);
//             squareGridToWord.SetBounds(10, 10);
//
//             var centerPosition = new Vector3(x, y,0);
//             var cell = squareGridToWord.GetNearestCell(centerPosition);
//             var calculateCenterCell = squareGridToWord.GetWordPositionCenterCell(cell);
//
//             Assert.AreEqual(centerPosition, calculateCenterCell);
//         }
//     }
// }