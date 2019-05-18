namespace Chatter.Auth.MongoIdentity.Options
{
    public class MongoIdentityOptions
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
        public string UsersCollection { get; set; } = "Users";
        public string RolesCollection { get; set; } = "Roles";
        public bool UseDefaultIdentity { get; set; } = true;
    }
}
