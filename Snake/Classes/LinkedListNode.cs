using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Classes
{
    public class LinkedListNode
    {
        public int value { get; set; }
        public LinkedListNode next { get; set; }
        public LinkedListNode(int snakeCell)
        {
            this.value = snakeCell;
            this.next = null;
        }
    }
}
