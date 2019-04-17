using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App2
{
	public class BoardObject
    { 
        // hold the top and bottom row/col value for 
        // a snake or a ladder
        public int TopX { get; set; }
        public int TopY { get; set; }
        public int BottomX { get; set; }
        public int BottomY { get; set; }
    }
}
