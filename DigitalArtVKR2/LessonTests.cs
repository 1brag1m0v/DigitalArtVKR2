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
    [Table("LessonTests")]
    public class LessonTests : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("lesson")]
        public int idLesson { get; set; }

        [Column("data")]
        public string jsonData { get; set; }
    }
}
