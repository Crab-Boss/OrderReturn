using System;
using System.Text;

namespace OrderReturn
{
    public static class EncodingForBase64
    {
        /// <summary>
        /// 编码成base64
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string EncodeBase64(this Encoding encoding, string txt)
        {
            if (txt == null)
            {
                return txt;
            }
            var bytes = encoding.GetBytes(txt);
            var s = Convert.ToBase64String(bytes);

            return s;
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="encodedTxt"></param>
        /// <returns></returns>
        public static string DecodeBase64(this Encoding encoding, string encodedTxt)
        {
            if (encodedTxt == null)
            {
                return encodedTxt;
            }

            var bytes = Convert.FromBase64String(encodedTxt);
            var s = encoding.GetString(bytes);

            return s;
        }
    }
}
