using System;

namespace MerchantTribe.Commerce.Utilities
{	
	public class TextUtilities
	{

		private const string list = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const char zero = '0';
		public static string GetStringRepresentation(int number)
		{
			int tmp = 0;
			int num = System.Math.Abs(number);
			string val = "";
			do {
				//gets number for new base
				tmp = num % 26;
				//gets digit in new base
				switch (tmp) {
					case 0:
						val = val.Insert(0, zero.ToString());
                        break;
					default:
						val = val.Insert(0, list.Substring(tmp - 1,1));
                        break;
				}
				//retrieves next number
				num = num / 26;
			}
			while (num != 0);
			return val;
		}
       
	}
}
