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
    [System.ComponentModel.DataAnnotations.Schema.Table("Users")]
    public class Users : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("avatarURL")]
        public string avatarURL { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("registerDate")]
        public DateTime registerDate { get; set; }

        [ForeignKey("RoleID")]
        public int RoleID { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
