using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Pages
{
    public class SortingVisualizerBase:ComponentBase
    {
        public const int NUMBER_OF_ARRAY_BARS = 260;
        public const int ANIMATION_SPEED_MS = 1;
        public const string PRIMARY_COLOR = "turquoise";
        public const string SECONDARY_COLOR = "red";
        public const string SORTING_COLOR = "blue";
        public List<int[]> animations { get; set; }
        public int[] array { get; set; }
        public string[] color { get; set; }
        System.Timers.Timer t;
        protected override Task OnInitializedAsync()
        {
            resetArray();
            return base.OnInitializedAsync();
        }
        public void resetArray()
        {
            color = new string[NUMBER_OF_ARRAY_BARS];
            array = new int[NUMBER_OF_ARRAY_BARS];
            for (int i = 0; i < NUMBER_OF_ARRAY_BARS; i++)
            {
                array[i]= randomIntFromInterval(5, 545);
                color[i] = PRIMARY_COLOR;
            }
        }
        public int randomIntFromInterval(int start, int end)
        {
            Random rand = new Random();
            int number = rand.Next(start, end + 1);
            return number;
        }
        
        public async void mergeSort()
        {
            int[] cloneArray = (int[])array.Clone();
            getMergeSortAnimations(cloneArray);
            int i = 0;
            t = new System.Timers.Timer();
            t.Elapsed += async (s, e) =>
            {
                if (i < animations.Count)
                {
                    bool isColorChange = i % 3 != 2;
                    if (isColorChange)
                    {
                        int[] temp = animations[i];
                        color[temp[0]] = i % 3 == 0 ? SECONDARY_COLOR : (temp[3]== 1 && (temp[2] >= temp[0])) ? SORTING_COLOR : PRIMARY_COLOR;
                        color[temp[1]] = i % 3 == 0 ? SECONDARY_COLOR : (temp[3]== 1 && (temp[2] >= temp[1])) ? SORTING_COLOR : PRIMARY_COLOR;
                        color[temp[2]] = i % 3 == 0 ? SORTING_COLOR : (temp[3]==0)?PRIMARY_COLOR: SORTING_COLOR;
                    }
                    else
                    {
                        int[] temp = animations[i];
                        array[temp[0]] = temp[1];
                    }
                }
                i++;
                await InvokeAsync(StateHasChanged);
            };
            t.Interval = 0.1;
            t.Start();

            
        }
        public List<int[]> getMergeSortAnimations(int[] array)
        {
            animations = new List<int[]>();
            if (array.Length <= 1) 
                return animations;
            int[] auxiliaryArray = (int[])array.Clone();
            mergeSortHelper(array, 0, array.Length - 1, auxiliaryArray, animations);
            return animations;
        }
        public void mergeSortHelper(int[] mainArray, int startIdx, int endIdx, int[] auxiliaryArray, List<int[]> animations)
        {
            if (startIdx == endIdx) return;
            int middleIdx = (startIdx + endIdx) / 2;
            mergeSortHelper(auxiliaryArray, startIdx, middleIdx, mainArray, animations);
            mergeSortHelper(auxiliaryArray, middleIdx + 1, endIdx, mainArray, animations);
            doMerge(mainArray, startIdx, middleIdx, endIdx, auxiliaryArray, animations);
        }
        public void doMerge(int[] mainArray, int startIdx, int middleIdx, int endIdx, int[] auxiliaryArray, List<int[]> animations)
        {
            int k = startIdx;
            int i = startIdx;
            int j = middleIdx + 1;
            int f = 0;
            if (startIdx == 0 && endIdx == mainArray.Length - 1)
                f = 1;
            while (i <= middleIdx && j <= endIdx)
            {
                animations.Add(new int[] { i, j, k, f });
                animations.Add(new int[] { i, j, k, f });
                if (auxiliaryArray[i] <= auxiliaryArray[j])
                {
                    animations.Add(new int[] { k, auxiliaryArray[i] });
                    mainArray[k++] = auxiliaryArray[i++];
                }
                else
                {
                    animations.Add(new int[] { k, auxiliaryArray[j] });
                    mainArray[k++] = auxiliaryArray[j++];
                }
            }
            while (i <= middleIdx)
            {
                animations.Add(new int[] { i, i, k, f });
                animations.Add(new int[] { i, i, k, f });
                animations.Add(new int[] { k, auxiliaryArray[i] });
                mainArray[k++] = auxiliaryArray[i++];
            }
            while (j <= endIdx)
            {
                animations.Add(new int[] { j, j, k, f });
                animations.Add(new int[] { j, j, k, f });
                animations.Add(new int[] { k, auxiliaryArray[j] });
                mainArray[k++] = auxiliaryArray[j++];
            }
        }
    }
}
