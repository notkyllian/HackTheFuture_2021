﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTF2021
{
    internal class A2Json
    {
        public int Start { get; set; }
        public int Destination { get; set; }

        public override string ToString()
        {
            return $"Start: {Start}, Destination: {Destination}";
        }
    }

    internal static class A2
    {
        private static readonly string testUrl = "api/path/1/medium/Sample";
        private static readonly string productionUrl = "api/path/1/medium/Puzzle";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
            Console.WriteLine($"Simple calculation algorithm: {string.Join("; ", simpleElevatorAlgorithm(0, 9))} \n");
            Console.WriteLine($"Future calculation algorithm: {string.Join("; ", futureElevatorAlgorithm(0, 9))} \n");
        }

        internal static async Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
            var testData = await clientInstance.client.GetFromJsonAsync<A2Json>(testUrl);
            Console.WriteLine($"Test endpoint data: {testData}");

            var testSimpleSolution = simpleElevatorAlgorithm(testData.Start, testData.Destination);
            var testFutureSolution = futureElevatorAlgorithm(testData.Start, testData.Destination);
            Console.WriteLine($"Test simple solution: {string.Join(", ", testSimpleSolution.ToArray())}");
            Console.WriteLine($"Test future solution: {string.Join(", ", testFutureSolution.ToArray())}");

            var testPostResponse =
                await clientInstance.client.PostAsJsonAsync<int[]>(testUrl, testSimpleSolution.ToArray());
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");

            var testData = await clientInstance.client.GetFromJsonAsync<A2Json>(productionUrl);
            Console.WriteLine($"Production endpoint data: {testData}");

            var testSimpleSolution = simpleElevatorAlgorithm(testData.Start, testData.Destination);
            Console.WriteLine(
                $"Production simple solution {testSimpleSolution.Count}: {string.Join(", ", testSimpleSolution.ToArray())}");

            var testPostResponse =
                await clientInstance.client.PostAsJsonAsync<int[]>(productionUrl, testSimpleSolution.ToArray());
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Production endpoint response: {testPostResponseValue}");
        }


        internal static List<int> futureElevatorAlgorithm(int beginFloor, int endFloor)
        {
            var floors = new List<int>();
            var currentFloor = beginFloor;
            var stepCount = 1;
            floors.Add(currentFloor);

            while (currentFloor != endFloor)
            {
                if (currentFloor + stepCount == endFloor)
                    currentFloor += stepCount;
                else if (currentFloor + stepCount + stepCount + 1 > endFloor)
                    currentFloor -= stepCount;
                else
                    currentFloor += stepCount;

                ++stepCount;
                floors.Add(currentFloor);
            }

            return floors;
        }

        internal static List<int> simpleElevatorAlgorithm(int beginFloor, int endFloor)
        {
            var floors = new List<int>();
            var currentFloor = beginFloor;
            var stepCount = 1;
            floors.Add(currentFloor);
            while (currentFloor != endFloor)
            {
                if (currentFloor + stepCount > endFloor)
                    currentFloor -= stepCount;
                else
                    currentFloor += stepCount;

                ++stepCount;
                floors.Add(currentFloor);
            }

            return floors;
        }
    }
}