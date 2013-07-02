using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FileHelpers;
using SPCASW.Common.Attributes;
using SPCASW.Marketing.Models;

namespace SPCASW.Marketing
{
    public static class ConstantContactFileEngine
    {
        public const string FILE_DELIMITER = ",";

        public static void WriteToStream(Stream stream, IEnumerable<ConstantContactRecord> records)
        {
            FileHelperEngine<ConstantContactRecord> engine = null;

            try
            {
                engine = new FileHelperEngine<ConstantContactRecord>();
                engine.HeaderText = BuildHeaderText<ConstantContactRecord>();

                TextWriter writer = null;

                try
                {
                    writer = new StreamWriter(stream, Encoding.ASCII);
                    engine.WriteStream(writer, records);

                    writer.Flush();
                }
                finally
                {
                    writer = null;
                }
            }
            finally
            {
                engine = null;
            }
        }

        private static string BuildHeaderText<T>()
        {
            StringBuilder sb = new StringBuilder();

            // Loop through the fields on the object (must be fields, cannot be properties) and pull the fields that should be put into the output file.
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance).ToList();
            for (int index = 0; index < fields.Count; index++)
            {
                FieldInfo current = fields[index];
                if (current == null)
                {
                    continue;
                }

                string name = null;

                var attribute = (HeaderTextAttribute)current.GetCustomAttributes(typeof(HeaderTextAttribute), true).FirstOrDefault();
                if (attribute == null)
                {
                    name = current.Name;
                }
                else
                {
                    name = attribute.Text;
                }

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                else
                {
                    sb.AppendFormat("\"{0}\"", name);

                    if (index < fields.Count - 1)
                    {
                        sb.Append(FILE_DELIMITER);
                    }
                }
            }

            return sb.ToString();
        }
    }
}