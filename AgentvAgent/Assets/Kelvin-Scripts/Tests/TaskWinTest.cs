using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TaskWinTest
    {
        // A Test behaves as an ordinary method
        [Test]
        [TestCase(null, null, null)]
        [TestCase(6, 3, false)]
        [TestCase(6, 6, true)]
        [TestCase(6, 7, true)]
        public void TaskWinTestSimplePasses(int tasksToCompleteNumber, int taskCompletedNumber, bool expectedOutcome)
        {
            var taskWin = new TaskWin();

            bool spyWin = taskWin.checkSpyTaskWin(tasksToCompleteNumber, taskCompletedNumber);
            // Use the Assert class to test conditions
            Assert.That(spyWin, Is.EqualTo(expectedOutcome));
        }
    }
}
