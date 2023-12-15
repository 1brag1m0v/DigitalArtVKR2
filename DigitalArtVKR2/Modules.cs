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
    [Table("Modules")]
    public class Modules : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("courseID")]
        public int courseID { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
