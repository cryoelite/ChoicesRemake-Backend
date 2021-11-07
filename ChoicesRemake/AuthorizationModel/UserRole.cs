using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationModel
{
    public class UserRole
    {
        [Key]
        public string username { get; set; }

        [NotMapped]
        private string _role;

        [Required]
        public string role
        {
            get
            {
                return _role;
            }
            set
            {
                if (value == Role.admin || value == Role.user || value == Role.vendor)
                {
                    _role = value;
                }
            }
        }
    }
}