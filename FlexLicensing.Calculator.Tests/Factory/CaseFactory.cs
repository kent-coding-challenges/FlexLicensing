using CsvHelper.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using FlexLicensing.InputReader.CsvHelper.Reader;

namespace FlexLicensing.Calculator.Tests.Factory
{
    public static class CaseFactory<T, TMap>
        where TMap : CsvClassMap
    {
        public static List<T> ReadCase(string csvFileName)
        {
            string currentAppPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentAppPath, "Cases", csvFileName);

            var reader = new CsvObjectReader<T, TMap>();
            return reader.GetList(filePath);
        }
    }
}
