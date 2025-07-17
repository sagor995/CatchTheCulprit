using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditModeTestScripts
    {

        public Transform transform;
        public GameObject obj;
        /*
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        
        */

        // A Test behaves as an ordinary method
        [Test]
        public void CheckCardValuesAreSame()
        {
            var cards = new BasicTestMethod(new string[] { "Master", "Police", "Robber", "Theif" });

            string[] elements = { "Master", "Police", "Robber", "Theif" };

            // Use the Assert class to test conditions
            Assert.AreEqual(true, cards.AreEqual(cards.getCardValues(), elements));
        }

        [Test]
        public void CheckIfResourcesContainsImage()
        {
            var img = Resources.LoadAll<Sprite>("avatar");
            Assert.Greater(img.Length, 0);
        }

        [Test]
        public void CheckingGameConditions()
        {
            var cond = new BasicTestMethod2();
            int input = 40;
            int mode = 1;
            //Assert.AreEqual(1, );
            Assert.That(1, Is.EqualTo(cond.GameModesCondition(input, mode)));
        }
    }
}
