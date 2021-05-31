using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TaskWinTest
    {
        [Test]
        [TestCase(null)]
        [TestCase(-1)]
        [TestCase(100)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(10000)]
        [TestCase(0)]
        [TestCase(2)]

        public void TaskWinTestSimplePasses(int tasksCompleted)
        {
            var gControl = new GControl();

            for (int i = 0; i < tasksCompleted; i++)
            {
                gControl.addTaskComplete();
            }

            Assert.That(gControl.checkSpyTaskWin());
        }
    }
}
