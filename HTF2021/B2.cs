﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTF2021
{
    internal static class B2
    {
        private static readonly string testUrl = "api/path/2/medium/Sample";
        private static readonly string productionUrl = "api/path/2/medium/Puzzle";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
            //var testData = 2.229422381483193E+129;
            var testData = 821521568221;
            Console.WriteLine($"Test endpoint data: {testData}");
            var testSolution = RepeatingNumbers(testData.ToString());
            Console.WriteLine(testSolution);
        }

        internal static async Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
            var testData = await clientInstance.client.GetFromJsonAsync<double>(testUrl);
            Console.WriteLine($"Test endpoint data: {testData}");
            //var testSolution = MostRepeatedSubstring(testData.ToString("F1000000000").Split(".")[0]);
            //Console.WriteLine(testSolution);
            //var testPostResponse = await clientInstance.client.PostAsJsonAsync<string>(testUrl, testSolution);
            //var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            //Console.WriteLine(testPostResponseValue);
        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
            var testData = await clientInstance.client.GetStringAsync(productionUrl);
            Console.WriteLine($"Production endpoint data: {testData} \n");
            var testSolution = RepeatingNumbers(testData);
            Console.WriteLine(testSolution);
            var testPostResponse = await clientInstance.client.PostAsJsonAsync(productionUrl, testSolution);
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Production endpoint response: {testPostResponseValue}");
        }

        private static int RepeatingNumbers(string data)
        {
            Dictionary<string, int> patterns = new();
            int repeats;
            string possible;
            for (var i = 0; i < data.Length; i++)
            for (var j = 2; j + i < data.Length; j++)
            {
                possible = data.Substring(i, j);
                repeats = 0;
                for (var k = 0; k <= data.Length - possible.Length; k++)
                    if (data.Substring(k, possible.Length) == possible)
                        repeats++;
                if (repeats > 1 && !patterns.ContainsKey(possible))
                    patterns.Add(possible, repeats);
            }

            string pattern = "";
            var reps = 0;
            foreach (var i in patterns)
                if (i.Value > reps)
                {
                    reps = i.Value;
                    pattern = i.Key;
                }

            return int.Parse(patterns.Count + pattern);
        }
    }
}