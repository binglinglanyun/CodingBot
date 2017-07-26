using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopeRuntime;
using SStreamPlusColumnGroup;
using StructuredStream;
using Microsoft.Cosmos.ScopeStudio.BusinessObjects.Common.VCUtility;
using Microsoft.Cosmos.ClientTools.IDECommon;
using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using Microsoft.SqlServer;
using CodingBot.ViewModels;
using VcClient;
namespace CodingBot
{



    internal class DataType
    {
        /// <summary />
        internal DataType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            this.AllowNull = underlyingType != null;

            this.ScopeType = this.AllowNull ? underlyingType : type;

            this.ClrType = FromScopeTypeToClrType(this.ScopeType);

            if (this.ClrType == null)
            {
                var message = string.Format("Data type {0} is not supported", type);

                throw new NotSupportedException(message);
            }

            this.SqlType = FromScopeTypeToSqlType(this.ScopeType);
        }

        /// <summary />
        internal bool AllowNull { get; private set; }

        /// <summary />
        internal Type ClrType { get; private set; }

        /// <summary />
        internal Type ScopeType { get; private set; }

        /// <summary />
        internal string SqlType { get; private set; }

        /// <summary />
        private static void ValidateDataSize(string columnName, int columnIndex, int dataSize)
        {
            if (dataSize > 8000)
            {
                // var message = string.Format(RuntimeContext.FormatProvider, "Column {0} \"{1}\" size ({2} bytes) exceeds the maximum allowed size of 8000 bytes", columnIndex, columnName, dataSize);

                throw new ArgumentOutOfRangeException("error!\n");
            }
        }

        /// <summary />
        internal object GetValueFromRow(IRow row, int index, out int copied)
        {
            copied = 0;

            object retval = null;

