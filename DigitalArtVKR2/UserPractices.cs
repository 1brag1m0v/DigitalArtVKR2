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
    [Table("UserPractices")]
    public class UserPractices : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }
        [Column("user")]
        public int idUser { get; set; }

        [Column("lesson")]
        public int lessonId { get; set; }

        [Column("text")]
        public string text { get; set; }

        [Column("fileLinks")]
        public string fileLinks { get; set; }

        public string nameUser { get; set; }
    }
}
