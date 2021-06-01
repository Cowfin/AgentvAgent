using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TimerWinTest
    {
        [Test]
        [TestCase(null)]
        [TestCase(180, false)]
        [TestCase(0, true)]
        [TestCase(-1, true)]
        [TestCase(-43, true)]
        [TestCase(1000, false)]
        public void TimerWinTestSimplePasses(int timeRemaining, bool actualOutcome)
        {
            var gControl = new GControl();
            gControl.setTimeRemaining(timeRemaining);
            Assert.That(gControl.checkTimeWin(), Is.EqualTo(actualOutcome));
        }

    }
}
