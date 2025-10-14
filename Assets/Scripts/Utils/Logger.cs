using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class Logger : MonoBehaviour
    {
        public static Logger Instance => _instance ??= FindObjectOfType<Logger>();
        private static Logger _instance;

        [Header("Debug Logs enable")] public bool PhotonStandartMessagesLog = true;
        public bool PhotonNetworkManagerLog = true;

        public void DebugMessageShow(ELogSource logSource, string title, ELogColor eLogColor = null,
            string message = "")
        {
#if PLATFORM_STANDALONE && !UNITY_EDITOR
            try
            {
                string folderPath = Application.persistentDataPath + "/Logs";
            
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string pathToLog = folderPath + "\\" + DateTime.Now.ToString("dd_MM_yyyy") + "_log.txt";
            
                foreach (string file in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
                {
                    if (file != pathToLog)
                        System.IO.File.Delete(file);
                }
            
                string text = "";
            
                if (message == "")
                    text = "[" + logSource + "] " + title;
                else
                    text = "[" + logSource + "] " + title + ": " + message;

                text += "\n\n\n\n";

                text = DateTime.Now + " |\n" + text;

                File.AppendAllText(pathToLog, text);
            }
            catch (Exception Ex)
            {
                
            }
#endif

            try
            {
#if UNITY_EDITOR || DEV || DEVELOPMENT_BUILD
                if ((bool)GetType().GetField(logSource + "Log").GetValue(this) == false)
                    return;
#endif
            }
            catch (Exception e)
            {
                throw new Exception("You didn't add a variable '" + logSource + "Log' for " + logSource +
                                    " in TestUtilsHandler");
            }

#if UNITY_EDITOR || DEV || DEVELOPMENT_BUILD
            if (eLogColor == null)
                eLogColor = ELogColor.Green;

            string offset = "\n \n";

#if UNITY_EDITOR
            if (message != "")
                message = ": </color>" + message;
            else
                message = "</color>";

            Debug.Log("<color=" + ELogColor.White + ">" + "[" + logSource + "] " + "</color>" + "<color=" + eLogColor +
                      ">" + title + message + offset);
#else
            if (message != "")
                message = ": " + message;

            Debug.Log(title + message + offset);
#endif
#endif
        }

        // When adding new source, add new bool variable on top in this script
        public enum ELogSource
        {
            PhotonStandartMessages,
            PhotonNetworkManager
        }

        public class ELogColor
        {
            private ELogColor(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }

            public static ELogColor Red
            {
                get { return new ELogColor("red"); }
            }

            public static ELogColor Green
            {
                get { return new ELogColor("#00ff00"); }
            }

            public static ELogColor Blue
            {
                get { return new ELogColor("#0081ff"); }
            }

            public static ELogColor Magenta
            {
                get { return new ELogColor("#fc0fc0"); }
            }

            public static ELogColor Swamp
            {
                get { return new ELogColor("#7c8a58"); }
            }

            public static ELogColor Yellow
            {
                get { return new ELogColor("#ddd602"); }
            }

            public static ELogColor Pink
            {
                get { return new ELogColor("#ff8fa2"); }
            }

            public static ELogColor White
            {
                get { return new ELogColor("#ffffff"); }
            }

            public static ELogColor Cyan
            {
                get { return new ELogColor("#21cfcc"); }
            }

            public static ELogColor Orange
            {
                get { return new ELogColor("#ff6a00"); }
            }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}