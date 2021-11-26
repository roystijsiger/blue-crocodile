using AspNetCore.Identity.Mongo.Model;

namespace PinkPanther.BlueCrocodile.WebApplication.Models
{
    public class ApplicationRole : MongoRole
    {
        public ApplicationRole(string name) : base(name)
        {
        }
    }
}
