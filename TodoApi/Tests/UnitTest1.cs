using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void TestRussianPostEnums()
        {
            Assert.AreEqual("text", ResponseFormat.Text.ToRussianPostFormatString());
            Assert.AreEqual("html", ResponseFormat.Html.ToRussianPostFormatString());
            Assert.AreEqual("htmlfull", ResponseFormat.HtmlFull.ToRussianPostFormatString());
            Assert.AreEqual("json", ResponseFormat.Json.ToRussianPostFormatString());
            Assert.AreEqual("jsontext", ResponseFormat.JsonText.ToRussianPostFormatString());
            Assert.AreEqual("easy", ResponseFormat.Easy.ToRussianPostFormatString());
        }
    }

    // TODO Вынести контракты в отдельную сборку
    public enum ResponseFormat
    {
        Text,
        Html,
        HtmlFull,
        Json,
        JsonText,
        Easy
    }

    // TODO Вынести контракты в отдельную сборку
    public static class ResponseFormatExtension
    {
        public static string ToRussianPostFormatString(this ResponseFormat responseFormat)
        {
            return responseFormat.ToString().ToLower();
        }
    }
}
