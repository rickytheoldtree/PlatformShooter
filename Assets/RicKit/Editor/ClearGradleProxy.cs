using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RicKit.EditorTools
{
    public class ClearGradleProxy : Editor
    {
        [MenuItem("RicKit/Others/Clear Gradle Proxy")]
        public static void ClearGradle()
        {
            var gradlePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            gradlePath += @"\.gradle\gradle.properties";
            var sb = new StringBuilder();
            if (System.IO.File.Exists(gradlePath))
            {
                //打开文件
                using var sr = new System.IO.StreamReader(gradlePath);
                // 删除带有代理的行
                while (sr.ReadLine() is { } line)
                {
                    if (line.StartsWith("systemProp.https.proxyHost") || line.StartsWith("systemProp.http.proxyHost")
                        || line.StartsWith("systemProp.https.proxyPort") || line.StartsWith("systemProp.http.proxyPort"))
                    {
                        Debug.Log("Deleted line: " + line);
                        continue;
                    }
                    sb.AppendLine(line);
                }
                sr.Close();
                // 保存文件
                using var sw = new System.IO.StreamWriter(gradlePath);
                sw.Write(sb.ToString());
                sw.Close();
            }
            else
            {
                Debug.Log("Gradle Proxy is already cleared.");
            }
        }
    }
}
