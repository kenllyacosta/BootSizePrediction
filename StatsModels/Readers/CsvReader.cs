using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;

namespace StatsModels.Readers
{
    public class CsvReader
    {
        protected CsvReader() { }

        public static IEnumerable<T> Read<T>(string filePath, char separator = ',') where T : new()
        {
            var Data = new ConcurrentBag<T>();

            var Lines = File.ReadLines(filePath);

            var DataHeaders = Lines.First().Replace("\"", "").Split(separator);
            var DataRows = Lines.Skip(1);

            var PropertiesOfT = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                _ = Parallel.ForEach(DataRows, row =>
                {
                    try
                    {
                        var Values = row.Split(separator);
                        if (Values.Length != DataHeaders.Length)
                        {
                            throw new CsvReaderException($"Error processing line {row}. Column count does not match header count.", null);
                        }

                        var InstanceOfT = new T();
                        for (int i = 0; i < DataHeaders.Length; i++)
                        {
                            var Property = PropertiesOfT.FirstOrDefault(p =>
                            p.Name.Equals(DataHeaders[i].Trim(), StringComparison.OrdinalIgnoreCase));

                            if (Property != null && Property.CanWrite)
                            {
                                var value = Values[i].Replace("\"", "").Trim();

                                if (value == "null")
                                {
                                    Property.SetValue(InstanceOfT, null);
                                }
                                else
                                {
                                    if (Property.PropertyType == typeof(int?))
                                    {
                                        Property.SetValue(InstanceOfT, Convert.ToInt32(value));
                                    }
                                    else if (Property.PropertyType == typeof(string))
                                    {
                                        Property.SetValue(InstanceOfT, value);
                                    }
                                    else if (Property.PropertyType == typeof(TimeSpan))
                                    {
                                        Property.SetValue(InstanceOfT, TimeSpan.Parse(value, CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        Property.SetValue(InstanceOfT, Convert.ChangeType(value, Property.PropertyType));
                                    }
                                }
                            }
                        }

                        Data.Add(InstanceOfT);
                    }
                    catch (Exception ex)
                    {
                        throw new CsvReaderException($"Error processing line {row}.", ex);
                    }
                });
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }

            return Data;
        }
    }

    public class CsvReaderException(string message, Exception innerException) : Exception(message, innerException)
    {
    }
}