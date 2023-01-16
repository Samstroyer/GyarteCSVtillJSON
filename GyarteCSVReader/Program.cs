using System.Text.Json;
using System.IO;

Start();

void Start()
{
    List<Result> results = new();

    foreach (string fileName in Directory.GetFiles("./ReaderMap"))
    {
        List<string> rows = File.ReadAllLines(fileName).ToList();

        List<int> medianCalculator = new();

        rows.RemoveAt(0);

        string lastTriggerFrame = "0";
        int total = 0;
        int usedRows = 0;
        int highest = 0;
        int lowest = Int32.MaxValue;

        foreach (string values in rows)
        {
            string[] frameAndClosestTriggerFrame = values.Split(",");

            if (frameAndClosestTriggerFrame[1] != lastTriggerFrame)
            {
                string key = frameAndClosestTriggerFrame[0];
                int frame = int.Parse(frameAndClosestTriggerFrame[1]);
                int triggerFrame = int.Parse(frameAndClosestTriggerFrame[2]);


                lastTriggerFrame = triggerFrame.ToString();
                total += frame - triggerFrame;
                usedRows++;

                int temp = frame - triggerFrame;
                medianCalculator.Add(temp);
                if (temp > highest) highest = temp;
                if (temp < lowest) lowest = temp;
            }
        }

        int averageFrames = total / usedRows;

        results.Add(new()
        {
            AverageFrames = averageFrames,
            Lowest = lowest,
            Highest = highest,
            AverageTimeMS = averageFrames * 33,
            FileName = fileName,
            Median = CalculateMedian(medianCalculator)
        });
    }

    var options = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    string json = JsonSerializer.Serialize<List<Result>>(results, options);

    File.AppendAllText("./Json.json", json);
}

float CalculateMedian(List<int> medianCalculator)
{
    float ret = 0;
    int[] array = medianCalculator.ToArray();

    if (array.Length % 2 == 0)
    {
        int end = array.Length / 2;
        int temp1 = end - 1;
        int temp2 = end;

        return (array[temp1] + array[temp2]) / 2;
    }
    else
    {
        double start = array.Length / 2;
        start = Math.Floor(start);

        return array[(int)start];
    }
}

Console.ReadLine();
