using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Classes
{
    public class LinkedList
    {
        public LinkedListNode head { get; set; }
        public LinkedListNode tail { get; set; }
        public LinkedList(Node snakeCell)
        {
            LinkedListNode node = new LinkedListNode(snakeCell);
            this.head = node;
            this.tail = node;
        }
    }
}