            if (this.ScopeType == typeof(long))
            {
                long? longValue = row.Get<long?>(index);
                if (longValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = longValue.Value;
                    copied = sizeof(long);
                }
            }
            else if (this.ScopeType == typeof(SqlInt64))
            {
                SqlInt64 longValue = row.Get<SqlInt64>(index);
                if (longValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = longValue.Value;
                    copied = sizeof(long);
                }
            }
            else if (this.ScopeType == typeof(int))
            {
                int? intValue = row.Get<int?>(index);
                if (intValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = intValue.Value;
                    copied = sizeof(int);
                }
            }
            else if (this.ScopeType == typeof(SqlInt32))
            {
                SqlInt32 intValue = row.Get<SqlInt32>(index);
                if (intValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = intValue.Value;
                    copied = sizeof(int);
                }
            }
            else if (this.ScopeType == typeof(short))
            {
                short? shortValue = row.Get<short?>(index);
                if (shortValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = shortValue.Value;
                    copied = sizeof(short);
                }
            }
            else if (this.ScopeType == typeof(SqlInt16))
            {
                SqlInt16 shortValue = row.Get<SqlInt16>(index);
                if (shortValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = shortValue.Value;
                    copied = sizeof(short);
                }
            }
            else if (this.ScopeType == typeof(byte))
            {
                byte? byteValue = row.Get<byte?>(index);
                if (byteValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = byteValue.Value;
                    copied = 1;
                }
            }
            else if (this.ScopeType == typeof(SqlByte))
            {
                SqlByte byteValue = row.Get<SqlByte>(index);
                if (byteValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = byteValue.Value;
                    copied = 1;
                }
            }
            else if (this.ScopeType == typeof(string))
            {
                string stringValue = row.Get<string>(index);
                if (stringValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    copied = Encoding.UTF8.GetByteCount(stringValue);

                    ValidateDataSize(row.Schema[index].Name, index, copied);

                    retval = Encoding.UTF8.GetBytes(stringValue);
                }
            }
            else if (this.ScopeType == typeof(byte[]))
            {
                byte[] binaryValue = row.Get<byte[]>(index);
                if (binaryValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    copied = binaryValue.Length;

                    ValidateDataSize(row.Schema[index].Name, index, copied);

                    retval = binaryValue;
                }
            }
            else if (this.ScopeType == typeof(SqlString))
            {
                SqlString stringValue = row.Get<SqlString>(index);
                if (stringValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    copied = Encoding.UTF8.GetByteCount(stringValue.Value);

                    ValidateDataSize(row.Schema[index].Name, index, copied);

                    retval = Encoding.UTF8.GetBytes(stringValue.Value);
                }
            }
            else if (this.ScopeType == typeof(char))
            {
                char? charValue = row.Get<char?>(index);
                if (charValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    var tmpChar = charValue.Value.ToString();
                    retval = Encoding.UTF8.GetBytes(tmpChar);
                    copied = Encoding.UTF8.GetByteCount(tmpChar);
                }
            }
            else if (this.ScopeType == typeof(DateTime))
            {
                DateTime? dateTimeValue = row.Get<DateTime?>(index);
                if (dateTimeValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = dateTimeValue.Value;
                    copied = 8;
                }
            }
            else if (this.ScopeType == typeof(SqlDate))
            {
                SqlDate dateTimeValue = row.Get<SqlDate>(index);
                if (dateTimeValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = dateTimeValue.Value;
                    copied = 8;
                }
            }
            else if (this.ScopeType == typeof(decimal))
            {
                decimal? decimalValue = row.Get<decimal?>(index);
                if (decimalValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = DecimalSerializationHelper.SerializeDecToByteArray(decimalValue.Value);
                    copied = DecimalSerializationHelper.StoredDecimalSize;
                }
            }
            else if (this.ScopeType == typeof(SqlDecimal))
            {
                SqlDecimal decimalValue = row.Get<SqlDecimal>(index);
                if (decimalValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = decimalValue.Value;
                    copied = sizeof(decimal);
                }
            }
            else if (this.ScopeType == typeof(bool))
            {
                bool? boolValue = row.Get<bool?>(index);
                if (boolValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = boolValue.Value;
                    copied = 1;
                }
            }
            else if (this.ScopeType == typeof(SqlBit))
            {
                SqlBit boolValue = row.Get<SqlBit>(index);
                if (boolValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = boolValue.Value;
                    copied = 1;
                }
            }
            else if (this.ScopeType == typeof(float))
            {
                float? floatValue = row.Get<float?>(index);
                if (floatValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = floatValue.Value;
                    copied = sizeof(float);
                }
            }
            else if (this.ScopeType == typeof(double))
            {
                double? doubleValue = row.Get<double?>(index);
                if (doubleValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = doubleValue.Value;
                    copied = sizeof(double);
                }
            }
            else if (this.ScopeType == typeof(sbyte))
            {
                sbyte? sbyteValue = row.Get<sbyte?>(index);
                if (sbyteValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = sbyteValue.Value;
                    copied = sizeof(sbyte);
                }
            }
            else if (this.ScopeType == typeof(uint))
            {
                uint? uintValue = row.Get<uint?>(index);
                if (uintValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = uintValue.Value;
                    copied = sizeof(uint);
                }
            }
            else if (this.ScopeType == typeof(ushort))
            {
                ushort? ushortValue = row.Get<ushort?>(index);
                if (ushortValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = ushortValue.Value;
                    copied = sizeof(ushort);
                }
            }
            else if (this.ScopeType == typeof(SqlGuid))
            {
                SqlGuid guidValue = row.Get<SqlGuid>(index);
                if (guidValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = guidValue.Value;
                    copied = 16;
                }
            }
            else if (this.ScopeType == typeof(Guid))
            {
                Guid? guidValue = row.Get<Guid?>(index);
                if (guidValue == null)
                {
                    retval = DBNull.Value;
                }
                else
                {
                    retval = guidValue.Value;
                    copied = 16;
                }
            }

            return retval;
        }

        /// <summary />
        internal static Type FromScopeTypeToClrType(Type type)
        {
            if (type == typeof(long) || type == typeof(SqlInt64))
            {
                return typeof(long);
            }
            else if (type == typeof(int) || type == typeof(SqlInt32))
            {
                return typeof(int);
            }
            else if (type == typeof(short) || type == typeof(SqlInt16))
            {
                return typeof(short);
            }
            else if (type == typeof(byte) || type == typeof(SqlByte))
            {
                return typeof(byte);
            }
            else if (type == typeof(string) || type == typeof(SqlString))
            {
                // TODO: varbinary(max)
                return typeof(byte[]);
            }
            else if (type == typeof(byte[]))
            {
                return typeof(byte[]);
            }
            else if (type == typeof(char))
            {
                // We use 6 because one character takes 6 bytes max in UTF8
                return typeof(byte[]);
            }
            else if (type == typeof(DateTime) || type == typeof(SqlDate))
            {
                return typeof(DateTime);
            }
            else if (type == typeof(decimal) || type == typeof(SqlDecimal))
            {
                return typeof(byte[]);
            }
            else if (type == typeof(bool) || type == typeof(SqlBit))
            {
                return typeof(bool);
            }
            else if (type == typeof(float))
            {
                return typeof(float);
            }
            else if (type == typeof(double))
            {
                return typeof(double);
            }
            else if (type == typeof(sbyte))
            {
                return typeof(sbyte);
            }
            else if (type == typeof(uint))
            {
                return typeof(uint);
            }
            else if (type == typeof(ushort))
            {
                return typeof(ushort);
            }
            else if (type == typeof(Guid) || type == typeof(SqlGuid))
            {
                return typeof(Guid);
            }
            else
            {
                return null;
            }
        }

