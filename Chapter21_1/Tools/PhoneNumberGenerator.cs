using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter21_1.Tools
{
    public class PhoneNumberGenerator
    {
        private static readonly Random _random = new Random();

        // 中国手机号前三位（已启用的号段）
        private static readonly string[] _prefixes =
        {
        "130", "131", "132", "133", "134", "135", "136", "137", "138", "139",
        "145", "146", "147", "148", "149",
        "150", "151", "152", "153", "155", "156", "157", "158", "159",
        "162", "165", "166", "167",
        "170", "171", "172", "173", "174", "175", "176", "177", "178",
        "180", "181", "182", "183", "184", "185", "186", "187", "188", "189",
        "190", "191", "192", "193", "195", "196", "197", "198", "199"
    };

        /// <summary>
        /// 生成一个随机手机号
        /// </summary>
        public static string Generate()
        {
            string prefix = _prefixes[_random.Next(_prefixes.Length)];
            string suffix = _random.Next(10000000, 99999999).ToString();
            return prefix + suffix;
        }
        /// <summary>
        /// 生成指定数量的不重复手机号
        /// </summary>
        public static List<string> Generate(int count)
        {
            var numbers = new HashSet<string>();
            while (numbers.Count < count)
            {
                numbers.Add(Generate());
            }
            return new List<string>(numbers);
        }
    }
}
