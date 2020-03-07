using System;

namespace VotingSystem.Util
{
    /// <summary>
    /// 生成随机密码.
    /// </summary>
    public class GetRandomString
    {
        private static char[] constant =
        {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        };

        /// <summary>
        /// 生成随机密码.
        /// </summary>
        /// <returns>.</returns>
        public static string GenerateRandomNumber()
        {
            int length = 5;
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }

            return newRandom.ToString();
        }
    }
}