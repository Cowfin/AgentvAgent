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
        [TestCase(-1, false)]
        [TestCase(100, true)]
        [TestCase(4, false)]
        [TestCase(6, true)]
        [TestCase(10000, true)]
        [TestCase(0, false)]
        [TestCase(2, false)]
        public void TaskWinTestSimplePasses(int tasksCompleted, bool actualOutcome)
        {
            var gControl = new GControl();

            for (int i = 0; i < tasksCompleted; i++)
            {
                gControl.addTaskComplete();
            }

            Assert.That(gControl.checkSpyTaskWin(), Is.EqualTo(actualOutcome));
        }
    }
}