        /// <summary />
        internal static string FromScopeTypeToSqlType(Type type)
        {
            if (type == typeof(long) || type == typeof(SqlInt64))
            {
                return "bigint";
            }
            else if (type == typeof(int) || type == typeof(SqlInt32))
            {
                return "int";
            }
            else if (type == typeof(short) || type == typeof(SqlInt16))
            {
                return "smallint";
            }
            else if (type == typeof(byte) || type == typeof(SqlByte))
            {
                return "tinyint";
            }
            else if (type == typeof(string) || type == typeof(SqlString))
            {
                // TODO: varbinary(max)
                return "varbinary(8000)";
            }
            else if (type == typeof(byte[]))
            {
                return "varbinary(8000)";
            }
            else if (type == typeof(char))
            {
                // We use 6 because one character takes 6 bytes max in UTF8
                return "varbinary(6)";
            }
            else if (type == typeof(DateTime) || type == typeof(SqlDate))
            {
                return "datetime2";
            }
            else if (type == typeof(decimal) || type == typeof(SqlDecimal))
            {
                return "binary(16)";
            }
            else if (type == typeof(bool) || type == typeof(SqlBit))
            {
                return "bit";
            }
            else if (type == typeof(float))
            {
                return "real";
            }
            else if (type == typeof(double))
            {
                return "float";
            }
            else if (type == typeof(sbyte))
            {
                return "smallint";
            }
            else if (type == typeof(uint))
            {
                return "bigint";
            }
            else if (type == typeof(ushort))
            {
                return "int";
            }
            else if (type == typeof(Guid) || type == typeof(SqlGuid))
            {
                return "uniqueidentifier";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Custom serialization for decimal type. This serialization preserves ordering, so we can push comparison operators.
        /// We are storing decimal as binary(16) in sql server.
        /// Since sql server compares binaries, byte by byte from left to right we are storing values in following manner:
        ///  - First bit is sign (highest bit in first byte) - 0 is negative, 1 is positive
        ///  - Byte with index 1 is number of digits in integer part of a decimal (0 for zero)
        ///  - Bytes 2 and 3 are unused.
        ///  - Bytes from 4 to 15 are representing the actual value of decimal with added trailing zeroes, so that total number of digits is 29.
        ///    Highest byte is stored first, lowest byte is last.
        /// If the number is negative, all bits are inverted.
        /// e.g.:   value                           binary
        ///         23.4                            800200004B9C06BDCEDBB8C468000000
        ///         79228162514264337593543950335   801D0000FFFFFFFFFFFFFFFFFFFFFFFF (max value)
        ///         1                               80010000204FCE5E3E25026110000000
        ///         -1                              7FFEFFFFDFB031A1C1DAFD9EEFFFFFFF
        /// </summary>
        class DecimalSerializationHelper
        {
            /// <summary>
            /// Size of decimal stored in sql server in bytes.
            /// </summary>
            public const int StoredDecimalSize = 16;

            /// <summary>
            /// Size of decimal stored in sql server in uints.
            /// </summary>
            private const int StoredDecimalSizeInUints = 4;

            /// <summary>
            /// Max scale of decimal.
            /// </summary>
            private const int MaxDecimalDigits = 29;

            /// <summary>
            /// Mask for sign.
            /// </summary>
            private const int SignMask = unchecked((int)0x80000000);

            /// <summary>
            /// When multiplied with this const, a decimal number has largest scale without overflowing.
            /// </summary>
            private const decimal ShiftLeftDecimalHelper = 1.0000000000000000000000000000m; // 28 zeroes

            /// <summary>
            /// Scale mask for the flags field.
            /// </summary>
            private const int ScaleMask = 0x00FF0000;

            /// <summary>
            /// Number of bits scale is shifted by.
            /// </summary>
            private const int ScaleShift = 16;

            /// <summary>
            /// Method for serializing decimal to byte array. See the comment above for explanation.
            /// </summary>
            public static byte[] SerializeDecToByteArray(decimal inpDecimal)
            {
                // Shift decimal to the left by multiplying it with 1.0000000000000000000000000000m (28 zeroes).
                // e.g. 1107671.873889 becomes 1107671.8738890000000000000000
                inpDecimal *= ShiftLeftDecimalHelper;

                int[] decimalBits = decimal.GetBits(inpDecimal);

                if (!inpDecimal.Equals(decimal.Zero))
                {
                    // Replace scale with number of digits in integer part of decimal.
                    int wholeNumberDigits = MaxDecimalDigits - ((decimalBits[3] & ScaleMask) >> ScaleShift);
                    decimalBits[3] = (decimalBits[3] & ~ScaleMask) | (wholeNumberDigits << ScaleShift);
                }

                if (decimalBits[3] < 0)
                {
                    // In case of a negative number, bitwise negate entire number.
                    for (int k = 0; k < StoredDecimalSizeInUints; k++)
                    {
                        decimalBits[k] = ~decimalBits[k];
                    }
                }
                else
                {
                    // In case of a positive number, set highest bit to 1, so that all positve numbers are ordered higher than negative numbers.
                    decimalBits[3] |= SignMask;
                }

                byte[] decimalBinary = new byte[StoredDecimalSize];

                Buffer.BlockCopy(decimalBits, 0, decimalBinary, 0, StoredDecimalSize);

                // Invert the array, so that highest byte is on index 0 of the array.
                for (int k = 0; k < StoredDecimalSize / 2; k++)
                {
                    decimalBinary[k] ^= decimalBinary[StoredDecimalSize - 1 - k];
                    decimalBinary[StoredDecimalSize - 1 - k] ^= decimalBinary[k];
                    decimalBinary[k] ^= decimalBinary[StoredDecimalSize - 1 - k];
                }

                return decimalBinary;
            }
        }
    }

    public static class DataTypeIdentifier
    {
        /// <summary>
        /// Identify the data type for <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value to identify the type for.</param>
        /// <returns>The detected data type.</returns>
        public static string IdentifyToDataType(string value)
        {
            bool boolValue;
            if (bool.TryParse(value.ToLowerInvariant(), out boolValue))
            {
                return "bool";
            }

            int intValue;
            if (int.TryParse(value, out intValue))
            {
                return "int";
            }

            long longValue;
            if (long.TryParse(value, out longValue))
            {
                return "long";
            }

            ulong ulong64Value;
            if (ulong.TryParse(value, out ulong64Value))
            {
                return "ulong";
            }

            double doubleValue;
            if (double.TryParse(value, out doubleValue))
            {
                return "double";
            }

            DateTime dateValue;
            if (DateTime.TryParse(value, out dateValue))
            {
                return "datetime";
            }

            Guid guid;
            if (Guid.TryParse(value, out guid))
            {
                return "guid";
            }

            return "string";
        }




    }




    public class PathParser
    {
        public PathParser()
        {

        }
        
        public  TableItem ParseFilePath(int index, string inpath)
        {
            string VC_path = "https://cosmos08.osdinfra.net/cosmos/bingads.algo.incubation";
            TableItem res_table;
            if (inpath.IndexOf(".ss") == inpath.Length - 3)
            {
                Stream stream = Factory.VCClientStreamFactory.OpenReadOnlyStream(VC_path+inpath);
                SStreamPlusColumnGroup.IStreamMetadataReader streamMetadataReader = Factory.CreateStreamMetadataReader();
                streamMetadataReader.Open(stream, VC_path+inpath);
                StructuredStream.StructuredStreamSchema sstreamSchema = streamMetadataReader.Metadata.Schema;
                List<string> table_name = new List<string>();
                List<string> col_name = new List<string>();
                List<string> col_type = new List<string>();

                table_name.Add("( SSTREAM @inpath"+index.ToString()+" )");
                List<ColumnInfo> col_list = sstreamSchema.ScopeSchema.Columns.ToList();
                for (int i = 0; i <= col_list.Count - 1; i++)
                {

                    col_name.Add(col_list[i].Name.ToString());
                    col_type.Add(col_list[i].Type.ToString().ToLower());
                    
                }
                res_table = new TableItem(table_name,col_name,col_type);
            }
            else
            {
                //VcWrapper vc = new VcWrapper();
                //VcWrapper VC = new VcWrapper();
                Stream stream2 = VC.ReadStream(VC_path+inpath, 0, 1000, true);
                using (StreamReader reader = new StreamReader(stream2))
                {
                    int currentRow = 0;
                    List<string> table_name = new List<string>();
                    List<string> col_name = new List<string>();
                    List<string> col_type = new List<string>();
                    table_name.Add("@inpath"+index.ToString());
                    while (!reader.EndOfStream && currentRow < 2)
                    {
                        currentRow++;
                        string line = reader.ReadLine();
                        string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                        for (int i = 0; i <= parts.Length - 1; i++)
                        {
                            col_name.Add("column_" + i.ToString());
                            col_type.Add(DataTypeIdentifier.IdentifyToDataType(parts[i]).ToLower());
                        }
                    }
                    res_table = new TableItem(table_name, col_name, col_type);
                }
            }
            return res_table;

        }
    }

}
