using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// This class has the functionality of converting stream(string)
    /// to calculations
    /// </summary>
    public class StreamConverter
    {
        public int answer;
        public string check;

        /// <summary>
        /// This function is checking if the stream has correctness
        /// </summary>
        /// <param name="msg">String to check</param>
        public void checkStream(string msg)
        {
            char x = msg.ElementAt(0);
            char y = msg.ElementAt(1);
            char z = msg.ElementAt(2);

            if (x == 13)
            {
                check = "CR";
            }
            else if (x >= 48 && x <= 57 && z >= 48 && z <= 57)
            {
                if (y == 42 || y == 43 || y == 45 || y == 47)
                {
                    check = "ok";
                }
            }
            else if (x == 109 && y == 115 && z == 103)
            {
                check = "db";
            }
            else
            {
                check = "error";
            }
        }
        /// <summary>
        /// This Function does the math
        /// </summary>
        /// <param name="msg">Stream to work with</param>
        public void Calculator(string msg)
        {
            int num1 = (int)Char.GetNumericValue(msg.ElementAt(0));
            int num2 = (int)Char.GetNumericValue(msg.ElementAt(2));
            char operation = msg.ElementAt(1);
            int ans = 0;

            if (operation == '/')
            {
                ans = num1 / num2;
            }
            else if (operation == '*')
            {
                ans = num1 * num2;
            }
            else if (operation == '+')
            {
                ans = num1 + num2;
            }
            else if (operation == '-')
            {
                ans = num1 - num2;
            }

            answer = ans;
        }

    }

}
