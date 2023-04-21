# TSLog
https://blog.csdn.net/linjf520/article/details/130285640

```csharp
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

    [MenuItem("实用工具/测试/测试TSLog并写日志结果")]
    private static void Usage()
    {
        var tslog = new TSLog();

        tslog.Clear();

        tslog.BeginTag("Test", "开始Test函数，并标记");
        Test();
        tslog.EndTag("Test", "结束Test函数，并统计消耗");

        tslog.BeginTag("Test2", "开始Test2函数，并标记");
        Test2();
        tslog.EndTag("Test2", "结束Test2函数，并统计耗时");

        tslog.WriteTagLogs();

        /*

        测试输出：

编号, 标记, 耗时, 开始描述, 结束描述, 开始时间, 结束时间, 开始文件, 开始行, 结束文件, 结束行
0, Test, 00:00:02.0112773, 开始Test函数，并标记, 结束Test函数，并统计消耗, 2023/4/21 15:36:10, 2023/4/21 15:36:12, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 73, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 75
1, Test2, 00:00:01.0104517, 开始Test2函数，并标记, 结束Test2函数，并统计耗时, 2023/4/21 15:36:12, 2023/4/21 15:36:13, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 77, C:\SuoguoProject\Client\Assets\Editor\TSLog.cs, 79


         * */
    }

    //=== DEMO END ======================
```

# OpenCSV File
![image](https://user-images.githubusercontent.com/25763884/233578053-7c564a57-96ef-4e5c-9d6f-b19b73eb220d.png)

