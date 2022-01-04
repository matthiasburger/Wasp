
using System.ComponentModel.DataAnnotations.Schema;

namespace wasp.WebApi.Data.Models.Schema
{
    public class TableColumn
    {
        /*
        [Column("TABLE_CATALOG")]
        public string TableCatalog { get; set; } = null!;
        
        [Column("TABLE_SCHEMA")]
        public string TableSchema { get; set; } = null!;
        */

        [Column("TABLE_NAME")] 
        public string TableName { get; set; } = null!;
        
        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; } = null!;
        
        [Column("IS_NULLABLE")]
        public bool IsNullable { get; set; }
        
        /*
        [Column("ORDINAL_POSITION")]
        public int OrdinalPosition { get; set; }
        
        [Column("COLUMN_DEFAULT")]
        public string? ColumnDefault { get; set; }
        

        [Column("DATA_TYPE")] 
        public string DataType { get; set; } = null!;
        
        [Column("CHARACTER_MAXIMUM_LENGTH")]
        public int? CharacterMaximumLength { get; set; }
        
        [Column("CHARACTER_OCTET_LENGTH")]
        public int? CharacterOctetLength { get; set; }
        
        [Column("NUMERIC_PRECISION")]
        public int? NumericPrecision { get; set; }
        
        [Column("NUMERIC_PRECISION_RADIX")]
        public int? NumericPrecisionRadix { get; set; }
        
        [Column("NUMERIC_SCALE")]
        public int? NumericScale { get; set; }
        
        [Column("DATETIME_PRECISION")]
        public int? DatetimePrecision { get; set; }
        
        [Column("CHARACTER_SET_CATALOG")]
        public string? CharacterSetCatalog { get; set; }
        
        [Column("CHARACTER_SET_SCHEMA")]
        public string? CharacterSetSchema { get; set; }
        
        [Column("CHARACTER_SET_NAME")]
        public string? CharacterSetName { get; set; }
        
        [Column("COLLATION_SCHEMA")]
        public string? CollationSchema { get; set; }
        
        [Column("COLLATION_CATALOG")]
        public string? CollationCatalog { get; set; }
        
        [Column("COLLATION_NAME")]
        public string? CollationName { get; set; }
        */
    }
}