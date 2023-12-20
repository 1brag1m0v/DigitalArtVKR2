using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = Postgrest.Attributes.ColumnAttribute;
using TableAttribute = Postgrest.Attributes.TableAttribute;

namespace DigitalArtVKR2
{
    [Table("LessonsProgress")]
    public class LessonsProgress : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("LessonID")]
        public int lessonId { get; set; }

        [Column("UserID")]
        public int userId { get; set; }
    }
}
