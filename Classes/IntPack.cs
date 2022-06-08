using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECL.Classes
{
    public class IntPack
	{
		public IntPack()
        {
            
        }

		public uint Value
		{
			get;
			private set;
		}

		public void setBit(int start, bool nvalue)
		{
			setBits(start, start, nvalue ? 1u : 0u);
		}

		public void setBits(int start, int end, uint nvalue)
		{
			int bitsCount = end - start + 1;
			UInt32 valueMask = (((UInt32)1 << bitsCount) - 1) << start;
			Value = ((Value & ~valueMask) | ((nvalue << start) & valueMask));
		}
	}
}
