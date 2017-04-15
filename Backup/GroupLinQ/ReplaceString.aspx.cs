using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class ReplaceString : System.Web.UI.Page
    {
        #region Fields (2)

        //readonly string[] _replacementChars = new[] { "ä", "à", "á", "â", "ã", "ö", "ò", "ó", "ô", "õ", "ë", "è", "é", "ê", "ï", "ì", "í", "î", "ü", "ù", "ú", "û", "ç", "ñ" };
        readonly Dictionary<string, string> _replacementMappings = new Dictionary<string, string>
                                                               {
                                                                    { "ä", "a"},{ "à", "a"},{ "á", "a"},{ "â", "a"},{ "ã", "a"},
                                                                    { "ö", "o"},{ "ò", "o"},{ "ó", "o"},{ "ô", "o"},{ "õ", "o"},
                                                                    { "ë", "e"},{ "è", "e"},{ "é", "e"},{ "ê", "e"},
                                                                    { "ï", "i"},{ "ì", "i"},{ "í", "i"},{ "î", "i"},
                                                                    { "ü", "u"},{ "ù", "u"},{ "ú", "u"},{ "û", "u"},
                                                                    { "ç", "c"},
                                                                    { "ñ", "n"}
                                                               };
        const string Pattern = @"[äàáâãöòóôõëèéêïìíîüùúûçñ]";

        #endregion Fields

        #region Methods (4)

        // Protected Methods (2) 

        protected void Button1_Click(object sender, EventArgs e)
        {
            const int count = 10000;
            var inputStr = tbTest.Text;

            var sw1 = new Stopwatch();
            sw1.Start();
            var retVal1 = string.Empty;
            for (var i = 0; i < count; i++)
            {
                retVal1 = NormalReplace(inputStr);
            }

            sw1.Stop();
            var time1 = sw1.ElapsedMilliseconds;

            var sw2 = new Stopwatch();
            sw2.Start();
            var retVal2 = string.Empty;
            for (var i = 0; i < count; i++)
            {
                retVal2 = TestReplace(inputStr);
            }
            sw2.Stop();
            var time2 = sw2.ElapsedMilliseconds;

            var sw3 = new Stopwatch();
            sw3.Start();
            var retVal3 = string.Empty;
            for (var i = 0; i < count; i++)
            {
                retVal3 = TestReplace(inputStr);
            }
            sw3.Stop();
            var time3 = sw3.ElapsedMilliseconds;

            lbTest.Text = string.Format("Time1: {0}{1}Time2: {2}{1}Time3: {3} Dung hay sai:{4}", time1, Environment.NewLine, time2, time3, retVal1.Equals(retVal2));
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Private Methods (2) 
        private string NormalReplace(string inputStr)
        {
            return inputStr
                   .Replace("ä", "a")
                   .Replace("à", "a")
                   .Replace("á", "a")
                   .Replace("â", "a")
                   .Replace("ã", "a")
                   .Replace("ö", "o")
                   .Replace("ò", "o")
                   .Replace("ó", "o")
                   .Replace("ô", "o")
                   .Replace("õ", "o")
                   .Replace("ü", "u")
                   .Replace("ù", "u")
                   .Replace("ú", "u")
                   .Replace("û", "u")
                   .Replace("ë", "e")
                   .Replace("è", "e")
                   .Replace("é", "e")
                   .Replace("ê", "e")
                   .Replace("ï", "i")
                   .Replace("ì", "i")
                   .Replace("í", "i")
                   .Replace("î", "i")
                   .Replace("ç", "c")
                   .Replace("ñ", "n");
        }

        private string TestReplace(string inputStr)
        {
            var regex = new Regex(Pattern);
            var matchCollection = regex.Matches(inputStr)
                                       .Cast<Match>()
                                       .Select(t => t.Value)
                                       .Distinct();

            return matchCollection.Aggregate(inputStr, (current, match) => Regex.Replace(current, match, _replacementMappings[match]));
        }

        #endregion Methods
    }
}