using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = Postgrest.Attributes.ColumnAttribute;

namespace DigitalArtVKR2
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Courses")]
    public class Courses : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("photo")]
        public string Photo { get; set; }

        [Column("difficultID")]
        public int difficultID { get; set; }
    }
}
