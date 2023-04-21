// jave.lin 2023/04/21 �� timespan ����־ ������ log hierarchy �ṹҪ�󣬼��� ���� stack Ҫ��

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TSLog
{
    // ts == time span
    public class WithTimeSpanLogData
    {
        public int idx;
        public string tag;
        public TimeSpan timeSpan; // (DateTime)start - (DateTime)end

        public string startDesc;
        public string endDesc;
        public DateTime startDateTime;
        public DateTime endDateTime;
        public string classNameStart;
        public int lineStart;
        public string classNameEnd;
        public int lineEnd;

        public static string GetCSVTitle()
        {
            return $"���, ���, ��ʱ, ��ʼ����, ��������, ��ʼʱ��, ����ʱ��, ��ʼ�ļ�, ��ʼ��, �����ļ�, ������\n";
        }

        public override string ToString()
        {
            return $"{idx}, {tag}, {timeSpan}, {startDesc}, {endDesc}, {startDateTime}, {endDateTime}, {classNameStart}, {lineStart}, {classNameEnd}, {lineEnd}";
        }
    }

    // clog == custom log
    public static void CLog(string content)
    {
        Debug.Log($"[{DateTime.Now}] {content}");
    }

    private static void GetCodeLineNum(out string fileName, out int lineNum)
    {
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, true);
        System.Diagnostics.StackFrame frame = st.GetFrame(st.FrameCount - 1);
        fileName = frame.GetFileName();
        lineNum = frame.GetFileLineNumber();
    }

    //=== DEMO START ======================

    private static void Test()
    {
        System.Threading.Thread.Sleep(2000);
        CLog("Test Func Testing with date time Log");
    }

    private static void Test2()
    {
        System.Threading.Thread.Sleep(1000);
        CLog("Test2 Func Testing with date time Log");
    }

    [MenuItem("ʵ�ù���/����/����TSLog��д��־���")]
    private static void Usage()
    {
        var tslog = new TSLog();

        tslog.Clear();

        tslog.BeginTag("Test", "��ʼTest�����������");
        Test();
        tslog.EndTag("Test", "����Test��������ͳ������");

        tslog.BeginTag("Test2", "��ʼTest2�����������");
        Test2();
        tslog.EndTag("Test2", "����Test2��������ͳ�ƺ�ʱ");

        tslog.WriteTagLogs();

        /*

        ���������

���, ���, ��ʱ, ��ʼ����, ��������, ��ʼʱ��, ����ʱ��, ��ʼ�ļ�, ��ʼ��, �����ļ�, ������
0, Test, 00:00:02.0112773, ��ʼTest�����������, ����Test��������ͳ������, 2023/4/21 15:36:10, 2023/4/21 15:36:12, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 73, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 75
1, Test2, 00:00:01.0104517, ��ʼTest2�����������, ����Test2��������ͳ�ƺ�ʱ, 2023/4/21 15:36:12, 2023/4/21 15:36:13, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 77, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 79


         * */
    }

    //=== DEMO END ======================

    // WTSL ==> With TimeSpan Log
    private int WTSLDataIDXCounter = 0;

    private List<WithTimeSpanLogData> logTag_List = new List<WithTimeSpanLogData>();

    private Dictionary<string, WithTimeSpanLogData> logTag_Dict = new Dictionary<string, WithTimeSpanLogData>();

    public void Clear()
    {
        WTSLDataIDXCounter = 0;
        logTag_List.Clear();
        logTag_Dict.Clear();
    }

    public void BeginTag(string tag, string description = null)
    {
        tag = tag.Replace(",", "_");
        if (description != null)
            description = description.Replace(",", "_");

        var alreadyBeginTag = logTag_Dict.TryGetValue(tag, out var data);
        var nowDateTime = DateTime.Now;
        if (alreadyBeginTag)
        {
            Debug.LogWarning($"[{nowDateTime}] W BeginTag : {tag}, but alreadyBeginTag at time : {data}");
        }
        else
        {
            GetCodeLineNum(out string fileName, out int lineNum);

            Debug.Log($"[{nowDateTime}] D BeginTag : {tag}");
            logTag_Dict[tag] = new WithTimeSpanLogData
            {
                idx = WTSLDataIDXCounter++,
                tag = tag,
                startDesc = description,
                startDateTime = nowDateTime,
                classNameStart = fileName,
                lineStart = lineNum,
            };
        }
    }

    public void EndTag(string tag, string description = null)
    {
        tag = tag.Replace(",", "_");
        if (description != null)
            description = description.Replace(",", "_");

        var hasBeginTag = logTag_Dict.TryGetValue(tag, out var data);
        var nowDateTime = DateTime.Now;
        if (hasBeginTag)
        {
            var ts = nowDateTime - data.startDateTime;
            Debug.Log($"[{nowDateTime}] D EndTag : {tag}, Timespan : {ts}");
            logTag_Dict.Remove(tag);

            GetCodeLineNum(out string fileName, out int lineNum);

            data.endDesc = description;
            data.endDateTime = nowDateTime;
            data.classNameEnd = fileName;
            data.lineEnd = lineNum;
            data.timeSpan = ts;

            logTag_List.Add(data);
        }
        else
        {
            Debug.LogWarning($"[{nowDateTime}] W EndTag : {tag}, have no Timespan");
        }
    }

    public bool WriteTagLogs()
    {
        var ret = false;
        var error = string.Empty;
        try
        {
            using (var streamWriter = new StreamWriter("BuildingTagLogs.csv", false))
            {
                // ����
                streamWriter.Write(WithTimeSpanLogData.GetCSVTitle());

                // ����

                var list = logTag_List;
                list.Sort((a, b) => a.idx - b.idx);

                foreach (var data in list)
                {
                    streamWriter.Write($"{data}\n");
                }

                streamWriter.Close();
            }
            Debug.Log("WriteTagLogs : BuildingTagLogs.csv Successfully!");

            ret = true;
            error = string.Empty;
        }
        catch (System.Exception er)
        {
            ret = false;
            error = er.ToString();
        }
        finally
        {
            if (!ret)
            {
                Debug.LogError($"WriteTagLogs : BuildingTagLogs.csv Failured, error : {error}");
            }
        }
        return ret;
    }
}
