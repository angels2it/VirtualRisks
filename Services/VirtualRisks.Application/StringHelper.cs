// Decompiled with JetBrains decompiler
// Type: CastleGo.Application.StringHelper
// Assembly: CastleGo.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 157A9D10-4624-400D-A7F3-8771FF84E829
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Application.dll

using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CastleGo.Application
{
  public class StringHelper
  {
    private static readonly Regex _tags_ = new Regex("<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);
    private static readonly Regex _notOkCharacter_ = new Regex("[^\\w;&#@.:/\\\\?=|%!() -]", RegexOptions.Compiled);
    private static readonly string[] removedTag = new string[1]{ "img" };
    private static readonly string[] usedTag = new string[4]{ "span", "strong", "b", "i" };
    private static readonly string[] removedTagStartWith = new string[6]{ "category", "dare", "club", "user", "search", "friend" };
    private static readonly string[] trustedClass = new string[3]{ "commentimg", "commentemotion", "icon-*" };

    public static string GetHash(string input)
    {
      return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input)));
    }

    public static T ParseEnum<T>(string value)
    {
      return (T) System.Enum.Parse(typeof (T), value, true);
    }

    public static string HtmlToPlainText(string html)
    {
      Regex regex1 = new Regex("<(br|BR)\\s{0,1}\\/{0,1}>", RegexOptions.Multiline);
      Regex regex2 = new Regex("<[^>]*(>|$)", RegexOptions.Multiline);
      string input1 = new Regex("(>|$)(\\W|\\n|\\r)+<", RegexOptions.Multiline).Replace(WebUtility.HtmlDecode(html), "><");
      string input2 = regex1.Replace(input1, Environment.NewLine).Replace("\\n\\n", " ").Replace("\n\n", " ");
      return new Regex("[ ]{2,}", RegexOptions.None).Replace(regex2.Replace(input2, " "), " ").Trim();
    }

    private static string RemoveTag(string html, string startTag, string endTag)
    {
      bool flag;
      do
      {
        flag = false;
        int startIndex = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
        if (startIndex >= 0)
        {
          int num = html.IndexOf(endTag, startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
          if (num > startIndex)
          {
            html = html.Remove(startIndex, num - startIndex + endTag.Length);
            flag = true;
          }
        }
      }
      while (flag);
      return html;
    }

    public static string UnFormatTitle(string title)
    {
      title = title.TrimEnd('-');
      title = title.TrimStart('-');
      title = title.Replace("-", " ");
      return title;
    }

    public static string FormatTitle(string title)
    {
      title = title.TrimEnd('-');
      title = title.TrimStart('-');
      title = Regex.Replace(title, " {2,}", " ");
      return title;
    }

    private static string SingleSpacedTrim(string inString)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char ch in inString)
      {
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            if (!flag)
            {
              flag = true;
              stringBuilder.Append(' ');
              break;
            }
            break;
          default:
            flag = false;
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.ToString().Trim();
    }

    public static string FriendlyName(string title)
    {
      title = title.ToLower();
      title = Regex.Replace(title, "&\\w+;", "");
      title = Regex.Replace(title, "[^a-z0-9\\-\\s]", "");
      title = title.Replace(' ', '-');
      title = Regex.Replace(title, "-{2,}", "-");
      title = title.TrimStart('-');
      if (title.Length > 80)
        title = title.Substring(0, 79);
      title = title.TrimEnd('-');
      return title;
    }
  }
}
