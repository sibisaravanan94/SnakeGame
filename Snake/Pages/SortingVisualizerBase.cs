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
        public int[] array { get; set; }
        protected override Task OnInitializedAsync()
        {
            resetArray();
            return base.OnInitializedAsync();
        }
        public void resetArray()
        {
            array = new int[NUMBER_OF_ARRAY_BARS];
            for (int i = 0; i < NUMBER_OF_ARRAY_BARS; i++)
            {
                array[i]= randomIntFromInterval(5, 545);
            }
        }
        public int randomIntFromInterval(int start, int end)
        {
            Random rand = new Random();
            int number = rand.Next(start, end + 1);
            return number;
        }
    }
}
