using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HunterAbilityTest
    {
        // A Test behaves as an ordinary method'
        [Test]
        [TestCase(null, null,null,null, null,null,null, false)]
        [TestCase(5, 0,0,0, 10,0,10, false)]
        [TestCase(5, 0,0,0, 2,0,2, true)]
        [TestCase(5, 0,0,0, -10,0,-10, false)]
        [TestCase(5, 0,0,0, -2,0,-2, true)]
        [TestCase(11, 0,0,0, 10,0,10, true)]
        [UnityTest]
        public void HunterAbilityDistanceCheck(float abilityRange, float hunterPosX, float hunterPosY, float hunterPosZ, float spyPosX, float spyPosY, float spyPosZ, bool expectedOutcome)
        {
            Vector3 hunterPos = new Vector3(hunterPosX, hunterPosY, hunterPosZ);
            Vector3 spyPos = new Vector3(spyPosX, spyPosY, spyPosZ);
            
            var hunterAbility = new HunterAbility();

            Assert.That(hunterAbility, Is.EqualTo(expectedOutcome));
        }
    }
}
