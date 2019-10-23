using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Reports : MonoBehaviour
    {
        public Text monitor1aText = null;
        public Text monitor2aText = null;
        public Text monitor3aText = null;
        public Text monitor4aText = null;
        public Text monitor1bText = null;
        public Text monitor2bText = null;
        public Text monitor3bText = null;
        public Text monitor4bText = null;
        public Text monitor1cText = null;
        public Text monitor2cText = null;
        public Text monitor3cText = null;
        public Text monitor4cText = null;

        // Start is called before the first frame update
        void Start()
        {
            var reportService = new ReportService();
            var reports = reportService.GetReports();

            monitor1aText.text = reports[0].Title;
            monitor2aText.text = reports[1].Title;
            monitor3aText.text = reports[2].Title;
            monitor4aText.text = reports[3].Title;
            monitor1bText.text = reports[0].SubTitle;
            monitor2bText.text = reports[1].SubTitle;
            monitor3bText.text = reports[2].SubTitle;
            monitor4bText.text = reports[3].SubTitle;
            monitor1cText.text = reports[0].Text;
            monitor2cText.text = reports[1].Text;
            monitor3cText.text = reports[2].Text;
            monitor4cText.text = reports[3].Text;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}