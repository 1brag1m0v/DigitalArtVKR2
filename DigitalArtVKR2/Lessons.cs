using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = Postgrest.Attributes.ColumnAttribute;
using TableAttribute = Postgrest.Attributes.TableAttribute;

namespace DigitalArtVKR2
{
    [Table("Lessons")]
    public class Lessons : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("moduleID")]
        public int moduleID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("media")]
        public string Media { get; set; }

        [Column("text")]
        public string Text { get; set; }
    }
}
