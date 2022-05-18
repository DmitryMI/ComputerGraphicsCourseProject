using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CgCourseProject.Maths;

namespace CgCourseProjectTests
{
    [TestClass]
    public class Matrix4Testing
    {

        [TestMethod]
        public void TestZeroEulerMatrix()
        {
            Matrix4 euler = Matrix4.GetRotationMatrix(Vector3.Zero);

            bool flag = true;

            for(int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                if (i == j && Math.Abs(euler[i, j] - 1) > .0001f)
                    flag = false;
                if (i != j && Math.Abs(euler[i, j]) > .0001f)
                    flag = false;
            }

            if(!flag)
                Assert.Fail();
        }
        
    }
}
