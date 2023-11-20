using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

    public class Logger : MonoBehaviour
    {	
	    private static string output					= "";

        private static TextMeshProUGUI logText;

        void Awake()
        {
            logText = GetComponent<TextMeshProUGUI>();
        }	

	    public static void Add(string text)
        {
            if(logText != null)
            {
                if (output.Length > 1000)
                {
                    output = "";
                }

                output = text+"\n"+output;
                logText.text = output;
            }

            Debug.Log(text);
	    }

        public static void Clear()
        {
            if (logText != null)
            {
                output = "";
                logText.text = output;
            }
        }

        public static void Set(string text)
        {
            if (logText != null)
            {
                output = text;
                logText.text = output;
            }

            Debug.Log(text);
        }

        public static string GetOutput()
        {
            return output;
        }
    }
