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
    [Table("Messages")]
    public class Messages : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime dateCreated { get; set; }

        [Column("senderID")]
        public int senderID { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("takerID")]
        public int takerID { get; set; }
    }
}
