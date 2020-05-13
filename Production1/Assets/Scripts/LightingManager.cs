using System;
using System.Collections;
using UnityEngine;

namespace Com.MachineApps.PrepareAndDeploy
{

    public class LightingManager : MonoBehaviour
    {
        public static LightingManager instance;

        const float stepsPerSecond = 2f;

        void Awake()
        {
            if (instance && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        [SerializeField] private GameObject boxLight1;
        [SerializeField] private GameObject boxLight2;
        [SerializeField] private GameObject boxLight3;
        [SerializeField] private GameObject boxLight4;
        [SerializeField] private GameObject overheadSpot;
        [SerializeField] private GameObject computerSpot;
        [SerializeField] private GameObject scoreboardSpot;

        #region Public methods

        public void BoxLightsFade(bool directionUp, int timeSpan, float low, float high)
        {
            Debug.Log("BoxLightsFade");
            StartCoroutine(directionUp 
                ? BoxLightsUp(timeSpan, low, high) 
                : BoxLightsDown(timeSpan, low, high));
        }

        public void OverheadLightsFade(bool directionUp, int timeSpan, float low, float high)
        {
            Debug.Log("BoxLightsFade");
            StartCoroutine(directionUp
                ? OverheadLightsUp(timeSpan, low, high)
                : OverheadLightsDown(timeSpan, low, high));
        }

        public void ScoreboardLightsFade(bool directionUp, int timeSpan, float low, float high)
        {
            Debug.Log("BoxLightsFade");
            StartCoroutine(directionUp
                ? ScoreboardLightsUp(timeSpan, low, high)
                : ScoreboardLightsDown(timeSpan, low, high));
        }

        public void ComputerLightsFade(bool directionUp, int timeSpan, float low, float high)
        {
            Debug.Log("BoxLightsFade");
            StartCoroutine(directionUp
                ? ComputerLightsUp(timeSpan, low, high)
                : ComputerLightsDown(timeSpan, low, high));
        }
        

        #endregion


        #region Private methods

        private IEnumerator BoxLightsUp(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = low;
            while (i <= high)
            {
                //Debug.Log(i);
                boxLight1.GetComponent<Light>().intensity = i;
                boxLight2.GetComponent<Light>().intensity = i;
                boxLight3.GetComponent<Light>().intensity = i;
                boxLight4.GetComponent<Light>().intensity = i;
                i += 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator BoxLightsDown(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = high;
            while (i >= low)
            {
                //Debug.Log(i);
                boxLight1.GetComponent<Light>().intensity = i;
                boxLight2.GetComponent<Light>().intensity = i;
                boxLight3.GetComponent<Light>().intensity = i;
                boxLight4.GetComponent<Light>().intensity = i;
                i -= 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator OverheadLightsUp(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = low;
            while (i <= high)
            {
                //Debug.Log(i);
                overheadSpot.GetComponent<Light>().intensity = i;
                i += 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator OverheadLightsDown(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = high;
            while (i >= low)
            {
                //Debug.Log(i);
                overheadSpot.GetComponent<Light>().intensity = i;
                i -= 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator ScoreboardLightsUp(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = low;
            while (i <= high)
            {
                //Debug.Log(i);
                scoreboardSpot.GetComponent<Light>().intensity = i;
                i += 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator ScoreboardLightsDown(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = high;
            while (i >= low)
            {
                //Debug.Log(i);
                scoreboardSpot.GetComponent<Light>().intensity = i;
                i -= 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator ComputerLightsUp(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = low;
            while (i <= high)
            {
                //Debug.Log(i);
                computerSpot.GetComponent<Light>().intensity = i;
                i += 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        private IEnumerator ComputerLightsDown(int timeSpan, float low, float high)
        {
            var x = timeSpan * 2 / Math.Abs(high - low);

            var i = high;
            while (i >= low)
            {
                //Debug.Log(i);
                computerSpot.GetComponent<Light>().intensity = i;
                i -= 1f / x;

                yield return new WaitForSeconds(1 / stepsPerSecond);
            }
        }

        #endregion


    }

}