using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;
using CgCourseProject.Graphics;
using CgCourseProject.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CgCourseProjectTests
{

    [TestClass]
    public class MathTesting
    {
        Random rnd = new Random();

        [TestMethod]
        public void TestClamp()
        {
            int min = 0;
            int max = 10;

            // Less than min
            int value = rnd.Next(-5, -1);
            int clampMin = MathUtils.Clamp(value, min, max);
            Assert.AreEqual(min, clampMin);

            // Greater than max
            value = rnd.Next(11, 15);
            int clampMax = MathUtils.Clamp(value, min, max);
            Assert.AreEqual(max, clampMax);
            
            // Max is less than min, value is inside
            max = 0;
            min = 10;
            value = 5;
            int clampValue = MathUtils.Clamp(value, min, max);
            Assert.AreEqual(value, clampValue);

        }

        /*[TestMethod]
        public void TestLerpInt()
        {
            int xl = -50;
            int xr = 50;

            int yMin = 10;
            int yMax = -10;

            List<double> lerpDouble = Camera.Interpolate(xl, yMin, xr, yMax);
            List<int> lertInt = Camera.InterpolateInt(xl, yMin, xr, yMax);

            int[] lerpDoubleVals = new int[lerpDouble.Count];
            for (int i = 0; i < lerpDoubleVals.Length; i++)
            {
                lerpDoubleVals[i] = (int) Math.Round(lerpDouble[i]);
            }

            int[] lerpIntVals = lertInt.ToArray();

            bool equal = lerpDoubleVals.Length == lerpIntVals.Length;

            for (int i = 0; i < lerpDoubleVals.Length && equal; i++)
            {
                equal = lerpDoubleVals[i] == lerpIntVals[i];
            }

            if(!equal)
                Assert.Fail();
        }*/
    }

    
}
